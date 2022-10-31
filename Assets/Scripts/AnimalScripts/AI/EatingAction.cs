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
        }

        public override void Execute()
        {
            float result = performer.Nourishment + food.NutritionalValue;
            performer.Nourishment = Mathf.Min(100, result);            
            food.Consume();
            OnComplete();
        }

        public override void OnComplete()
        {
            foreach(Animal animal in food.Eaters)
            {
                if(animal.currentAction != null) //since there can be animals on this list that had their action cancelled/changed due to other factors, we have to                               
                    animal.currentAction.Cancel(); // check for null
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

