using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EcosystemSimulation
{
    public abstract class Animal : MonoBehaviour
    {
        protected enum Priority
        {
            None, 
            FindWater, FindFood,
            Reproduce,
            RunAway
        }

        [SerializeField]
        protected Priority currentPriority;
        [SerializeField]
        public Vector3 currentDestination;
        public Action currentAction;

        public GameObject animalObject;
        protected FieldOfView fov;

        [SerializeField]
        protected float hunger;
        protected float thirst;
        protected float reproductionUrge;

        public float Hunger 
        {
            get { return hunger; }
            set { hunger = value; }
        }

        protected float ReproductionUrge
        {
            get { return Hunger; }
            set { reproductionUrge = (hunger + thirst) / 2; }
        }

        protected float movementSpeed;
        protected float rotationSpeed = 10f;
        protected float lineOfSightRadius;

        [SerializeField]
        private bool test;

        protected bool NeedsDestination { get { return animalObject.transform.position == currentDestination; } }
        protected bool NeedsAction { get { return currentAction == null; } }

        [SerializeField]
        private NavMeshAgent navAgent;

        protected Collider[] PredatorColliders { get { return fov.GetNearbyColliders(animalObject.transform.position, lineOfSightRadius, fov.PredatorLayerMask); } }
        protected Collider[] PlantColliders { get { return fov.GetNearbyColliders(animalObject.transform.position, lineOfSightRadius, fov.PlantLayerMask); } }
        protected Collider[] PreyColliders { get { return fov.GetNearbyColliders(animalObject.transform.position, lineOfSightRadius, fov.PreyLayerMask); } }

        int frameCount = 0;

        public void Update()
        {
            frameCount++;
            currentPriority = GetPriority();
            //Debug.Log(currentPriority);
            test = NeedsAction;
            if (NeedsAction)
            {
                //Debug.Log("Needs action  " + animalObject.name);
                currentAction = GetNextAction();
                Debug.Log("Needs action  " + animalObject.name + currentAction);
                currentDestination = currentAction.actionDestination;
            }
            navAgent.SetDestination(currentDestination);

            if (currentAction.AreConditionsMet())
            {
                currentAction.Execute();
            }
        }

        //private void LateUpdate()
        //{
        //    if (NeedsAction)
        //    {
        //        Debug.Log("Needs action  " + animalObject.name);
        //        currentAction = GetNextAction();
        //        currentDestination = currentAction.actionDestination;
        //    }
        //}

        public void Init(GameObject animalObject, float baseHunger, float baseThirst, float baseSpeed, float baseSightRadius)
        {
            this.animalObject = animalObject;
            fov = new FieldOfView();

            hunger = baseHunger;
            thirst = baseThirst;
            movementSpeed = baseSpeed;
            lineOfSightRadius = baseSightRadius;
            currentDestination = animalObject.transform.position;
            currentPriority = GetPriority();
            currentAction = GetNextAction();
            //Collider[] colliders = fov.GetNearbyColliders(animalObject.transform.position, 3);
            //foreach(Collider collider in colliders)
            //{
            //    Debug.Log(collider.gameObject);
            //}
        }

        public Collider FindNearestCollider(Collider[] colliders)
        {
            Collider nearestCollider = colliders[0];
            float nearestDistance = float.MaxValue;
            float distance;

            foreach(Collider collider in colliders)
            {
                distance = (animalObject.transform.position - collider.gameObject.transform.position).sqrMagnitude;
                if(distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestCollider = collider;
                }
            }

            return nearestCollider;
        }

        protected abstract void Move(Vector3 destination);
        protected abstract void RotateTowards(Vector3 destination);
        protected abstract Priority GetPriority();
        protected abstract Vector3 GetNextDestination();
        protected abstract Action GetNextAction();
    }
}

