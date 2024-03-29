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
            string searchString;
            switch(performer.currentPriority)
            {
                case Animal.Priority.FindFood:
                    searchString = "for food";
                    break;
                case Animal.Priority.FindWater:
                    searchString = "for water";
                    break;
                case Animal.Priority.Reproduce:
                    searchString = "for mate";
                    break;
                default:
                    searchString = "";
                    break;

            }
            return $"searching {searchString}";
        }
    }
}

