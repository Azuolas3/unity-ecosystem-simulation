using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public abstract class Action
    {
        protected Animal performer; //object which performs the action
        public Vector3 actionDestination;

        public abstract void Execute();
        public abstract void OnComplete();
        public abstract bool IsInRange();
    }

}

