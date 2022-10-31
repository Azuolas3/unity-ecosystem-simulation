using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    //Singleton used to start various couroutines for classes which do not derive from MonoBehaviour
    public class MapHelper
    {
        private static MapHelper instance;
        public static MapHelper Instance { get { return instance; } }

        int mapLength;
        int mapWidth;
        TerrainName[,] terrainMap;

        public MapHelper(int length, int width, TerrainName[,] terrain)
        {
            instance = this;
            mapLength = length;
            mapWidth = width;
            terrainMap = terrain;
        }

        public bool IsOutOfBounds(Vector3 point)
        {
            float x = point.x;
            float z = point.z;

            if ((0 < x && x < mapWidth) && (0 < z && z < mapLength))
                return false;
            else
                return true;
        }

        public bool IsInWater(Vector3 point)
        {
            float x = point.x;
            float z = point.z;

            if (terrainMap[(int)x, (int)z] == TerrainName.DeepWater || terrainMap[(int)x, (int)z] == TerrainName.ShallowWater)
                return true;
            else
                return false;
        }

        //private int
    }
}

