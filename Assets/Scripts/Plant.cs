using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class Plant : MonoBehaviour, IEatable
    {
        private int nutritionalValue;
        public int NutritionalValue { get { return nutritionalValue; } }
         
        public void Eat()
        {
            Destroy(gameObject);
        }

        public void Init(int nutritionalValue)
        {
            this.nutritionalValue = nutritionalValue;
        }
    }
}

