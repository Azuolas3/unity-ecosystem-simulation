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

        public Priority currentPriority;
        public Vector3 currentDestination;
        //public Vector3 currentDestination;
        public Action currentAction;

        public GameObject animalObject;
        protected FieldOfView fov;

        protected AnimalStats animalStats;
        public Genes genes;

        [SerializeField]
        protected float health;
        [SerializeField]
        protected float nourishment;
        [SerializeField]
        protected float hydration;
        [SerializeField]
        protected float reproductionUrge;

        private float hungerTick = 0.01f;
        private float thirstTick = 0.01f;
        private float growthTick = 0.001f;

        public float GrowthProgress { get; set; }
        [SerializeField]
        public bool IsGrownUp { get { return GrowthProgress == 1; } }

        public GenderHandler genderHandler;

        [SerializeField]
        AnimalGender gendertest;

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


        protected bool NeedsDestination { get { return animalObject.transform.position == currentDestination; } }
        protected bool NeedsAction { get { return currentAction == null; } }

        [SerializeField]
        private NavMeshAgent navAgent;

        public Collider[] PredatorColliders { get { return fov.GetNearbyColliders(animalObject.transform.position, animalStats.LineOfSightRadius, fov.PredatorLayerMask); } }
        public Collider[] WaterColliders { get { return fov.GetNearbyColliders(animalObject.transform.position, animalStats.LineOfSightRadius, fov.WaterLayerMask); } }
        public Collider[] PlantColliders { get { return fov.GetNearbyColliders(animalObject.transform.position, animalStats.LineOfSightRadius, fov.PlantLayerMask); } }
        public Collider[] PreyColliders { get { return fov.GetNearbyColliders(animalObject.transform.position, animalStats.LineOfSightRadius, fov.PreyLayerMask); } }

        public void Start()
        {
            Health = 100;
            gendertest = genderHandler.Gender;
            //navAgent.autoRepath = false;
        }

        public void Update()
        {       
            currentPriority = GetPriority();

            if (NeedsAction)
            {
                currentAction = GetNextAction();
                Debug.Log("Needs action  " + animalObject.name);

                Debug.Log("Needs action  " + animalObject.name + currentAction + currentAction.ActionDestination);
                currentDestination = currentAction.ActionDestination;

                if (!navAgent.SetDestination(currentDestination))
                    Debug.Log("bad");
            }

            if (currentDestination.IsFar(currentAction.ActionDestination, 0.1f))
            {
                currentDestination = currentAction.ActionDestination;
                navAgent.SetDestination(currentDestination);
            }

            if (currentAction.AreConditionsMet())
            {
                //Debug.Log("action completed " + animalObject.name + currentAction);
                currentAction.Execute();
            }

            Nourishment = Mathf.Clamp(Nourishment - hungerTick, 0, 100);
            Hydration = Mathf.Clamp(Hydration - thirstTick, 0, 100);

            reproductionUrge = ReproductionUrge;

            if (GrowthProgress != 1)
            {
                GrowthProgress = Mathf.Clamp01(GrowthProgress += growthTick);
                
                gameObject.transform.localScale = new Vector3(animalStats.Size, animalStats.Size, animalStats.Size) * GrowthProgress;
                navAgent.speed = (animalStats.MovementSpeed / animalStats.Size) * GrowthProgress;
            }

            if (Nourishment == 0)
                Health -= hungerTick;

            if (Hydration == 0)
                Health -= thirstTick;

            if (Health <= 0)
               OnDeath();
        }

        public void Init(GameObject animalObject, float baseHunger, float baseThirst, float baseSpeed, float baseSightRadius, float growthProgress, GenderHandler gender, Color colour)
        {
            currentPriority = Priority.None;
            currentAction = null;

            this.animalObject = animalObject;
            fov = new FieldOfView();

            Renderer renderer = gameObject.Child().GetComponent<Renderer>();
            renderer.material.color = colour;

            nourishment = baseHunger;
            hydration = baseThirst;
            animalStats = new AnimalStats(gameObject.transform.lossyScale.x, baseSpeed, baseSightRadius, colour); // can take any value of scale
            currentDestination = animalObject.transform.position;
            animalStats.Size = gameObject.transform.lossyScale.x;      
            GrowthProgress = growthProgress;
            navAgent.speed = GrowthProgress * (animalStats.MovementSpeed / animalStats.Size);
            genes = new Genes(animalStats);

            genderHandler = gender;
        }

        public void Init(GameObject animalObject, float baseHunger, float baseThirst, AnimalStats stats, float growthProgress, GenderHandler gender)
        {
            currentPriority = Priority.None;
            currentAction = null;

            this.animalObject = animalObject;
            fov = new FieldOfView();

            Renderer renderer = gameObject.Child().GetComponent<Renderer>();
            renderer.material.color = stats.Colour;

            nourishment = baseHunger;
            hydration = baseThirst;
            animalStats = stats;
            currentDestination = animalObject.transform.position;
            animalStats.Size = gameObject.transform.lossyScale.x;
            GrowthProgress = growthProgress;
            navAgent.speed = GrowthProgress * animalStats.MovementSpeed;
            genes = new Genes(animalStats);
            Debug.Log($"{gameObject.name} stats {stats.MovementSpeed} animalStats {animalStats.MovementSpeed}");
            genderHandler = gender;
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

        public Vector3 GetSearchDestination()
        {
            Vector3 position = gameObject.Position();
            Vector3 searchDestination;
            float randomAngle;

            float x;
            float z;

            int loopCounter = 0;
            do
            {
                randomAngle = Random.Range(-45, 45);
                Vector3 searchDirection = transform.forward.RotateByAngle(randomAngle);
                searchDestination = (searchDirection * animalStats.LineOfSightRadius) + position;
                if (IsInaccessible(searchDestination))
                {
                    Debug.Log("changed path" + searchDestination);
                    searchDirection = -searchDirection;                    
                }

                searchDestination = (searchDirection * animalStats.LineOfSightRadius) + position;
                //x = (Mathf.Cos(randomAngle) * animalStats.LineOfSightRadius * 2) + position.x;
                //z = (Mathf.Sin(randomAngle) * animalStats.LineOfSightRadius * 2) + position.z;
                loopCounter++;
            } while (IsInaccessible(searchDestination) && loopCounter < 32);
            return searchDestination;
        }

        protected Vector3 GetRunningAwayDestination(Collider predator)
        {
            Vector3 position = gameObject.Position();
            Vector3 runningDestination;
            float randomAngle;

            float x;
            float z;

            //Vector3 searchDirection = (position - predator.gameObject.Position()).normalized;

            int loopIteration = 0;
            int directionCount = 0;
            int nDirection = 0;
            // Rather awfully written loop, but the idea is that it tries to find a destination to go to 
            // by prioritising running directly away from the predator and if it cant find a suitable destination forwards, tries the sides
            // and finally just hopes for the best and runs the same direction as the predator is coming from
            do
            {
                Vector3 searchDirection = (position - predator.gameObject.Position()).normalized;
                randomAngle = Random.Range((90 * directionCount) - 45, (90 * directionCount) + 45);
                runningDestination = (searchDirection.RotateByAngle(randomAngle) * animalStats.LineOfSightRadius) + position;

                //runningDestination = (searchDirection * animalStats.LineOfSightRadius) + position;
                loopIteration++;

                if (loopIteration == 15)
                {
                    loopIteration = 0;
                    if (nDirection % 2 == 0)
                    {
                        directionCount++;
                    }

                    directionCount *= -1;
                    nDirection++;
                }

            } while (IsInaccessible(runningDestination) && loopIteration < 16 && nDirection < 3);
            
            return runningDestination;
        }

        public bool IsInaccessible(Vector3 position)
        {
            return MapHelper.Instance.IsInaccessible(position);
        }

        private bool IsOutOfBounds(Vector3 position)
        {
            return MapHelper.Instance.IsOutOfBounds(position);
            //return !Physics.Raycast(position + new Vector3(0, 2, 0), Vector3.down, 5);  // Launches raycast from vector offset directly downward to find if is out of bounds
        }

        private bool IsInWater(Vector3 position)
        {
            return MapHelper.Instance.IsInWater(position);
            //return Physics.Raycast(position + new Vector3(0, 2, 0), Vector3.down, 5, fov.WaterLayerMask);  // Launches raycast from vector offset directly downward to find if is out of bounds
        }

        protected abstract Priority GetPriority();
        protected abstract Action GetNextAction();
        protected abstract void OnDeath();
    }

    public struct AnimalStats
    {
        public AnimalStats(float size, float movementSpeed, float lineOfSightRadius, Color colour)
        {
            this.size = size;
            this.movementSpeed = movementSpeed;
            this.lineOfSightRadius = lineOfSightRadius;
            this.colour = colour;
        }

        public float Size { get { return size; } set { size = value; } }
        public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
        public float LineOfSightRadius { get { return lineOfSightRadius; } set { lineOfSightRadius = value; } }
        public Color Colour { get { return colour; } set { colour = value; } }

        private float size;
        private float movementSpeed;
        private float lineOfSightRadius;
        private Color colour;
    }
}

