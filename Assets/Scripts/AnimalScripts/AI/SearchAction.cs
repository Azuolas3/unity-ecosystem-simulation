using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class SearchAction : Action
    {
        private Func<Collider[]> getColliders;

        public SearchAction(Animal actionPerformer, Func<Collider[]> getColliders, Vector3 destination)
        {
            this.performer = actionPerformer;
            actionDestination = destination;
            this.getColliders = getColliders;
        }


        public override void Execute()
        {
            OnComplete();
        }

        public override void OnComplete()
        {
            //Collider collider = performer.FindNearestCollider(getColliders());
            //performer.currentAction = new EatingAction(performer, GetComponent<>);
            performer.currentAction = null;
        }

        public override void Cancel()
        {
            performer.currentAction = null;
        }

        public override bool AreConditionsMet()
        {
            return getColliders().Length != 0;
        }
    }
}

