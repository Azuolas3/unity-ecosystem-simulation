using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class EatingAction : Action
    {
        public EatingAction(Animal actionPerformer, IEatable food, Vector3 destination)
        {
            this.performer = actionPerformer;
            this.food = food;
            actionDestination = destination;
        }

        private IEatable food;

        public override void Execute()
        {
            performer.Hunger += food.NutritionalValue;
            food.Eat();
            OnComplete();
        }

        public override void OnComplete()
        {
            performer.currentAction = null;
        }

        public override bool IsInRange()
        {
            return (Vector3.Distance(performer.gameObject.transform.position, actionDestination) < 0.1f);

        }
    }
}

