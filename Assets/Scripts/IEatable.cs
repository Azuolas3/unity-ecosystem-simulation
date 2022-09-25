using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EcosystemSimulation
{
    public interface IEatable
    {
        void Eat();

        int NutritionalValue { get; }
        List<Animal> Eaters { get; set; }
    }
}
