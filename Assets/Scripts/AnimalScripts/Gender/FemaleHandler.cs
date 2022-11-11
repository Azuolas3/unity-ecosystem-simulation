using System;
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

        public override Action HandleReproductionPriority(Func<Collider[]> getColliders)
        {
            if (hasMate)
                return new WaitingAction(baseAnimal, 5);

            if (getColliders().Length == 0)
                return new SearchAction(baseAnimal, getColliders, baseAnimal.GetSearchDestination());
            else
                return new MoveToAction(baseAnimal, baseAnimal.GetSearchDestination());
        }

        public override void HandleMating(Genes fatherGenes)
        {
            //Debug.Log($"{baseAnimal.gameObject.name} {baseAnimal.currentAction}");
            if(baseAnimal.currentAction != null)
                baseAnimal.currentAction.Cancel();
            baseAnimal.currentPriority = Animal.Priority.None;
            hasMate = false;
            isPregnant = true;
            baseAnimal.StartCoroutine(PregnancyCouroutine(gestationPeriod, fatherGenes));
        }

        void GiveBirth(GameObject gameObject, Genes fatherGenes)
        {
            for(int i = 0; i < 2; i++)
            {
                GameObject childObject = UnityEngine.Object.Instantiate(gameObject, gameObject.transform.position, Quaternion.identity);
                Animal child = childObject.GetComponent<Animal>();


                AnimalStats childGenes = baseAnimal.genes.GetInheritedGenes(fatherGenes, baseAnimal.genes);
                child.Init(childObject, 20, 20, childGenes, 0.25f, GetRandomGender(child));
                childObject.transform.SetParent(gameObject.transform.parent);
                Debug.Log("poof");
            }
        }

        IEnumerator PregnancyCouroutine(int gestationPeriod, Genes fatherGenes)
        {
            yield return new WaitForSeconds(gestationPeriod);
            if(baseAnimal != null) //Checking if the female isn't null to make sure its alive (hasn't been eaten after couroutine start and etc.)
                GiveBirth(baseAnimal.gameObject, fatherGenes);
        }

        public override bool IsAvailableForMating()
        {
            return !isPregnant && baseAnimal.IsGrownUp;
        }
    }
}
