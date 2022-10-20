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
            actionDestination = matingPartner.gameObject.transform.position;
        }

        public override void Execute()
        {
            GameObject child = Object.Instantiate(performer.gameObject, performer.gameObject.transform.position, Quaternion.identity);
            child.GetComponent<Animal>().Init(child, performer.Hunger, 100, 20, 5);
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
            return Vector3.Distance(performer.gameObject.transform.position, matingPartner.gameObject.transform.position) < 0.3f;
        }
    }
}

