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

        [SerializeField]
        private bool spawnPredators;

        public void GenerateTerrain()
        {
            Noise noiseGenerator = new Noise();
            float[,] noiseMap = noiseGenerator.GenerateNoiseMap(mapSeed, width, length, noiseScale, octaves, persistence, lacunarity, Vector2.zero);
            terrainMap = GenerateTerrainMap(noiseMap);

            mapTexture = GenerateTexture(width, length, noiseMap);
            DrawMesh(MeshGenerator.GenerateMesh(width, length, noiseMap), mapTexture);

            SetupCollider();

            occupiedTilesMap = new bool[width, length];            

            floraGenerator.Init(mapSeed, width, length, terrainMap, occupiedTilesMap);
            floraGenerator.GenerateTrees();

            bool[,] occupiedTilesMapCopy = (bool[,])occupiedTilesMap.Clone();
            new MapHelper(length - 1, width - 1, terrainMap, occupiedTilesMap); // Passing dimensions subtracted by one since that is the actual size of the map
            floraGenerator.GeneratePlants();
            

            faunaGenerator.Init(mapSeed, width, length, terrainMap, occupiedTilesMap);
            faunaGenerator.GeneratePreyFauna();
           
            if(spawnPredators)
                faunaGenerator.GeneratePredatorFauna();
            navMeshSurface.BuildNavMesh();
            InstantiateInvisibleWalls();
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
                    if((terrainMap[x, y] == TerrainName.DeepWater || terrainMap[x, y] == TerrainName.ShallowWater) && IsNeighbouringLand(x, y))
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
            obstacleComponent.size = size - new Vector3(0.92f, 0, 0.92f); // cutting down size of navObstacle to let animal get close to it
            return obj;
        }

        private void InstantiateInvisibleWalls()
        {
            GameObject wallColliderObject = new GameObject("Wall Collider");
            Vector3 position;

            GameObject firstWall = new GameObject();
            BoxCollider collider = firstWall.AddComponent<BoxCollider>();
            collider.transform.position = new Vector3(width / 2, 30, 0);
            collider.size = new Vector3(width, 60, 1);
            firstWall.transform.SetParent(wallColliderObject.transform);

            position = firstWall.Position() + new Vector3(0, 0, length);
            GameObject secondWall = Instantiate(firstWall, position, Quaternion.identity);
            secondWall.transform.SetParent(wallColliderObject.transform);


            GameObject thirdWall = new GameObject();
            collider = thirdWall.AddComponent<BoxCollider>();
            collider.transform.position = new Vector3(0, 30, length / 2);
            collider.size = new Vector3(1, 60, length);
            thirdWall.transform.SetParent(wallColliderObject.transform);

            position = thirdWall.Position() + new Vector3(width, 0, 0);
            GameObject fourthWall = Instantiate(thirdWall, position, Quaternion.identity);
            fourthWall.transform.SetParent(wallColliderObject.transform);

            GameObject ceiling = new GameObject();
            collider = ceiling.AddComponent<BoxCollider>();
            collider.transform.position = new Vector3(width / 2, 60, length / 2);
            collider.size = new Vector3(width, 1, length);
            ceiling.transform.SetParent(wallColliderObject.transform);
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

        private bool IsNeighbouringLand(int x, int y)
        {
            int[] offsets = { 1, -1 };
            for (int i = 0; i < 2; i++)
            {
                if (x + offsets[i] < width && x + offsets[i] >= 0 && terrainMap[x + offsets[i], y] == TerrainName.Sand)
                    return true;
                if (y + offsets[i] < length && y + offsets[i] >= 0 && terrainMap[x, y + offsets[i]] == TerrainName.Sand)
                    return true;
            }

            return false;
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