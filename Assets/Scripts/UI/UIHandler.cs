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
        TerrainGenerator terrainGenerator;

        [SerializeField]
        Button button;
        [SerializeField]
        TMP_InputField seedInputField;
        [SerializeField]
        TMP_InputField widthInputField;
        [SerializeField]
        TMP_InputField lengthInputField;
        [SerializeField]
        TMP_InputField preyInputField;
        [SerializeField]
        TMP_InputField predatorInputField;
        [SerializeField]
        TMP_InputField plantInputField;
        [SerializeField]
        TMP_InputField treeInputField;

        int seed;
        int mapWidth;
        int mapLength;

        float preyDensity;
        float predatorDensity;

        float plantDensity;
        float treeDensity;


        void Start()
        {
            SetupInputListeners();
            GenerateInitialSimulation();
        }

        void OnClickGenerate()
        {
            terrainGenerator.ClearPreviousGeneration();

            MapSettings mapSettings = new MapSettings(seed, mapWidth, mapLength, preyDensity, predatorDensity, plantDensity, treeDensity);
            terrainGenerator.GenerateTerrain(mapSettings);
        }

        int ParseIntegerInput(string txt)
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

        float ParsePercentageInput(string txt)
        {
            float result;
            try
            {
                result = (float.Parse(txt, System.Globalization.CultureInfo.InvariantCulture)) / 100; //Dividing by 100 since code expects value from 0 to 1
                return result;
            }
            catch
            {
                Debug.Log("Input is not a float");
                return 0;
            }
        }

        void SetupInputListeners()
        {
            seedInputField.onEndEdit.AddListener((string value) => seed = ParseIntegerInput(value));
            widthInputField.onEndEdit.AddListener((string value) => mapWidth = ParseIntegerInput(value));
            lengthInputField.onEndEdit.AddListener((string value) => mapLength = ParseIntegerInput(value));
            preyInputField.onEndEdit.AddListener((string value) => preyDensity = ParsePercentageInput(value));
            predatorInputField.onEndEdit.AddListener((string value) => predatorDensity = ParsePercentageInput(value));
            plantInputField.onEndEdit.AddListener((string value) => plantDensity = ParsePercentageInput(value));
            treeInputField.onEndEdit.AddListener((string value) => treeDensity = ParsePercentageInput(value));

            button.onClick.AddListener(OnClickGenerate);
        }

        void GenerateInitialSimulation()
        {
            MapSettings mapSettings = new MapSettings(0, 60, 60, 0.03f, 0.003f, 0.15f, 0.2f);
            terrainGenerator.GenerateTerrain(mapSettings);
        }
    }
}