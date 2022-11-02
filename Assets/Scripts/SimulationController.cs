using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class SimulationController : MonoBehaviour
    {
        public TerrainGeneration terrainGenerator;
        void Start()
        {
            terrainGenerator = GetComponent<TerrainGeneration>();

            terrainGenerator.GenerateTerrain();
        }
    }
}

