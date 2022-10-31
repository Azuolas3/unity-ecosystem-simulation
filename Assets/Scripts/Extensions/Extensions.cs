using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public static class Extensions
    {
        public static Vector3 Position(this GameObject gameObject) // dumb extension to prevent writing .transform.position all the time
        {
            return gameObject.transform.position;
        }

        public static GameObject Child(this GameObject gameObject, int index = 0) // dumb extension to prevent writing .transform.GetChild(0).gameObject all the time
        {
            return gameObject.transform.GetChild(index).gameObject;
        }
    }
}
