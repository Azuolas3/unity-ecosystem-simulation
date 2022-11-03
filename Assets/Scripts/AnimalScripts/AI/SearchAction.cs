using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class SearchAction : Action
    {
        public override Vector3 ActionDestination { get { return destination; } }

        private Vector3 destination;
        private Func<Collider[]> getColliders;

        public SearchAction(Animal actionPerformer, Func<Collider[]> getColliders, Vector3 destination)
        {
            this.performer = actionPerformer;
            this.destination = destination;
            this.getColliders = getColliders;
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
            return (getColliders().Length != 0 || (
                Mathf.Abs(performer.gameObject.Position().x - ActionDestination.x) < 0.7f && //Doing this with Abs instead of Vector3.Distance for performance
               Mathf.Abs(performer.gameObject.Position().z - ActionDestination.z) < 0.7f));
        }

        public override string ToString()
        {
            return "searching";
        }
    }
}

