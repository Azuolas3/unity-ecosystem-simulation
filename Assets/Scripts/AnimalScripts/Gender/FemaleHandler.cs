using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EcosystemSimulation
{
    public class FemaleHandler : GenderHandler
    {
        public bool isPregnant = false;

        readonly int gestationPeriod;

        public FemaleHandler(Animal animal, int gestationPeriod)
        {
            this.baseAnimal = animal;
            Gender = AnimalGender.Female;
            this.gestationPeriod = gestationPeriod;
        }

        public override Action HandleReproductionPriority()
        {
            if (baseAnimal.PreyColliders.Length == 0)
                return new SearchAction(baseAnimal, () => baseAnimal.PreyColliders, baseAnimal.GetSearchDestination());
            else
                return new WaitingAction(baseAnimal, 3);
        }

        public override void HandleMating()
        {
            baseAnimal.currentAction.Cancel();
            baseAnimal.currentPriority = Animal.Priority.None;
            hasMate = false;
            isPregnant = true;
            baseAnimal.gameObject.AddComponent<Pregnancy>().CallPregnancyCouroutine(gestationPeriod);
        }

        public override bool IsAvailableForMating()
        {
            return !isPregnant && baseAnimal.isGrownUp;
        }
    }
}
