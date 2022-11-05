using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EcosystemSimulation
{
    public interface IEatable
    {
        void Consume();

        List<Animal> Eaters { get; set; } // a list of all animals that want to eat this IEatable
        int NutritionalValue { get; }
    }
}
