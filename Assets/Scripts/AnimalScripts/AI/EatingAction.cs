using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class EatingAction : Action
    {
        private IEatable food;

        public EatingAction(Animal actionPerformer, IEatable food, Vector3 destination)
        {
            this.performer = actionPerformer;
            this.food = food;
            actionDestination = destination;
            food.Eaters.Add(performer);
            //Debug.Log("CIA" + food);
        }

        public override void Execute()
        {
            //Debug.Log(food + " " + food.NutritionalValue);
            performer.Nourishment += food.NutritionalValue;
            if (performer.Nourishment > 100)
                performer.Nourishment = 100;
            //Debug.Log(performer.animalObject.name + " " + food);
            food.Eat();
            OnComplete();
        }

        public override void OnComplete()
        {
            //performer.currentAction = null;
            foreach(Animal animal in food.Eaters)
            {
                //if(animal.currentAction != null) //have to check if its null cause there's a chance its the second time the animal's current action is being removed
                animal.currentAction.Cancel();
                Debug.Log(animal.gameObject.name + " Cancelled");
                //food.Eaters.Remove(animal);
            }
        }

        public override void Cancel()
        {
            performer.currentPriority = Animal.Priority.None;
            performer.currentAction = null;
        }

        public override bool AreConditionsMet()
        {
            return (Vector3.Distance(performer.gameObject.transform.position, actionDestination) < 0.3f);
        }
    }
}

