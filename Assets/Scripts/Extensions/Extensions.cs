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

        public static Vector3 RotateByAngle(this Vector3 vector, float angle) // Rotates Vector by given euler angle in degrees
        {
            vector = Quaternion.Euler(0, angle, 0) * vector;
            return vector;
        }

        public static bool IsClose(this Vector3 position, Vector3 destination, float distance) //Checking whether one vector is some distance away from another vector
        {
            return Mathf.Abs(position.x - destination.x) < distance && //Doing this with Abs instead of Vector3.Distance for performance
               Mathf.Abs(position.z - destination.z) < distance;
        }

        public static bool IsFar(this Vector3 position, Vector3 destination, float distance) //Checking whether one vector is some distance away from another vector
        {
            return Mathf.Abs(position.x - destination.x) > distance && //Doing this with Abs instead of Vector3.Distance for performance
               Mathf.Abs(position.z - destination.z) > distance;
        }
    }
}
