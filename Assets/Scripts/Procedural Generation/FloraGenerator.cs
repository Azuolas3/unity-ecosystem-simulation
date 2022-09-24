using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class FloraGenerator : MonoBehaviour
    {
        [SerializeField]
        GameObject[] treePrefabs;
        [SerializeField]
        GameObject[] plantPrefabs;

        [SerializeField]
        private float treeDensity;
        [SerializeField]
        private float plantDensity;

        TerrainName[,] terrainMap;
        private int seed;
        private int width;
        private int length;

        private GameObject instantiatedFlora;
        private bool[,] occupiedTilesMap;

        public void GenerateTrees()
        {
            instantiatedFlora = new GameObject();
            System.Random pseudoRNG = new System.Random(seed);

            for (int y = 0; y < length - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    if(terrainMap[x, y] == TerrainName.Grass && !occupiedTilesMap[x, y])
                    {
                        float chance = pseudoRNG.Next(0, 100);
                        if(chance <= treeDensity)
                        {
                            int chosenTreeIndex = pseudoRNG.Next(0, treePrefabs.Length);
                            GameObject obj = Instantiate(treePrefabs[chosenTreeIndex], new Vector3(x + 0.5f, 0, y + 0.5f), Quaternion.identity);
                            obj.transform.SetParent(instantiatedFlora.transform);

                            occupiedTilesMap[x, y] = true;
                        }
                    }
                }
            }
        }

        public void GeneratePlants()
        {
            System.Random pseudoRNG = new System.Random(seed);

            for (int y = 0; y < length - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    if ((terrainMap[x, y] == TerrainName.Grass || terrainMap[x, y] == TerrainName.ShallowGrass) && !occupiedTilesMap[x, y])
                    {
                        float chance = pseudoRNG.Next(0, 100);
                        if (chance <= plantDensity)
                        {
                            int chosenPlantIndex = pseudoRNG.Next(0, plantPrefabs.Length);
                            GameObject obj = Instantiate(plantPrefabs[chosenPlantIndex], new Vector3(x + 0.5f, 0, y + 0.5f), Quaternion.identity);
                            obj.transform.SetParent(instantiatedFlora.transform);
                            obj.GetComponent<Plant>().Init(1);
                        }
                    }
                }
            }
        }

        public void ClearGeneratedFlora()
        {
            Destroy(instantiatedFlora);
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

