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
                if(hunger < 100 && Hunger <= thirst)
                    return Priority.FindFood;
                if(thirst < 100 && thirst <= Hunger)
                    return Priority.FindWater;
                if(ReproductionUrge > 70 && Hunger > 50 && thirst > 50)
                {
                    Debug.Log("NORO YRA");
                    return Priority.Reproduce;
                }

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
                case Priority.Reproduce:
                    if (PreyColliders.Length != 0)
                    {
                        Collider nearestCollider = FindNearestCollider(PreyColliders);
                        foreach(Collider collider in PreyColliders)
                        {
                            Animal animal = collider.GetComponent<Animal>();
                            if(animal.ReproductionUrge >= 50)
                                return new MatingAction(this, animal);
                        }
                        return new SearchAction(this, () => PreyColliders,  new Vector3(25, 0, 25));
                    }
                    else
                    {
                        return new SearchAction(this, () => PreyColliders, new Vector3(25, 0, 25));
                    }

                default:
                    return new SearchAction(this, () => PlantColliders, new Vector3(25, 0, 25)); ;

            }
        }

        protected override Animal GetMatingPartner()
        {
            foreach (Collider collider in PreyColliders)
            {
                Animal animal = collider.GetComponent<Animal>();
                if (animal.ReproductionUrge >= 50)
                    return animal;
            }
            return null;
        }

        public void Eat()
        {
            Destroy(gameObject);
            gameObject.GetComponent<Collider>().enabled = false;
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

