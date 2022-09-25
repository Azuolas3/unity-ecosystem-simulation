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
            Debug.Log(food + " " + food.NutritionalValue);
            performer.Hunger += food.NutritionalValue;
            if (performer.Hunger > 100)
                performer.Hunger = 100;
            //Debug.Log(performer.animalObject.name + " " + food);
            food.Eat();
            OnComplete();
        }

        public override void OnComplete()
        {
            //performer.currentAction = null;
            foreach(Animal animal in food.Eaters)
            {
                if(animal.currentAction != null)
                    animal.currentAction.Cancel();
            }
        }

        public override void Cancel()
        {
            performer.currentAction = null;
        }

        public override bool AreConditionsMet()
        {
            return (Vector3.Distance(performer.gameObject.transform.position, actionDestination) < 0.2f);

        }
    }
}

