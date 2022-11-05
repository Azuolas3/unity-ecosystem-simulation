using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EcosystemSimulation
{
    public class DrinkingAction : Action
    {
        public override Vector3 ActionDestination { get { return destination; } }
        private Vector3 destination;

        public DrinkingAction(Animal actionPerformer, Vector3 destination)
        {
            performer = actionPerformer;
            //NavMeshHit hit;
            this.destination = destination;
            //NavMesh.SamplePosition(destination, out hit, 1.0f, NavMesh.AllAreas);
            //this.destination = hit.position;
        }

        public override void Execute()
        {
            performer.Hydration = 100;
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
            return Mathf.Abs(performer.gameObject.Position().x - ActionDestination.x) < 0.7f && //Doing this with Abs instead of Vector3.Distance for performance
               Mathf.Abs(performer.gameObject.Position().z - ActionDestination.z) < 0.7f;
        }

        public override string ToString()
        {
            return "drinking";
        }
    }
}
