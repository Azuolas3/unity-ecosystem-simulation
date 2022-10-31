using System.Reflection;
using UnityEngine;


namespace EcosystemSimulation
{
    public class Genes
    {
        private const float mutationChance = 0.5f;
        private readonly AnimalStats stats;

        public Genes(AnimalStats stats)
        {
            this.stats = stats;
        }

        public AnimalStats GetInheritedGenes(Genes father, Genes mother)
        {
            float speed = Random.value >= 0.5f ? father.stats.MovementSpeed : mother.stats.MovementSpeed;
            if (Random.value >= mutationChance)
                speed = MutateGene(speed);

            float size = Random.value >= 0.5f ? father.stats.Size : mother.stats.Size;
            if (Random.value >= mutationChance)
                size = MutateGene(size);

            float lineOfSightRadius = Random.value >= 0.5f ? father.stats.LineOfSightRadius : mother.stats.LineOfSightRadius;
            if (Random.value >= mutationChance)
                lineOfSightRadius = MutateGene(lineOfSightRadius);


            Color colour = Random.value >= 0.5f ? father.stats.Colour : mother.stats.Colour;
            if (Random.value >= mutationChance)
                lineOfSightRadius = MutateGene(lineOfSightRadius);

            AnimalStats test = new AnimalStats(size, speed, lineOfSightRadius, colour);
            return test;
        }

        public float MutateGene(float geneValue)
        {
            geneValue += Random.Range(-0.3f, 0.3f);
            return geneValue;
        }

        public Color MutateColor(Color geneValue)
        {
            geneValue.r += Random.Range(-0.3f, 0.3f);
            geneValue.g += Random.Range(-0.3f, 0.3f);
            geneValue.b += Random.Range(-0.3f, 0.3f);

            return geneValue;
        }
    }
}
