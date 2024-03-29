using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EcosystemSimulation
{
    public class Predator : Animal
    {
        protected override Priority GetPriority()
        {
            if (ReproductionUrge > 70 && Nourishment > 20 && Hydration > 20 && genderHandler.IsAvailableForMating())
            {
                return Priority.Reproduce;
            }

            if (nourishment < 90 && Nourishment <= Hydration)
                return Priority.FindFood;
            if (hydration < 90 && Hydration <= Nourishment)
                return Priority.FindWater;

            return Priority.None;
        }

        protected override Action GetNextAction()
        {
            switch (currentPriority)
            {
                case Priority.FindFood:
                    if (PreyColliders.Length != 0)
                    {
                        Collider collider = FindNearestCollider(PreyColliders);
                        Vector3 destination = collider.gameObject.transform.position;
                        IEatable prey = collider.GetComponent<IEatable>();
                        return new EatingAction(this, prey, collider.gameObject);
                    }
                    else
                    {
                        return new SearchAction(this, () => PreyColliders, GetSearchDestination());
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
                    return genderHandler.HandleReproductionPriority(() => PredatorColliders);
                default:
                    return new MoveToAction(this, GetSearchDestination());

            }
        }

        protected override void OnDeath()
        {
            Destroy(gameObject);
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
