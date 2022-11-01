using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class MoveToAction : Action
    {
        public override Vector3 ActionDestination { get { return destination; } }

        private Vector3 destination;

        public MoveToAction(Animal actionPerformer, Vector3 destination)
        {
            performer = actionPerformer;
            this.destination = destination;
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
            return (Vector3.Distance(performer.gameObject.Position(), ActionDestination) < 0.7f);
        }
    }
}
