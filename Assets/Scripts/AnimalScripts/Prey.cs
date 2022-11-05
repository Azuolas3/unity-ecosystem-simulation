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

        public int NutritionalValue { get { return (int)(animalStats.Size * 30); } }

        private bool isRunningAway;

        void Start()
        {
             eaters = new List<Animal>();
        }

        protected override Priority GetPriority()
        {          
            if(PredatorColliders.Length != 0)
            {
                if (!isRunningAway && currentAction != null)
                {
                    currentAction.Cancel();
                    isRunningAway = true;
                }
                return Priority.RunAway;
            }
            else
            {
                if (isRunningAway)
                    isRunningAway = false;

                if (ReproductionUrge > 70 && Nourishment > 50 && Hydration > 50 && genderHandler.IsAvailableForMating())
                {
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
                        return new EatingAction(this, collider.GetComponent<IEatable>(), collider.gameObject);
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
                    return genderHandler.HandleReproductionPriority(() => PreyColliders);
                case Priority.RunAway:
                    Collider predator = FindNearestCollider(PredatorColliders);
                    Vector3 runningAwayDestination = GetRunningAwayDestination(predator);
                    if(!Equals(runningAwayDestination, gameObject.Position()))
                    {
                        return new MoveToAction(this, runningAwayDestination);
                    }
                    else
                    {
                        return new WaitingAction(this, 2);
                    }
                default:
                    return new MoveToAction(this, GetSearchDestination());

            }
        }

        public void Consume()
        {
            OnDeath();
        }

        protected override void OnDeath()
        {
            Destroy(gameObject);
            gameObject.GetComponent<Collider>().enabled = false;
            foreach (Animal animal in Eaters)
            {
                if (animal.currentAction != null && animal != null) //since the animal could be eaten or have his action cancelled on the same frame, need to check)
                {
                    animal.currentAction.Cancel();
                }
            }
        }
    }
}

