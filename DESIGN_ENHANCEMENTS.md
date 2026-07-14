# GeneticSharp Design Enhancements

Identified during FSC.Traffic Phase 4 (GA optimization) preparation alongside the performance
analysis in `PERFORMANCE_ENHANCEMENTS.md`.

---

## Gene Ordering — Chromosome/Operator Compatibility

### Problem

`CrossoverBase` and `MutationBase` both have an `IsOrdered: bool` property, but:

- It is not on the interfaces (`ICrossover`, `IMutation`) — cannot be queried without a cast.
- It is binary — cannot express the three distinct structural guarantees chromosomes can make.
- `IChromosome` has no matching property — the chromosome never declares what it needs.
- The only enforcement today is a runtime `AnyHasRepeatedGene()` exception thrown deep inside
  permutation crossovers, after the GA has already started running.

### The Three Gene Orderings

| | Positions encode meaning | Must contain all genes | No repeats |
|---|---|---|---|
| **Positional** | ✓ | — | — |
| **Set** | — | ✓ | ✓ |
| **Permutation** | ✓ | ✓ | ✓ |

**Positional** — each index is an independent named slot. Index `i` always means a specific
parameter. Genes may repeat across slots. No coverage requirement. Any operator is valid.

**Set** — every gene must appear exactly once, but positions are interchangeable. The chromosome
is valid as long as it is complete. Permutation-safe operators work; slot-swapping operators
(Uniform, OnePoint, TwoPoint) break coverage and are invalid.

**Permutation** — every gene appears exactly once AND the sequence encodes meaning. The strictest
level. Only permutation-aware operators (OX1, PMX, CycleCrossover, etc.) are valid.

Compatibility rule: an operator's `RequiredOrdering` must be ≤ the chromosome's `GeneOrdering`.
A chromosome satisfying a stricter constraint automatically satisfies any looser one:

```
Positional (0) ≤ Set (1) ≤ Permutation (2)
```

A `Permutation` chromosome is compatible with any operator. A `Positional` chromosome is only
compatible with operators that require `Positional`.

### Operator Compatibility Matrix

| Operator | Positional | Set | Permutation |
|---|---|---|---|
| UniformCrossover | ✓ | ✗ | ✗ |
| OnePointCrossover | ✓ | ✗ | ✗ |
| TwoPointCrossover | ✓ | ✗ | ✗ |
| CutAndSpliceCrossover | ✓ | ✗ | ✗ |
| VotingRecombinationCrossover | ✓ | ✗ | ✗ |
| OrderedCrossover (OX1) | ✓ | ✓ | ✓ |
| PartiallyMappedCrossover (PMX) | ✓ | ✓ | ✓ |
| CycleCrossover | ✓ | ✓ | ✓ |
| OrderBasedCrossover | ✓ | ✓ | ✓ |
| PositionBasedCrossover | ✓ | ✓ | ✓ |
| AlternatingPositionCrossover | ✓ | ✓ | ✓ |
| FlipBitMutation | ✓ | ✗ | ✗ |
| UniformMutation | ✓ | ✗ | ✗ |
| PartialShuffleMutation | ✓ | ✓ | ✓ |
| ReverseSequenceMutation | ✓ | ✓ | ✓ |
| DisplacementMutation | ✓ | ✓ | ✓ |
| InsertionMutation | ✓ | ✓ | ✓ |
| TworsMutation | ✓ | ✓ | ✓ |

### Proposed Implementation

**Step 1 — Add the enum:**

```csharp
namespace GeneticSharp
{
    public enum GeneOrdering
    {
        Positional  = 0,  // each slot independent; genes may repeat; no coverage constraint
        Set         = 1,  // all genes present exactly once; positions interchangeable
        Permutation = 2   // all genes present exactly once; position encodes meaning
    }
}
```

**Step 2 — Declare on `IChromosome`:**

```csharp
public interface IChromosome : IComparable<IChromosome>
{
    GeneOrdering GeneOrdering { get; }   // add this
    // ... existing members unchanged
}
```

Default in `ChromosomeBase` (non-breaking — existing subclasses inherit `Positional`):

```csharp
public virtual GeneOrdering GeneOrdering => GeneOrdering.Positional;
```

**Step 3 — Replace `IsOrdered: bool` on operators:**

Add `RequiredOrdering` to `ICrossover` and `IMutation`:

```csharp
public interface ICrossover : IChromosomeOperator
{
    GeneOrdering RequiredOrdering { get; }   // replaces IsOrdered
    // ... existing members unchanged
}

public interface IMutation : IChromosomeOperator
{
    GeneOrdering RequiredOrdering { get; }   // replaces IsOrdered
}
```

Update `CrossoverBase` and `MutationBase`:

```csharp
// CrossoverBase — replace:
public bool IsOrdered { get; protected set; }
// with:
public GeneOrdering RequiredOrdering { get; protected set; }  // default: Positional

// Derived classes that currently set IsOrdered = true set Permutation instead:
//   OrderedCrossover, PartiallyMappedCrossover, CycleCrossover, etc.
//   → RequiredOrdering = GeneOrdering.Permutation
```

**Step 4 — Enforce in `CrossoverBase.Cross()`:**

```csharp
public IList<IChromosome> Cross(IList<IChromosome> parents)
{
    ExceptionHelper.ThrowIfNull("parents", parents);

    if (parents.Count != ParentsNumber)
        throw new ArgumentOutOfRangeException(...);

    var firstParent = parents[0];

    if (firstParent.Length < MinChromosomeLength)
        throw new CrossoverException(...);

    // New: ordering compatibility check
    if ((int)firstParent.GeneOrdering < (int)RequiredOrdering)
    {
        throw new CrossoverException(this,
            $"{GetType().Name} requires {RequiredOrdering} gene ordering, " +
            $"but chromosome {firstParent.GetType().Name} declares {firstParent.GeneOrdering}.");
    }

    return PerformCross(parents);
}
```

Same check in `MutationBase.Mutate()`.

**Step 5 — Add a pre-run compatibility validator:**

```csharp
public static class GeneticAlgorithmValidator
{
    public static void ValidateOperatorCompatibility(
        IChromosome chromosome, ICrossover crossover, IMutation mutation)
    {
        if ((int)chromosome.GeneOrdering < (int)crossover.RequiredOrdering)
            throw new InvalidOperationException(
                $"Crossover {crossover.GetType().Name} requires {crossover.RequiredOrdering} " +
                $"but chromosome {chromosome.GetType().Name} declares {chromosome.GeneOrdering}.");

        if ((int)chromosome.GeneOrdering < (int)mutation.RequiredOrdering)
            throw new InvalidOperationException(
                $"Mutation {mutation.GetType().Name} requires {mutation.RequiredOrdering} " +
                $"but chromosome {chromosome.GetType().Name} declares {chromosome.GeneOrdering}.");
    }
}
```

Call this from `GeneticAlgorithm.Start()` before `CreateInitialGeneration()`.

---

## Compile-Time Enforcement — Not Worth It

The natural follow-on question is whether this can be a compile error rather than a runtime one.
The answer is: not without making `GeneticAlgorithm` and all its dependencies generic on
`TChromosome`, which is a pervasive change that touches every interface in the library.

The mechanism would be:

```csharp
// Additive typed crossover interface
public interface ICrossover<in TChromosome> : ICrossover where TChromosome : IChromosome { }

// Permutation crossovers declare their constraint via the type parameter
public class OrderedCrossover : CrossoverBase, ICrossover<IPermutationChromosome> { ... }

// GA becomes generic — compiler links chromosome type to crossover type
public class GeneticAlgorithm<TChromosome> where TChromosome : IChromosome
{
    public GeneticAlgorithm(..., ICrossover<TChromosome> crossover, ...) { }
}
```

This would produce a compile error when pairing `OrderedCrossover` (which implements
`ICrossover<IPermutationChromosome>`) with a positional chromosome. But it requires `IPopulation`,
`ISelection`, `IReinsertion`, `IGenerationStrategy`, and the task executor infrastructure to all
become generic — a library-wide refactor with significant breaking change surface.

**Decision: not worth it for an internal tool.** The runtime check at `GeneticAlgorithm.Start()`
catches the mistake before any generation runs, which is early enough.

---

## FSC.Traffic Chromosome Classification

The Traffic chromosome is **composite** — different sections have different orderings:

**FIXED_SLOT sections → Positional.** Continuous genes (`split`, `offset`, `walk_time`, etc.)
and categorical genes (`recall_mode`, `fya_mode`) are positional: index `i` always means a
specific named parameter for a specific phase at a specific intersection. Values are independent,
may repeat across slots, no coverage constraint.

**PERMUTATION sections → Permutation.** Intra-barrier phase ordering within each intersection's
barrier groups. Every phase in the barrier group must appear exactly once, and the sequence
determines when each phase receives green — what the GA is evolving for green wave coordination.
Barrier membership is fixed (a phase cannot move between barriers); only intra-barrier sequence
is evolvable.

**Top-level declaration:** The chromosome declares `GeneOrdering.Permutation` (the most
restrictive section wins) to prevent accidentally pairing it with slot-swapping operators like
`UniformCrossover`.

**Important caveat:** The Traffic PERMUTATION sections are not one flat permutation — they are
multiple independent sub-permutations, one per barrier group per intersection. No standard OX1 or
PMX understands that structure. Custom crossover operators are required regardless, and they handle
section routing internally. The framework-level `GeneOrdering` declaration is a guard against
gross mismatches, not a routing mechanism.
