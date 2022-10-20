using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EcosystemSimulation
{
    public abstract class Animal : MonoBehaviour
    {
        public enum Priority
        {
            None, 
            FindWater, FindFood,
            Reproduce,
            RunAway
        }

        public enum Gender
        {
            Male, Female
        }

        public Priority currentPriority;
        public Vector3 currentDestination;
        public Action currentAction;

        public GameObject animalObject;
        protected FieldOfView fov;

        [SerializeField]
        protected float hunger;
        protected float thirst;
        [SerializeField]
        protected float reproductionUrge;

        public float Hunger 
        {
            get { return hunger; }
            set { hunger = value; }
        }

        public float ReproductionUrge
        {
            get { return (hunger + thirst) / 2; }
            set { reproductionUrge = value; }
        }

        protected float movementSpeed;
        protected float lineOfSightRadius;

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
            reproductionUrge = ReproductionUrge;
            currentPriority = GetPriority();
            //Debug.Log(currentPriority);
            if (NeedsAction)
            {
                //Debug.Log("Needs action  " + animalObject.name);
                currentAction = GetNextAction();
                Debug.Log("Needs action  " + animalObject.name + currentAction);
                currentDestination = currentAction.actionDestination;
            }
            Debug.Log("Current Action  " + currentAction);
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

        protected Vector3 GetSearchDestination()
        {
            Vector3 position = gameObject.Position();
            float randomAngle;

            float x;
            float z;

            int loopCap = 0;
            do
            {
                randomAngle = Random.Range(0, 360);
                x = (Mathf.Cos(randomAngle) * lineOfSightRadius * 5) + position.x;
                z = (Mathf.Sin(randomAngle) * lineOfSightRadius * 5) + position.z;
                loopCap++;
            } while (IsOutOfBounds(new Vector3(x, 0, z)) && loopCap < 16);
            return new Vector3(x, 0, z);
        }

        private bool IsOutOfBounds(Vector3 position)
        {
            Debug.Log(position + new Vector3(0, 1, 0));
            return !Physics.Raycast(position + new Vector3(0, 1, 0), Vector3.down, 5);  // Launches raycast from vector offset directly downward to find if is out of bounds
        }

        protected abstract Priority GetPriority();
        protected abstract Vector3 GetNextDestination();
        protected abstract Action GetNextAction();
        protected abstract Animal GetMatingPartner();
    }
}

