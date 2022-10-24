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
                        Debug.Log("CIA" + matingAnimal.genderHandler.IsAvailableForMating() + " " + matingAnimal.currentAction);

                        if(matingAnimal.currentAction != null)
                            matingAnimal.currentAction.Cancel();

                        matingAnimal.genderHandler.hasMate = true;
                        matingAnimal.currentDestination = matingAnimal.gameObject.Position();
                        matingAnimal.currentAction = new WaitingAction(matingAnimal, 7); // 7 seconds for male to come and do the thing
                        return new MatingAction(baseAnimal, matingAnimal);
                    }
                }
                return new SearchAction(baseAnimal, () => baseAnimal.PreyColliders, baseAnimal.GetSearchDestination());
            }
            else
            {
                return new SearchAction(baseAnimal, () => baseAnimal.PreyColliders, baseAnimal.GetSearchDestination());
            }
        }

        public override void HandleMating() { }

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
