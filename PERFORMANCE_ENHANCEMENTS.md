# GeneticSharp Performance Enhancements

Identified during FSC.Traffic Phase 4 (GA optimization) preparation. All findings are based on
static analysis of the domain library as it stands; no benchmarks were run. Ordered by estimated
impact.

---

## On Making Gene Generic

`Gene` is a struct wrapping `object m_value`. Every value-type gene value (double, int, enum, bool)
is boxed on construction. Making `Gene<T>` would eliminate that boxing, but only when all genes in
a chromosome share the same T — which is not the case for mixed-type chromosomes (e.g. a chromosome
with float splits, enum recall modes, and bool enable flags all in the same `Gene[]`).

**Verdict: not worth it.** The boxing pressure occurs at chromosome construction and cloning, not
per fitness evaluation. If profiling ever shows Gene allocation is a bottleneck, the correct fix is
typed parallel arrays on the chromosome itself, not `Gene<T>`.

---

## High Impact

### 1. Wrong randomizer is active

**File:** `Randomizations/RandomizationProvider.cs`

```csharp
// Current (slow):
Current = new BasicRandomization();

// FastRandomRandomization line is commented out:
//Current = new FastRandomRandomization();
```

`FastRandomRandomization` wraps a thread-local `SharpNeatLib.Maths.FastRandom`, which is
significantly faster than `System.Random`. The randomizer fires on every gene probability check
during crossover and every mutation candidate check — thousands of times per generation.

**Fix:** Set `RandomizationProvider.Current = new FastRandomRandomization()` at GA startup in
the caller. No changes to the library required.

---

### 2. `RouletteWheelSelection.SelectFromWheel` — linear scan with anonymous-object allocations

**File:** `Selections/RouletteWheelSelection.cs:52`

```csharp
// Current: allocates anonymous object per element, linear scan
var chromosome = rouletteWheel
    .Select((value, index) => new { Value = value, Index = index })
    .FirstOrDefault(r => r.Value >= pointer);
```

`rouletteWheel` is a pre-built sorted cumulative array — binary search is correct here and has
zero allocations.

**Fix:**
```csharp
protected static IList<IChromosome> SelectFromWheel(
    int number, IList<IChromosome> chromosomes, IList<double> rouletteWheel, Func<double> getPointer)
{
    var selected = new List<IChromosome>(number);
    var wheel = (rouletteWheel as List<double>) ?? new List<double>(rouletteWheel);

    for (int i = 0; i < number; i++)
    {
        var pointer = getPointer();
        int index = wheel.BinarySearch(pointer);
        if (index < 0) index = ~index;           // first entry >= pointer
        if (index >= wheel.Count) index = wheel.Count - 1;
        selected.Add(chromosomes[index].Clone());
    }

    return selected;
}
```

---

### 3. `TournamentSelection` — O(n) index scan and sort-to-get-max

**File:** `Selections/TournamentSelection.cs:90-91`

```csharp
// Current:
var randomIndexes = RandomizationProvider.Current.GetUniqueInts(Size, 0, candidates.Count);
var tournamentWinner = candidates
    .Where((c, i) => randomIndexes.Contains(i))      // O(n) scan per candidate
    .OrderByDescending(c => c.Fitness).First();       // full sort just to get max
```

Two problems: `int[].Contains` is O(n) per element; `OrderByDescending().First()` allocates a
sorted sequence to retrieve one element.

**Fix:**
```csharp
var randomIndexes = RandomizationProvider.Current.GetUniqueInts(Size, 0, candidates.Count);
var indexSet = new HashSet<int>(randomIndexes);

IChromosome winner = null;
foreach (var (c, i) in candidates.Select((c, i) => (c, i)))
{
    if (!indexSet.Contains(i)) continue;
    if (winner == null || c.Fitness > winner.Fitness) winner = c;
}

selected.Add(winner.Clone());
```

Or more simply, since `randomIndexes` has length `Size` (small), just iterate `randomIndexes`
directly:

```csharp
var randomIndexes = RandomizationProvider.Current.GetUniqueInts(Size, 0, candidates.Count);
IChromosome winner = candidates[randomIndexes[0]];
for (int j = 1; j < randomIndexes.Length; j++)
{
    var c = candidates[randomIndexes[j]];
    if (c.Fitness > winner.Fitness) winner = c;
}
selected.Add(winner.Clone());
```

---

### 4. Double sort per generation

**Files:** `GeneticAlgorithm.cs:419`, `Populations/Generation.cs:72`

`EvaluateFitness` sorts chromosomes descending by fitness:
```csharp
Population.CurrentGeneration.Chromosomes =
    Population.CurrentGeneration.Chromosomes.OrderByDescending(c => c.Fitness.Value).ToList();
```

`Generation.End` (called immediately after) sorts them again:
```csharp
Chromosomes = Chromosomes.Where(ValidateChromosome).OrderByDescending(c => c.Fitness.Value).ToList();
```

**Fix:** Remove the sort in `EvaluateFitness`. Leave `Generation.End` as the single sort site.
`ValidateChromosome` only throws on missing fitness (which `EvaluateFitness` already guarantees),
so the `.Where` in `End` can also be removed in practice, but leave it for safety.

---

## Medium Impact

### 5. `SelectParentsAndCross` — list allocation per crossover pair

**File:** `OperatorsStrategy/OperatorsStrategyBase.cs:42`

```csharp
// Current: allocates a new List for every pair (N/2 times per generation)
var selectedParents = parents.Skip(firstParentIndex).Take(crossover.ParentsNumber).ToList();
```

**Fix:** Index directly without allocating. For the common 2-parent case:
```csharp
// For crossover.ParentsNumber == 2 (most crossovers):
var selectedParents = new List<IChromosome>(crossover.ParentsNumber);
for (int j = firstParentIndex; j < firstParentIndex + crossover.ParentsNumber && j < parents.Count; j++)
    selectedParents.Add(parents[j]);
```

Or pass an `ArraySegment<IChromosome>` / `IReadOnlyList<IChromosome>` slice if the `ICrossover`
interface is willing to accept it, avoiding the extra list entirely.

---

### 6. `GetUniqueInts` — `List<int>.RemoveAt` is O(n)

**File:** `Randomizations/RandomizationBase.cs:59-80`

```csharp
// Current: RemoveAt shifts elements on every draw
var orderedValues = Enumerable.Range(min, diff).ToList();
for (int i = 0; i < length; i++)
{
    var removeIndex = GetInt(0, orderedValues.Count);
    ints[i] = orderedValues[removeIndex];
    orderedValues.RemoveAt(removeIndex);       // O(diff) shift
}
```

For small `length` (e.g. 2 cut points) the list shift is ~4 element moves — negligible. Matters
when `TournamentSelection.Size` is large.

**Fix:** Partial Fisher-Yates in-place on a pre-allocated array:
```csharp
public override int[] GetUniqueInts(int length, int min, int max)
{
    var diff = max - min;
    if (diff < length) throw new ArgumentOutOfRangeException(...);

    var pool = new int[diff];
    for (int i = 0; i < diff; i++) pool[i] = min + i;

    var result = new int[length];
    for (int i = 0; i < length; i++)
    {
        int j = GetInt(i, diff);
        (pool[i], pool[j]) = (pool[j], pool[i]);
        result[i] = pool[i];
    }
    return result;
}
```

---

### 7. `OrderedCrossover.CreateChild` — lazy `Except` over a lazy query

**File:** `Crossovers/OrderedCrossover.cs:89-91`

```csharp
// Current: middleSectionGenes is lazy; Except builds its exclusion set from a re-enumerated chain
var middleSectionGenes = firstParent.GetGenes()
    .Skip(middleSectionBeginIndex)
    .Take((middleSectionEndIndex - middleSectionBeginIndex) + 1);   // still lazy

using (var secondParentRemainingGenes = secondParent.GetGenes().Except(middleSectionGenes).GetEnumerator())
```

**Fix:** Materialize before `Except`:
```csharp
var middleSectionGenes = firstParent.GetGenes()
    .Skip(middleSectionBeginIndex)
    .Take((middleSectionEndIndex - middleSectionBeginIndex) + 1)
    .ToArray();   // <-- add this

using (var secondParentRemainingGenes = secondParent.GetGenes().Except(middleSectionGenes).GetEnumerator())
```

---

### 8. `VotingRecombinationCrossover` — `GroupBy` allocated per gene position

**File:** `Crossovers/VotingRecombinationCrossover.cs:75-79`

```csharp
// Current: GroupBy + OrderByDescending on every gene position i
var moreOcurrencesGeneValue = parents
    .GroupBy(p => p.GetGene(i).Value)
    .Where(p => p.Count() >= _threshold)
    .OrderByDescending(g => g.Count())
    .FirstOrDefault();
```

For a 50-gene chromosome with 4 parents this allocates ~50 dictionaries per crossover operation.

**Fix:** Replace with a manual frequency count, reusing a dictionary across positions:
```csharp
var freq = new Dictionary<object, int>(parents.Count);

for (int i = 0; i < child.Length; i++)
{
    freq.Clear();
    foreach (var p in parents)
    {
        var v = p.GetGene(i).Value;
        freq[v] = freq.TryGetValue(v, out var cnt) ? cnt + 1 : 1;
    }

    object bestValue = null;
    int bestCount = 0;
    foreach (var kv in freq)
    {
        if (kv.Value >= _threshold && kv.Value > bestCount)
        {
            bestCount = kv.Value;
            bestValue = kv.Key;
        }
    }

    if (bestValue != null)
        child.ReplaceGene(i, new Gene(bestValue));
    else
        mutableGenesIndexes.Add(i);
}
```

---

## Low Impact / Polish

### 9. `AnyHasRepeatedGene` — `Distinct().Count()` with no early exit

**File:** `Chromosomes/ChromosomeExtensions.cs:27`

```csharp
// Current: always walks entire gene array, allocates IEqualityComparer set
var notRepeatedGenesLength = c.GetGenes().Distinct().Count();
```

**Fix:** Manual HashSet with early exit:
```csharp
public static bool AnyHasRepeatedGene(this IList<IChromosome> chromosomes)
{
    for (int i = 0; i < chromosomes.Count; i++)
    {
        var genes = chromosomes[i].GetGenes();
        var seen = new HashSet<Gene>(genes.Length);
        foreach (var g in genes)
        {
            if (!seen.Add(g)) return true;
        }
    }
    return false;
}
```

---

### 10. `ValidateGenes` should be debug-only

**File:** `Populations/Population.cs:167`

`CreateNewGeneration` calls `chromosomes.ValidateGenes()` (a LINQ double-walk of all genes) on
every generation. This is a safety net for chromosome implementation bugs, not a runtime invariant.

**Fix:** Wrap in `#if DEBUG` or remove from the hot path once chromosome implementations are
proven correct.

---

### 11. `RunEvaluateFitness` unnecessary `object` cast

**File:** `GeneticAlgorithm.cs:426`

```csharp
private void RunEvaluateFitness(object chromosome)
{
    var c = chromosome as IChromosome;
```

The parameter is passed as `object` via a lambda capture, but the actual type is always
`IChromosome`. Change the signature to accept `IChromosome` directly (the lambda can still
capture it by reference).

---

## Not Worth Changing

- `Gene` boxing — see discussion at the top. Real but minor relative to simulation cost.
- `Generation.CreationDate = DateTime.Now` — one call per generation, negligible.
- `TplOperatorsStrategy` using `ConcurrentBag` then `.ToList()` — the lock contention on the
  bag is the bottleneck there, not the `ToList`.
