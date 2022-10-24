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
        }

        public override void Execute()
        {
            matingPartner.genderHandler.HandleMating();
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
            performer.currentAction = null;
        }

        public override bool AreConditionsMet()
        {            
            return Vector3.Distance(performer.gameObject.transform.position, matingPartner.gameObject.Position()) < 0.7f;
        }
    }
}

