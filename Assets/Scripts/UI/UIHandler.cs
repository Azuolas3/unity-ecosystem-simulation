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
        Slider timeSpeedSlider;

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

        const int INITIAL_SEED = 0;
        const int INITIAL_MAP_WIDTH = 60;
        const int INITIAL_MAP_LENGTH = 60;

        const float INITIAL_PREY_DENSITY = 0.03f;
        const float INITIAL_PREDATOR_DENSITY = 0.003f;

        const float INITIAL_PLANT_DENSITY = 0.15f;
        const float INITIAL_TREE_DENSITY = 0.2f;

        int seed = INITIAL_SEED;
        int mapWidth = INITIAL_MAP_WIDTH;
        int mapLength = INITIAL_MAP_LENGTH;

        float preyDensity = INITIAL_PREY_DENSITY;
        float predatorDensity = INITIAL_PREDATOR_DENSITY;

        float plantDensity = INITIAL_PLANT_DENSITY;
        float treeDensity = INITIAL_TREE_DENSITY;


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

            timeSpeedSlider.onValueChanged.AddListener(SliderListener);

            button.onClick.AddListener(OnClickGenerate);
        }

        void SliderListener(float value)
        {
            if (value == -1)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = Mathf.Pow(2, value);
            }

        }

        void GenerateInitialSimulation()
        {
            seedInputField.text = seed.ToString();
            widthInputField.text = mapWidth.ToString();
            lengthInputField.text = mapLength.ToString();
            preyInputField.text = (preyDensity * 100).ToString();
            predatorInputField.text = (predatorDensity * 100).ToString();
            plantInputField.text = (plantDensity * 100).ToString();
            treeInputField.text = (treeDensity * 100).ToString();

            MapSettings mapSettings = new MapSettings(seed, mapWidth, mapLength, preyDensity, predatorDensity, plantDensity, treeDensity);
            terrainGenerator.GenerateTerrain(mapSettings);
        }
    }
}