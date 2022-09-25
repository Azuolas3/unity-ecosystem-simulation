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
            if((UnityEngine.Object)this != null)
                Destroy(gameObject);
        }

        public void Init(int nutritionalValue)
        {
            Eaters = new List<Animal>();
            this.nutritionalValue = nutritionalValue;
            plant = gameObject;
        }
    }
}

