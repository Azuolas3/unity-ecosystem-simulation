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
        private const float growthTick = 0.001f;

        private float size;
        public List<Animal> Eaters
        {
            get { return eaters; }
            set { eaters = value; }
        }

        private int nutritionalValue;
        public int NutritionalValue { get { return (int)(nutritionalValue * GrowthProgress); } }

        public void Start()
        {
            size = gameObject.transform.lossyScale.x;
            gameObject.transform.localScale = new Vector3(size, size, size) * GrowthProgress;
        }

        public void Update()
        {
            if(GrowthProgress != 1)
            {
                GrowthProgress = Mathf.Clamp01(GrowthProgress += growthTick);
                gameObject.transform.localScale = new Vector3(size, size, size) * GrowthProgress;
            }
        

            if(Time.frameCount % 120 == 0)
            {
                if (GrowthProgress == 1 && Random.value > 0.98f)
                {
                    DisperseSeeds();
                }
            }
        }

        public void Consume() // Contrary to name, this function is more like GetConsumed().
        {
            Destroy(gameObject);
            gameObject.GetComponent<Collider>().enabled = false; // disabling it manually after destroying since destroy is delayed until end of update(), which means another animal can queue action to eat it aswell.
            foreach (Animal animal in Eaters)
            {
                if (animal.currentAction != null && animal != null) //since the animal could be eaten or have his action cancelled on the same frame, need to check)
                {
                    //Debug.Log($"Animal {animal.gameObject.name} cancelled rip");
                    animal.currentAction.Cancel();
                }
            }

            MapHelper.Instance.SetTileOccupancy((int)gameObject.Position().x, (int)gameObject.Position().y, false);
        }

        public void Init(int nutritionalValue, float growthProgress)
        {
            Eaters = new List<Animal>();
            GrowthProgress = growthProgress;
            this.nutritionalValue = nutritionalValue;
            size = gameObject.transform.lossyScale.x;
        }

        private void DisperseSeeds()
        {
            Vector3 offset = new Vector3(1f, 0, 1f);

            if (Random.value < 0.7f)
                offset.x *= -1;

            if (Random.value < 0.7f)
                offset.z *= -1;

            Vector3 plantPosition = gameObject.transform.position + offset;
            if (MapHelper.Instance.IsInaccessible(plantPosition) || MapHelper.Instance.IsOccupiedByFlora(plantPosition))
                return;

            GameObject plant = Instantiate(gameObject, plantPosition, Quaternion.identity);
            plant.GetComponent<Plant>().Init(NutritionalValue, 0);
            plant.transform.SetParent(gameObject.transform.parent);
            MapHelper.Instance.SetTileOccupancy((int)plantPosition.x, (int)plantPosition.y, true);
        }
    }
}

