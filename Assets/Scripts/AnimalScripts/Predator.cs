using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EcosystemSimulation
{
    public class Predator : Animal
    {
        protected override void Move(Vector3 destination)
        {
            float step = movementSpeed * Time.deltaTime;
            animalObject.transform.position = Vector3.MoveTowards(animalObject.transform.position, destination, step);
        }

        protected override void RotateTowards(Vector3 destination)
        {
            Vector3 targetDirection = destination - animalObject.transform.position;
            float step = rotationSpeed * Time.deltaTime;

            Vector3 direction = Vector3.RotateTowards(animalObject.transform.right, targetDirection, step, 0f);

            animalObject.transform.rotation = Quaternion.LookRotation(direction);
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

        protected override Action GetNextAction()
        {
            return new EatingAction(this, new Plant(), Vector3.zero);
        }
    }
}
