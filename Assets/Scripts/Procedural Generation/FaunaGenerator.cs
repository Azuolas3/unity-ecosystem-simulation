using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class FaunaGenerator : MonoBehaviour
    {
        [SerializeField]
        GameObject[] predatorPrefabs;
        [SerializeField]
        GameObject[] preyPrefabs;

        [SerializeField]
        private float predatorDensity;
        [SerializeField]
        private float preyDensity;

        private int preyCount = 0;
        private int predatorCount = 0;

        TerrainName[,] terrainMap;
        private int seed;
        private int width;
        private int length;

        private GameObject instantiatedFauna;
        private bool[,] occupiedTilesMap;

        public void GeneratePreyFauna()
        {
            instantiatedFauna = new GameObject();
            System.Random pseudoRNG = new System.Random(seed);

            for (int y = 0; y < length - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    if ((terrainMap[x, y] == TerrainName.Grass || terrainMap[x, y] == TerrainName.ShallowGrass) && !occupiedTilesMap[x, y])
                    {
                        float chance = pseudoRNG.Next(0, 1000);
                        if (chance <= preyDensity)
                        {
                            int chosenAnimalIndex = pseudoRNG.Next(0, preyPrefabs.Length);
                            GameObject obj = Instantiate(preyPrefabs[chosenAnimalIndex], new Vector3(x + 0.5f, 0, y + 0.5f), Quaternion.identity);
                            Prey prey = obj.GetComponent<Prey>();
                            obj.transform.SetParent(instantiatedFauna.transform);

                            GenderHandler gender = preyCount % 2 == 0 ? new MaleHandler(prey) : new FemaleHandler(prey, 5);
                            obj.GetComponent<Prey>().Init(obj, 50, 25, 3, 10, 1, gender, Color.grey);
                            obj.name = "Rabbit" + preyCount;

                            occupiedTilesMap[x, y] = true;
                            preyCount++;
                            //if (preyCount == 1)
                            //    return;
                        }
                    }
                }
            }
        }

        public void GeneratePredatorFauna()
        {
            System.Random pseudoRNG = new System.Random(seed);

            for (int y = 0; y < length - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    if ((terrainMap[x, y] == TerrainName.Grass || terrainMap[x, y] == TerrainName.ShallowGrass) && !occupiedTilesMap[x, y])
                    {
                        float chance = pseudoRNG.Next(0, 100);
                        if (chance <= predatorDensity)
                        {
                            int chosenAnimalIndex = pseudoRNG.Next(0, predatorPrefabs.Length);
                            GameObject obj = Instantiate(predatorPrefabs[chosenAnimalIndex], new Vector3(x + 0.5f, 0, y + 0.5f), Quaternion.identity);
                            Animal animal = obj.GetComponent<Animal>();
                            obj.transform.SetParent(instantiatedFauna.transform);

                            GenderHandler gender = predatorCount % 2 == 0 ? new MaleHandler(animal) : new FemaleHandler(animal, 10);
                            obj.GetComponent<Animal>().Init(obj, 50, 50, 1, 3, 1, gender, Color.grey);

                            occupiedTilesMap[x, y] = true;
                            predatorCount++;
                        }
                    }
                }
            }
        }

        public void ClearGeneratedFauna()
        {
            Destroy(instantiatedFauna);
            preyCount = 0;
        }

        public void Init(int seed, int width, int length, TerrainName[,] terrainMap, bool[,] occupiedTilesMap)
        {
            this.seed = seed;
            this.width = width;
            this.length = length;
            this.terrainMap = terrainMap;
            this.occupiedTilesMap = occupiedTilesMap;
        }
    }

}

