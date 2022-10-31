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
            //else if(currentPriority != Priority.None)
            //{
            //    return currentPriority;
            //}
            else
            {
                if (ReproductionUrge > 70 && Nourishment > 50 && Hydration > 50 && genderHandler.IsAvailableForMating())
                {
                    Debug.Log("NORO YRA");
                    return Priority.Reproduce;
                }

                if (nourishment < 100 && Nourishment <= Hydration)
                    return Priority.FindFood;
                if(hydration < 100 && Hydration <= Nourishment)
                    return Priority.FindWater;

                return Priority.None;
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
                        Debug.Log($"{collider} {destination}");
                        return new EatingAction(this, collider.GetComponent<IEatable>(), destination);
                    }
                    else
                    {
                        return new SearchAction(this, () => PlantColliders, GetSearchDestination());
                    }

                case Priority.FindWater:
                    if (WaterColliders.Length != 0)
                    {
                        Collider collider = FindNearestCollider(WaterColliders);
                        Vector3 destination = collider.gameObject.transform.position;
                        return new DrinkingAction(this, destination);
                    }
                    else
                    {
                        return new SearchAction(this, () => WaterColliders, GetSearchDestination());
                    }

                case Priority.Reproduce:
                    return genderHandler.HandleReproductionPriority();
                default:
                    return new SearchAction(this, () => PlantColliders, GetSearchDestination()); ;

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

        public void Consume()
        {
            Destroy(gameObject);
            gameObject.GetComponent<Collider>().enabled = false;
        }

        //public void Init(GameObject animalObject, float baseHunger, float baseThirst, float baseSpeed, float baseSightRadius, int baseNutritionalValue, float growthProgress, GenderHandler gender)
        //{
        //    currentPriority = Priority.None;
        //    currentAction = null;

        //    eaters = new List<Animal>();
        //    this.animalObject = animalObject;
        //    fov = new FieldOfView();

        //    nourishment = baseHunger;
        //    hydration = baseThirst;
        //    movementSpeed = baseSpeed;
        //    lineOfSightRadius = baseSightRadius;
        //    currentDestination = animalObject.transform.position;
        //    nutritionalValue = baseNutritionalValue;
        //    size = gameObject.transform.lossyScale;
        //    GrowthProgress = growthProgress;

        //    this.genderHandler = gender;
        //    //Collider[] colliders = fov.GetNearbyColliders(animalObject.transform.position, 3);
        //    //foreach(Collider collider in colliders)
        //    //{
        //    //    Debug.Log(collider.gameObject);
        //    //}
        //}
    }
}

