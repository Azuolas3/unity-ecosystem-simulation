using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using EcosystemSimulation;
using UnityEngine.AI;

namespace EcosystemSimulation
{
    public class TerrainGeneration : MonoBehaviour
    {
        public FloraGenerator floraGenerator;
        public FaunaGenerator faunaGenerator;

        [SerializeField]
        private TerrainType[] terrainTypes;

        private TerrainName[,] terrainMap;

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

        [SerializeField]
        public int mapSeed { get; set; }

        public NavMeshSurface navMeshSurface;

        [SerializeField]
        private MeshCollider mapCollider;
        private GameObject waterColliderObject;
        private List<GameObject> waterColliderObjects = new List<GameObject>();

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
            terrainMap = GenerateTerrainMap(noiseMap);

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
            Destroy(waterColliderObject);
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
            mapCollider.sharedMesh = meshFilter.sharedMesh;
            SetupWaterCollider();
        }

        private void SetupWaterCollider()
        {
            waterColliderObject = new GameObject("Water Collider");
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if(terrainMap[x, y] == TerrainName.DeepWater || terrainMap[x, y] == TerrainName.ShallowWater)
                    {
                        GameObject temp = InstantiateWaterCollider(x, y);
                        temp.transform.SetParent(waterColliderObject.transform);
                        waterColliderObjects.Add(temp);
                    }
                }
            }
        }

        private GameObject InstantiateWaterCollider(int x, int y)
        {
            GameObject obj = new GameObject();
            Vector3 position = new Vector3(x + 0.5f, 0, y + 0.5f);
            Vector3 size = new Vector3(1, 0.5f, 1);

            obj.layer = 4; //water layer mask
            BoxCollider collider = obj.AddComponent<BoxCollider>();
            obj.transform.position = position;
            collider.size = size; // making y smaller to prevent problems with raycasts

            NavMeshObstacle obstacleComponent = obj.AddComponent<NavMeshObstacle>();
            obstacleComponent.carving = true;
            obstacleComponent.size = size - new Vector3(0.9f, 0, 0.9f); // cutting down size of navObstacle to let animal get close to it
            return obj;
        }

        public void DrawMesh(MeshData meshData, Texture2D texture)
        {
            Mesh mesh = meshData.CreateMesh();
            meshFilter.sharedMesh = mesh;
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