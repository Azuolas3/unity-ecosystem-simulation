using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class EatingAction : Action
    {
        public override Vector3 ActionDestination { get { return foodObject.Position(); } }

        private IEatable food;
        private GameObject foodObject;

        public EatingAction(Animal actionPerformer, IEatable food, GameObject foodObject)
        {
            this.performer = actionPerformer;
            this.food = food;
            this.foodObject = foodObject;
            Debug.Log(actionPerformer.name + food.Eaters + food);
            Debug.Log(performer);
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
            //foreach(Animal animal in food.Eaters)
            //{
            //    if(animal.currentAction != null && animal != null) //since the animal could be eaten or have his action cancelled on the same frame, need to check)
            //    { 
            //        Debug.Log($"Animal {animal.gameObject.name} cancelled rip");
            //        animal.currentAction.Cancel();
            //    }
            //    //Debug.Log(animal.gameObject.name + " Cancelled");
            //    //food.Eaters.Remove(animal);
            //}
        }

        public override void Cancel()
        {
            performer.currentPriority = Animal.Priority.None;
            performer.currentAction = null;
        }

        public override bool AreConditionsMet()
        {
            return performer.gameObject.Position().IsClose(ActionDestination, 1f);
        }

        public override string ToString()
        {
            return "eating";
        }
    }
}

