using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class MatingAction : Action
    {
        private Animal matingPartner;

        public MatingAction(Animal actionPerformer, Animal matingPartner)
        {
            performer = actionPerformer;
            this.matingPartner = matingPartner;
            actionDestination = matingPartner.gameObject.Position();
            Debug.Log("hot time " + performer.name + " " + matingPartner.name + " " + matingPartner.currentAction);
        }

        public override void Execute()
        {
            matingPartner.genderHandler.HandleMating(matingPartner.genes);
            Debug.Log($"Mated {matingPartner.gameObject.name}");
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
            return Vector3.Distance(performer.gameObject.Position(),matingPartner.gameObject.Position()) < 0.7f;
        }
    }
}

