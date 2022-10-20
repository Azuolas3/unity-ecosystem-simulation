using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EcosystemSimulation
{
    public class Predator : Animal
    {
        protected override Priority GetPriority()
        {
            return 0;
        }

        protected override Vector3 GetNextDestination()
        {
            //switch (currentPriority)
            //{
            //    case Priority.FindFood:
            //        Collider collider = FindNearestCollider(PlantColliders);
            //        return collider.gameObject.transform.position;
            //    default:
            //        return gameObject.transform.position;

            //}

            return Vector3.zero;
        }

        protected override Action GetNextAction()
        {
            return new EatingAction(this, new Plant(), Vector3.zero);
        }

        protected override Animal GetMatingPartner()
        {
            return this;
        }
    }
}
