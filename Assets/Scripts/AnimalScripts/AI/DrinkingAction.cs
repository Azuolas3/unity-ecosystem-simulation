using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class DrinkingAction : Action
    {
        public DrinkingAction(Animal actionPerformer, Vector3 destination)
        {
            performer = actionPerformer;
            actionDestination = destination;
        }

        public override void Execute()
        {
            performer.Hydration = 100;

            OnComplete();
        }

        public override void OnComplete()
        {
            Debug.Log("I drank lule");
            performer.currentAction.Cancel();
        }

        public override void Cancel()
        {
            performer.currentPriority = Animal.Priority.None;
            performer.currentAction = null;
        }

        public override bool AreConditionsMet()
        {
            return (Vector3.Distance(performer.gameObject.transform.position, actionDestination) < 0.6f);
        }
    }
}
