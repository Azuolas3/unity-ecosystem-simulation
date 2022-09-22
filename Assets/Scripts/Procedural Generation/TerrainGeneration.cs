using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EcosystemSimulation;

namespace EcosystemSimulation
{
    public class TerrainGeneration : MonoBehaviour
    {
        [SerializeField]
        private TerrainType[] terrainTypes;

        [SerializeField]
        private float noiseScale;
        [SerializeField]
        private int octaves;
        [SerializeField]
        private float persistence;
        [SerializeField]
        private float lacunarity;

        [SerializeField]
        private int width;
        [SerializeField]
        private int length;

        [SerializeField]
        private int mapSeed;

        [SerializeField]
        private GameObject blockPrefab;

        public void GenerateTerrain()
        {
            Noise noiseGenerator = new Noise();
            float[,] noiseMap = noiseGenerator.GenerateNoiseMap(mapSeed, width, length, noiseScale, octaves, persistence, lacunarity, Vector2.zero);
            TerrainName[,] terrainMap = GenerateTerrainMap(noiseMap);

            for (int y = 0; y < length; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    GameObject block = Instantiate(blockPrefab, new Vector3(x, 0, y), Quaternion.identity);
                    Color currentColor = terrainTypes[(int)terrainMap[x, y]].colour;
                    ChangeColor(block, currentColor);
                }
            }
        }

        public TerrainName[,] GenerateTerrainMap(float[,] noiseMap)
        {
            TerrainName[,] terrainMap = new TerrainName[width, length];
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float currentNoiseValue = noiseMap[x, y];
                    for(int i = 0; i < terrainTypes.Length; i++)
                    {
                        if(currentNoiseValue <= terrainTypes[i].height)
                        {
                            terrainMap[x, y] = terrainTypes[i].terrainName;
                            break;
                        }
                    }
                }
            }
            return terrainMap;
        }

        public void ChangeColor(GameObject gameObject, Color color)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }
    }

    [System.Serializable]
    public struct TerrainType
    {
        public TerrainName terrainName;
        public float height;
        public Color colour;
    }

    public enum TerrainName
    {
        DeepWater,
        ShallowWater,
        Sand,
        ShallowGrass,
        Grass
    }

}