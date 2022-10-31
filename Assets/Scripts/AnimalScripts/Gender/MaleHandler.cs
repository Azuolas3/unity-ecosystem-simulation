using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EcosystemSimulation
{
    public class MaleHandler : GenderHandler
    {
        public MaleHandler(Animal animal)
        {
            this.baseAnimal = animal;
            Gender = AnimalGender.Male;
        }

        public override Action HandleReproductionPriority()
        {
            if (baseAnimal.PreyColliders.Length != 0)
            {
                foreach (Collider collider in baseAnimal.PreyColliders)
                {
                    Animal matingAnimal = collider.GetComponent<Animal>();
                    if (IsPartnerAppropiate(matingAnimal))
                    {
                        if(matingAnimal.currentAction != null)
                            matingAnimal.currentAction.Cancel();

                        matingAnimal.genderHandler.hasMate = true;
                        matingAnimal.currentDestination = matingAnimal.gameObject.Position();
                        matingAnimal.currentAction = null;
                        return new MatingAction(baseAnimal, matingAnimal);
                    }
                }
                return new MoveToAction(baseAnimal, baseAnimal.GetSearchDestination());
            }
            else
            {
                return new SearchAction(baseAnimal, () => baseAnimal.PreyColliders, baseAnimal.GetSearchDestination());
            }
        }

        public override void HandleMating(Genes partnerGenes) { }

        private bool IsPartnerAppropiate(Animal partner)
        {
            return (partner.currentPriority == Animal.Priority.Reproduce && partner.genderHandler.Gender == AnimalGender.Female && !partner.genderHandler.hasMate);
        }

        public override bool IsAvailableForMating()
        {
            return baseAnimal.isGrownUp;
        }
    }
}
