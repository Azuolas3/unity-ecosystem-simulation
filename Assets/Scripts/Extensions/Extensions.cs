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
    }
}
