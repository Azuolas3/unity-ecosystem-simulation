using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{

    // Class which handles anything that relates with an animals gender - it's gender, how it reproduces, etc.
    public abstract class GenderHandler
    {
        protected Animal baseAnimal;

        public AnimalGender Gender { get; set; }

        public bool hasMate = false;

        public static GenderHandler GetRandomGender(Animal animal)
        {
            GenderHandler gender = UnityEngine.Random.value >= 0.5f ? new MaleHandler(animal) : new FemaleHandler(animal, 5);
            return gender;
        }

        protected bool IsOppositeGender(AnimalGender gender)
        {
            if (Gender == gender) 
                return true;
            else 
                return false;
        }

        public abstract Action HandleReproductionPriority(Func<Collider[]> getColliders);
        public abstract void HandleMating(Genes partnerGenes);
        public abstract bool IsAvailableForMating();
    }

    public enum AnimalGender
    {
        Male, Female
    }
}
