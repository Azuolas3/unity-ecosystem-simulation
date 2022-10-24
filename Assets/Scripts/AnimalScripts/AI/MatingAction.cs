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
            Debug.Log(matingPartner.gameObject.name + matingPartner.currentAction);
            matingPartner.genderHandler.HandleMating();
            GameObject childObject = Object.Instantiate(performer.gameObject, performer.gameObject.transform.position, Quaternion.identity);
            Animal child = childObject.GetComponent<Animal>();
            Debug.Log("dud born" + performer.gameObject.name + " " + matingPartner.gameObject.name);
            Debug.Log(matingPartner.gameObject.name + " " + matingPartner.genderHandler.IsAvailableForMating());


            child.isGrownUp = false;
            child.Init(childObject, performer.Nourishment, performer.Hydration, 20, 5, performer.genderHandler.GetRandomGender(child));
            Debug.Log(childObject.name + " " +child.genderHandler.IsAvailableForMating());
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

