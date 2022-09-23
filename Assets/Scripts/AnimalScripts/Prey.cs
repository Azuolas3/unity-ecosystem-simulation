using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class Prey : Animal
    {
        protected override void Move(Vector3 destination)
        {
            float step = speed * Time.deltaTime;
            animalObject.transform.position = Vector3.MoveTowards(animalObject.transform.position, destination, step);

            //if(Vector3.Distance(animalObject.transform.position, destination) > 0.1f)
            //{
            //    Move(destination);
            //}
        }

        protected override Priority GetPriority()
        {
            //Collider[] predatorColliders = fov.GetNearbyColliders(animalObject.transform.position, lineOfSightRadius, fov.PredatorLayerMask);
            
            if(PredatorColliders.Length != 0)
            {
                return Priority.RunAway;
            }
            else if(currentPriority != Priority.None)
            {
                return currentPriority;
            }
            else
            {
                if(hunger < 100 && hunger <= thirst)
                    return Priority.FindFood;
                if(thirst < 100 && thirst <= hunger)
                    return Priority.FindWater;
                if(PreyColliders.Length != 0 && reproductionUrge > 70 && hunger > 50 && thirst > 50)
                    return Priority.Reproduce;

                return Priority.None;
            }
        }

        protected override Vector3 GetNextDestination()
        {
            switch(currentPriority)
            {
                case Priority.FindFood:
                    if(PlantColliders.Length != 0)
                    {
                        Collider collider = FindNearestCollider(PlantColliders);
                        return collider.gameObject.transform.position;
                    }
                    else 
                    {
                        return new Vector3(25, 0, 25);
                    }
                default:
                    return gameObject.transform.position;
                    
            }
        }
    }
}

