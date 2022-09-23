using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public abstract class Animal : MonoBehaviour
    {
        protected GameObject animalObject;
        protected FieldOfView fov;

        protected float hunger;
        protected float thirst;

        protected float speed;

        public void Update()
        {
            //fov.GetNearbyColliders(animalObject.transform.position, 10);
            //Move(new Vector3(25, 0, 25));
        }

        public void Init(GameObject animalObject, float baseHunger, float baseThirst, float baseSpeed)
        {
            this.animalObject = animalObject;
            fov = new FieldOfView();

            hunger = baseHunger;
            thirst = baseThirst;
            speed = baseSpeed;
            Collider[] colliders = fov.GetNearbyColliders(animalObject.transform.position, 3);
            foreach(Collider collider in colliders)
            {
                Debug.Log(collider.gameObject);
            }
        }

        public abstract void Move(Vector3 destination);
    }
}

