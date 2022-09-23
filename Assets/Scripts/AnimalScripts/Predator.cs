using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EcosystemSimulation
{
    public class Predator : Animal
    {
        public override void Move(Vector3 destination)
        {
            float step = speed * Time.deltaTime;
            animalObject.transform.position = Vector3.MoveTowards(animalObject.transform.position, destination, step);
        }
    }
}
