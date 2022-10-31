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
        public bool isGrownUp = true;

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
            navAgent.autoRepath = false;
        }

        public void Update()
        {       
            currentPriority = GetPriority();

            if (NeedsAction)
            {
                currentAction = GetNextAction();
                //Debug.Log("Needs action  " + animalObject.name + currentAction + currentAction.actionDestination);
                currentDestination = currentAction.actionDestination;
                navAgent.SetDestination(currentDestination);
            }

            //if (navAgent.destination != currentDestination)
            //{
            //    // Unity NavMesh SetDestination() doesn't always work for some reason so we do this
            //    Debug.Log($"{gameObject.name} {currentDestination} {navAgent.destination} {currentAction}");
            //    //navAgent.SetDestination(currentDestination);
            //}

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
                navAgent.speed = animalStats.MovementSpeed * GrowthProgress;
            }

            if (Nourishment == 0)
                Health -= hungerTick;

            if (Hydration == 0)
                Health -= thirstTick;

            if (Health <= 0)
                Destroy(gameObject);
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
            navAgent.speed = GrowthProgress * animalStats.MovementSpeed;
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
            float randomAngle;

            float x;
            float z;

            int loopCap = 0;
            do
            {
                randomAngle = Random.Range(0, 360);
                x = (Mathf.Cos(randomAngle) * animalStats.LineOfSightRadius * 2) + position.x;
                z = (Mathf.Sin(randomAngle) * animalStats.LineOfSightRadius * 2) + position.z;
                loopCap++;
            } while ((IsOutOfBounds(new Vector3(x, 0, z)) || IsInWater(new Vector3(x, 0, z))) && loopCap < 32);
            return new Vector3(x, 0, z);
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
        protected abstract Animal GetMatingPartner();
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

