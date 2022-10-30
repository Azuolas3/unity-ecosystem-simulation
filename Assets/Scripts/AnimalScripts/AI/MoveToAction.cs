using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class MoveToAction : Action
    {

        public MoveToAction(Animal actionPerformer, Vector3 destination)
        {
            performer = actionPerformer;
            actionDestination = destination;
        }


        public override void Execute()
        {
            OnComplete();
        }

        public override void OnComplete()
        {
            performer.currentPriority = Animal.Priority.None;
            performer.currentAction = null;
        }

        public override void Cancel()
        {
            performer.currentPriority = Animal.Priority.None;
            performer.currentAction = null;
        }

        public override bool AreConditionsMet()
        {
            return (Vector3.Distance(performer.gameObject.Position(), actionDestination) < 0.7f);
        }
    }
}
