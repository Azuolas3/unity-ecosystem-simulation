using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public abstract class GenderHandler
    {
        protected Animal baseAnimal;

        public AnimalGender Gender { get; set; }

        public bool hasMate = false;

        public GenderHandler GetRandomGender(Animal animal)
        {
            GenderHandler gender = Random.Range(0, 2) == 0 ? new MaleHandler(animal) : new FemaleHandler(animal, 10);
            return gender;
        }

        protected bool IsOppositeGender(AnimalGender gender)
        {
            if (Gender == gender) return true;
            else return false;
        }

        public abstract Action HandleReproductionPriority();
        public abstract void HandleMating();
        public abstract bool IsAvailableForMating();
    }

    public enum AnimalGender
    {
        Male, Female
    }
}
