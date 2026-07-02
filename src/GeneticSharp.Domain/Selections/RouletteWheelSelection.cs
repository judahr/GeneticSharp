using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GeneticSharp
{
    /// <summary>
    /// Roulette Wheel Selection
    /// <remarks>
    /// Is a kind of Fitness Proportionate Selection. 
    /// <see href=" http://watchmaker.uncommons.org/manual/ch03s02.html">Fitness-Proportionate Selection</see>
    /// <para>
    /// In the Roulette wheel selection method [Holland, 1992], the first step is to calculate the cumulative fitness of the 
    /// whole population through the sum of the fitness of all individuals. After that, the probability of selection is 
    /// calculated for each individual.
    /// </para>
    /// <para>
    /// Then, an array is built containing cumulative probabilities of the individuals. So, n random numbers are generated in the range 0 to fitness sum.
    /// and for each random number an array element which can have higher value is searched for. Therefore, individuals are selected according to their 
    /// probabilities of selection. 
    /// </para>
    /// <see href="http://en.wikipedia.org/wiki/Fitness_proportionate_selection">Wikipedia</see>
    /// </remarks>
    /// </summary>
    [DisplayName("Roulette Wheel")]
    public class RouletteWheelSelection : SelectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneticSharp.RouletteWheelSelection"/> class.
        /// </summary>
        public RouletteWheelSelection() : base(2)
        {
        }

        /// <summary>
        /// Selects from wheel.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="chromosomes">The chromosomes.</param>
        /// <param name="rouletteWheel">The roulette wheel.</param>
        /// <param name="getPointer">The get pointer.</param>
        /// <returns>The selected chromosomes.</returns>
        protected static IList<IChromosome> SelectFromWheel(int number, IList<IChromosome> chromosomes, IList<double> rouletteWheel, Func<double> getPointer)
        {
            var selected = new List<IChromosome>(number);

            // rouletteWheel's cumulative values are built in non-decreasing order, so a binary
            // search finds the same entry a linear scan would, without the per-draw allocations.
            var wheel = rouletteWheel as List<double> ?? new List<double>(rouletteWheel);

            for (int i = 0; i < number; i++)
            {
                var pointer = getPointer();
                var index = wheel.BinarySearch(pointer);

                if (index < 0)
                {
                    // Bitwise complement of the index of the first entry greater than pointer
                    // (or wheel.Count if every entry is less than pointer - including when the
                    // wheel is all NaN, e.g. every chromosome has zero fitness).
                    index = ~index;
                }

                // BinarySearch doesn't guarantee which duplicate cumulative value it lands on
                // (e.g. a zero-fitness chromosome shares its predecessor's cumulative value), so
                // walk back to the first entry satisfying "value >= pointer" to match the
                // original linear-scan semantics exactly.
                while (index > 0 && wheel[index - 1] >= pointer)
                {
                    index--;
                }

                if (index < wheel.Count)
                {
                    selected.Add(chromosomes[index].Clone());
                }
            }

            return selected;
        }

        /// <summary>
        /// Calculates the cumulative percent.
        /// </summary>
        /// <param name="chromosomes">The chromosomes.</param>
        /// <param name="rouletteWheel">The roulette wheel.</param>
        protected static void CalculateCumulativePercentFitness(IList<IChromosome> chromosomes, IList<double> rouletteWheel)
        {
            var sumFitness = chromosomes.Sum(c => c.Fitness.Value);

            var cumulativePercent = 0.0;

            for (int i = 0; i < chromosomes.Count; i++)
            {
                cumulativePercent += chromosomes[i].Fitness.Value / sumFitness;
                rouletteWheel.Add(cumulativePercent);
            }
        }

        /// <summary>
        /// Performs the selection of chromosomes from the generation specified.
        /// </summary>
        /// <param name="number">The number of chromosomes to select.</param>
        /// <param name="generation">The generation where the selection will be made.</param>
        /// <returns>The select chromosomes.</returns>
        protected override IList<IChromosome> PerformSelectChromosomes(int number, Generation generation)
        {
            var chromosomes = generation.Chromosomes;
            var rouletteWheel = new List<double>();
            var rnd = RandomizationProvider.Current;

            CalculateCumulativePercentFitness(chromosomes, rouletteWheel);

            return SelectFromWheel(number, chromosomes, rouletteWheel, () => rnd.GetDouble());
        }        
    }
}