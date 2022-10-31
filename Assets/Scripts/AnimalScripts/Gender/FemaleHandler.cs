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
            if (hasMate)
                return new WaitingAction(baseAnimal, 5);

            if (baseAnimal.PreyColliders.Length == 0)
                return new SearchAction(baseAnimal, () => baseAnimal.PreyColliders, baseAnimal.GetSearchDestination());
            else
                return new MoveToAction(baseAnimal, baseAnimal.GetSearchDestination());
        }

        public override void HandleMating(Genes fatherGenes)
        {
            baseAnimal.currentAction.Cancel();
            baseAnimal.currentPriority = Animal.Priority.None;
            hasMate = false;
            isPregnant = true;
            CouroutineHelper.Instance.CallCouroutine(PregnancyCouroutine(gestationPeriod, fatherGenes));
        }

        void GiveBirth(GameObject gameObject, Genes fatherGenes)
        {
            GameObject childObject = Object.Instantiate(gameObject, gameObject.transform.position, Quaternion.identity);
            Animal child = childObject.GetComponent<Animal>();


            child.isGrownUp = false;
            AnimalStats childGenes = baseAnimal.genes.GetInheritedGenes(fatherGenes, baseAnimal.genes);
            child.Init(childObject, 20, 20, childGenes, 0.25f, GetRandomGender(child));
            childObject.transform.SetParent(gameObject.transform.parent);
            Debug.Log("poof");
        }

        IEnumerator PregnancyCouroutine(int gestationPeriod, Genes fatherGenes)
        {
            yield return new WaitForSeconds(gestationPeriod);
            GiveBirth(baseAnimal.gameObject, fatherGenes);
        }

        public override bool IsAvailableForMating()
        {
            return !isPregnant && baseAnimal.isGrownUp;
        }
    }
}
