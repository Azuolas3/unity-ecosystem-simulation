using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class Plant : MonoBehaviour, IEatable
    {
        private List<Animal> eaters;

        private GameObject plant;

        public List<Animal> Eaters
        {
            get { return eaters; }
            set { eaters = value; }
        }

        private int nutritionalValue;
        public int NutritionalValue { get { return nutritionalValue; } }
         
        public void Eat()
        {
            //Eaters = new List<Animal>();
            //Debug.Log(gameObject);
            Destroy(gameObject);
            gameObject.GetComponent<Collider>().enabled = false; // disabling it manually after destroying since destroy is delayed until end of update(), which means another animal can queue action to eat it aswell.
        }

        public void Init(int nutritionalValue)
        {
            Eaters = new List<Animal>();
            this.nutritionalValue = nutritionalValue;
            plant = gameObject;
        }
    }
}

