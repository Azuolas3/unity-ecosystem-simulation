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

        public static bool IsClose(this Vector3 position, Vector3 destination, float distance) // a more performant (though less accurate) version of Vector3.Distance()
        {
            return Mathf.Abs(position.x - destination.x) < distance && //Doing this with Abs instead of Vector3.Distance for performance
               Mathf.Abs(position.z - destination.z) < distance;
        }

        public static bool IsFar(this Vector3 position, Vector3 destination, float distance) // a more performant (though less accurate) version of Vector3.Distance()
        {
            return Mathf.Abs(position.x - destination.x) > distance && //Doing this with Abs instead of Vector3.Distance for performance
               Mathf.Abs(position.z - destination.z) > distance;
        }
    }
}
