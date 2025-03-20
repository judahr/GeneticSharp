
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSharp
{
    /// <summary>
    /// Allows a gene value to self mutate.  The gene value the implements IGeneValueMutate defines what that mutation is.
    /// </summary>
    public class GeneMutation : MutationBase
    {
        Action<Gene> action;
        public GeneMutation(Action<Gene> a) { action = a; }

        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            ValidateLength(chromosome);

            if (RandomizationProvider.Current.GetDouble() <= probability)
            {

                
                int i = RandomizationProvider.Current.GetInt(0, chromosome.Length);

                Gene gene = chromosome.GetGene(i);

                action(gene);

                //var geneM = gene.Value as IGeneValueMutate;

                //if (geneM !=null)
                //{
                //    geneM.Mutate();
                //}


            }
        }
        /// <summary>
        /// Validate length of the chromosome.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        protected virtual void ValidateLength(IChromosome chromosome)
        {
            if (chromosome.Length < 0)
            {
                throw new MutationException(this, "A chromosome should have, at least, 1 gene. {0} has only {1} gene.".With(chromosome.GetType().Name, chromosome.Length));
            }

            //bool implementsIGeneValueMutate = false;

            //foreach(Gene gene in chromosome.GetGenes())
            //{
            //    if (gene is IGeneValueMutate)
            //    {
            //        implementsIGeneValueMutate = true;
            //        break;
            //    }
            //}

            //if (!implementsIGeneValueMutate)
            //{
            //    throw new MutationException("At least one gene in a chromosome must implement IGeneMutate.");
            //}
        }


    }
}
