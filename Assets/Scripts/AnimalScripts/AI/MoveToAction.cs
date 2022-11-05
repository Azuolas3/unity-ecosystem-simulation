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

        public override string ToString()
        {
            string moveString;
            switch (performer.currentPriority)
            {
                case Animal.Priority.RunAway:
                    moveString = "running away";
                    break;
                default:
                    moveString = "idling";
                    break;

            }
            return moveString;
        }
    }
}
