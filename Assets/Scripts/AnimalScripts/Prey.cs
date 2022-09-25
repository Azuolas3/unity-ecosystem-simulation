using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class Prey : Animal, IEatable
    {
        private List<Animal> eaters;

        public List<Animal> Eaters 
        { 
            get { return eaters; } 
            set { eaters = value; }
        }

        private int nutritionalValue;
        public int NutritionalValue { get { return nutritionalValue; } }

        protected override void Move(Vector3 destination)
        {
            float step = movementSpeed * Time.deltaTime;
            animalObject.transform.position = Vector3.MoveTowards(animalObject.transform.position, destination, step);


        }

        protected override void RotateTowards(Vector3 destination)
        {
            Vector3 targetDirection = destination - animalObject.transform.position;
            float step = rotationSpeed * Time.deltaTime;

            Vector3 direction = Vector3.RotateTowards(animalObject.transform.forward, targetDirection, step, 0f); 

            animalObject.transform.rotation = Quaternion.LookRotation(direction);
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

        protected override Action GetNextAction()
        {
            switch (currentPriority)
            {
                case Priority.FindFood:
                    if (PlantColliders.Length != 0)
                    {
                        Collider collider = FindNearestCollider(PlantColliders);
                        Vector3 destination =  collider.gameObject.transform.position;
                        return new EatingAction(this, collider.GetComponent<IEatable>(), destination);
                    }
                    else
                    {
                        return new SearchAction(this, () => PlantColliders, new Vector3(25, 0, 25));
                    }
                default:
                    return new SearchAction(this, () => PlantColliders, new Vector3(25, 0, 25)); ;

            }
        }

        public void Eat()
        {
            Destroy(gameObject);
        }

        public void Init(GameObject animalObject, float baseHunger, float baseThirst, float baseSpeed, float baseSightRadius, int baseNutritionalValue)
        {
            eaters = new List<Animal>();
            this.animalObject = animalObject;
            fov = new FieldOfView();

            hunger = baseHunger;
            thirst = baseThirst;
            movementSpeed = baseSpeed;
            lineOfSightRadius = baseSightRadius;
            currentDestination = animalObject.transform.position;
            nutritionalValue = baseNutritionalValue;
            //Collider[] colliders = fov.GetNearbyColliders(animalObject.transform.position, 3);
            //foreach(Collider collider in colliders)
            //{
            //    Debug.Log(collider.gameObject);
            //}
        }
    }
}

