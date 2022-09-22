using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EcosystemSimulation;
using TMPro;

namespace EcosystemSimulation
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField]
        SimulationController simulationController;
        [SerializeField]
        TerrainGeneration terrainGenerator;

        [SerializeField]
        Button button;
        [SerializeField]
        TMP_InputField inputField;

        int seed;
        void Start()
        {
            //terrainGenerator = simulationController.terrainGenerator;
            inputField.onEndEdit.AddListener(ChangeListener);
            button.onClick.AddListener(OnClickGenerate);
        }

        void OnClickGenerate()
        {
            terrainGenerator.ClearPreviousGeneration();
            terrainGenerator.mapSeed = seed;
            terrainGenerator.GenerateTerrain();
        }

        void ChangeListener(string value)
        {
            seed = ParseInput(value);
        }

        int ParseInput(string txt)
        {
            int result;
            try
            {
                result = int.Parse(txt);
                return result;
            }
            catch
            {
                Debug.Log("Input is not integer");
                return 0;
            }
        }
    }
}