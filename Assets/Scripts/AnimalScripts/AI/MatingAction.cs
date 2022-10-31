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
            return Mathf.Abs(performer.gameObject.Position().x - matingPartner.gameObject.Position().x) < 0.7f && //Doing this with Abs instead of Vector3.Distance for performance
               Mathf.Abs(performer.gameObject.Position().z - matingPartner.gameObject.Position().z) < 0.7f;
        }
    }
}

