using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcosystemSimulation
{
    public abstract class Action
    {
        public Animal performer; //object which performs the action
        public abstract Vector3 ActionDestination { get; }

        public abstract void Execute();
        public abstract void Cancel();
        public abstract bool AreConditionsMet();
    }

}

