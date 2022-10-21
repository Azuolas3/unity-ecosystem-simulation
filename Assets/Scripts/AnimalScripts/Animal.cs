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
        protected float health;
        [SerializeField]
        protected float nourishment;
        [SerializeField]
        protected float hydration;
        [SerializeField]
        protected float reproductionUrge;

        private float hungerTick = 0.02f;
        private float thirstTick = 0.02f;


        public float Health
        {
            get { return health; }
            set { health = value; }
        }
        public float Nourishment 
        {
            get { return nourishment; }
            set { nourishment = value; }
        }

        public float Hydration
        {
            get { return hydration; }
            set { hydration = value; }
        }

        public float ReproductionUrge
        {
            get { return (nourishment + hydration) / 2; }
            set { reproductionUrge = value; }
        }

        protected float movementSpeed;
        protected float lineOfSightRadius;

        protected bool NeedsDestination { get { return animalObject.transform.position == currentDestination; } }
        protected bool NeedsAction { get { return currentAction == null; } }

        [SerializeField]
        private NavMeshAgent navAgent;

        protected Collider[] PredatorColliders { get { return fov.GetNearbyColliders(animalObject.transform.position, lineOfSightRadius, fov.PredatorLayerMask); } }
        protected Collider[] WaterColliders { get { return fov.GetNearbyColliders(animalObject.transform.position, lineOfSightRadius, fov.WaterLayerMask); } }
        protected Collider[] PlantColliders { get { return fov.GetNearbyColliders(animalObject.transform.position, lineOfSightRadius, fov.PlantLayerMask); } }
        protected Collider[] PreyColliders { get { return fov.GetNearbyColliders(animalObject.transform.position, lineOfSightRadius, fov.PreyLayerMask); } }

        public void Start()
        {
            Health = 100;
        }

        public void Update()
        {
            currentPriority = GetPriority();
            //Debug.Log(currentPriority);
            if (NeedsAction)
            {
                //Debug.Log("Needs action  " + animalObject.name);
                currentAction = GetNextAction();
                //Debug.Log("Needs action  " + animalObject.name + currentAction);
                currentDestination = currentAction.actionDestination;
            }
            //Debug.Log("Current Action  " + currentAction);
            navAgent.SetDestination(currentDestination);

            if (currentAction.AreConditionsMet())
            {
                currentAction.Execute();
            }

            Nourishment = Mathf.Clamp(Nourishment - hungerTick, 0, 100);
            Hydration = Mathf.Clamp(Hydration - thirstTick, 0, 100);

            reproductionUrge = ReproductionUrge;

            if (Nourishment == 0)
                Health -= hungerTick;

            if (Hydration == 0)
                Health -= thirstTick;

            if (Health <= 0)
                Destroy(gameObject);
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

            nourishment = baseHunger;
            hydration = baseThirst;
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
                x = (Mathf.Cos(randomAngle) * lineOfSightRadius * 2) + position.x;
                z = (Mathf.Sin(randomAngle) * lineOfSightRadius * 2) + position.z;
                loopCap++;
            } while ((IsOutOfBounds(new Vector3(x, 0, z)) || IsInWater(new Vector3(x, 0, z))) && loopCap < 16);
            return new Vector3(x, 0, z);
        }

        private bool IsOutOfBounds(Vector3 position)
        {
            return !Physics.Raycast(position + new Vector3(0, 2, 0), Vector3.down, 5);  // Launches raycast from vector offset directly downward to find if is out of bounds
        }

        private bool IsInWater(Vector3 position)
        {
            return Physics.Raycast(position + new Vector3(0, 2, 0), Vector3.down, 5, fov.WaterLayerMask);  // Launches raycast from vector offset directly downward to find if is out of bounds
        }

        protected abstract Priority GetPriority();
        protected abstract Vector3 GetNextDestination();
        protected abstract Action GetNextAction();
        protected abstract Animal GetMatingPartner();
    }
}

