using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public class Prey : Animal
    {
        bool isInDanger;


        public override void Move(Vector3 destination)
        {
            float step = speed * Time.deltaTime;
            animalObject.transform.position = Vector3.MoveTowards(animalObject.transform.position, destination, step);

            //if(Vector3.Distance(animalObject.transform.position, destination) > 0.1f)
            //{
            //    Move(destination);
            //}
        }
    }
}

