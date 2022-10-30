using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class Plant : MonoBehaviour, IEatable
    {
        private List<Animal> eaters;      

        [SerializeField]
        public float GrowthProgress { get; set; }
        private float growthTick = 0.001f;

        private Vector3 prefabFullScale;
        public List<Animal> Eaters
        {
            get { return eaters; }
            set { eaters = value; }
        }

        private int nutritionalValue;
        public int NutritionalValue { get { return (int)(nutritionalValue * GrowthProgress); } }

        public void Start()
        {
            Eaters = new List<Animal>();

            prefabFullScale = gameObject.transform.lossyScale;
            gameObject.transform.localScale = Vector3.zero;
        }

        public void Update()
        {
            if(GrowthProgress != 1)
            {
                GrowthProgress = Mathf.Clamp01(GrowthProgress += growthTick);
                gameObject.transform.localScale = prefabFullScale * GrowthProgress;
            }
        

            if(Time.frameCount % 120 == 0)
            {
                Debug.Log("Frame vibe check passed");

                if (GrowthProgress == 1 && Random.Range(1, 100) > 98)
                {
                    DisperseSeeds();
                }
            }
        }

        public void Eat() // Contrary to name, this function is more like GetEaten().
        {
            Destroy(gameObject);
            gameObject.GetComponent<Collider>().enabled = false; // disabling it manually after destroying since destroy is delayed until end of update(), which means another animal can queue action to eat it aswell.
        }

        public void Init(int nutritionalValue, float growthProgress)
        {
            GrowthProgress = growthProgress;
            this.nutritionalValue = nutritionalValue;
            prefabFullScale = gameObject.transform.lossyScale;
        }

        private void DisperseSeeds()
        {
            Vector3 offset = new Vector3(0.5f, 0, 0.5f);

            if (Random.value < 0.5f)
                offset.x *= -1;

            if (Random.value < 0.5f)
                offset.z *= -1;

            GameObject plant = Instantiate(gameObject, gameObject.transform.position + offset, Quaternion.identity);
            plant.transform.SetParent(gameObject.transform.parent);
            //plant.GetComponent<Plant>().Init(NutritionalValue, 0);
            //plant.transform.localScale = Vector3.zero;
        }
    }
}

