using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using EcosystemSimulation;

namespace EcosystemSimulation
{
    public class TerrainGeneration : MonoBehaviour
    {
        public FloraGenerator floraGenerator;
        public FaunaGenerator faunaGenerator;

        [SerializeField]
        private TerrainType[] terrainTypes;



        //private GameObject[] instantiatedObjects;
        private GameObject instantiatedObjects;
        private bool[,] occupiedTilesMap;

        [SerializeField]
        private float noiseScale;
        [SerializeField]
        private int octaves;
        [SerializeField]
        private float persistence;
        [SerializeField]
        private float lacunarity;

        [SerializeField]
        private int treeDensity;
        [SerializeField]
        private int plantDensity;

        [SerializeField]
        private int width;
        [SerializeField]
        private int length;


        public static int MapLength { get; set; }

        [SerializeField]
        public int mapSeed { get; set; }

        public NavMeshSurface navMeshSurface;

        [SerializeField]
        private BoxCollider mapCollider;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        TextureGenerator textureGenerator = new TextureGenerator();
        Texture2D mapTexture;

        public void GenerateTerrain()
        {
            //instantiatedObjects = new GameObject[width * length];
            //instantiatedObjects = new GameObject();


            Noise noiseGenerator = new Noise();
            float[,] noiseMap = noiseGenerator.GenerateNoiseMap(mapSeed, width, length, noiseScale, octaves, persistence, lacunarity, Vector2.zero);
            TerrainName[,] terrainMap = GenerateTerrainMap(noiseMap);

            mapTexture = GenerateTexture(width, length, noiseMap);
            DrawMesh(MeshGenerator.GenerateMesh(width, length, noiseMap), mapTexture);

            SetupCollider();

            occupiedTilesMap = new bool[width, length];

            floraGenerator.Init(mapSeed, width, length, terrainMap, occupiedTilesMap);
            floraGenerator.GenerateTrees();
            floraGenerator.GeneratePlants();

            faunaGenerator.Init(mapSeed, width, length, terrainMap, occupiedTilesMap);
            faunaGenerator.GeneratePreyFauna();
            navMeshSurface.BuildNavMesh();
            //faunaGenerator.GeneratePredatorFauna();
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

        public void ClearPreviousGeneration()
        {
            Destroy(instantiatedObjects);
            floraGenerator.ClearGeneratedFlora();
            faunaGenerator.ClearGeneratedFauna();
        }

        public Texture2D GenerateTexture(int width, int height, float[,] noiseMap)
        {
            Color[] colourMap = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float currentNoiseValue = noiseMap[x, y];
                    for (int i = 0; i < terrainTypes.Length; i++)
                    {
                        if (currentNoiseValue <= terrainTypes[i].height)
                        {
                            colourMap[y * width + x] = terrainTypes[i].colour;
                            break;
                        }
                    }
                }
            }
            return textureGenerator.TextureFromColourMap(colourMap, width, height);
        }

        private void SetupCollider()
        {
            mapCollider.size = new Vector3(width-1, 0, length-1);
            mapCollider.center = new Vector3((float)(width-1)/2, 0, (float)(length-1)/2);
        }

        public void DrawMesh(MeshData meshData, Texture2D texture)
        {
            meshFilter.sharedMesh = meshData.CreateMesh();
            meshRenderer.sharedMaterial.mainTexture = texture;
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