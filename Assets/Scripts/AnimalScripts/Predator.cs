using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EcosystemSimulation
{
    public class Predator : Animal
    {
        protected override void Move(Vector3 destination)
        {
            float step = speed * Time.deltaTime;
            animalObject.transform.position = Vector3.MoveTowards(animalObject.transform.position, destination, step);
        }

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
    }
}
