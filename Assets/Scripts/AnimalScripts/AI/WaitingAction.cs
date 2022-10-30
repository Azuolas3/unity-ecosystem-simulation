using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;


namespace EcosystemSimulation
{
    public class WaitingAction : Action
    {
        Timer timer;

        bool isCountdownOver = false;
        int duration;

        public WaitingAction(Animal actionPerformer, int duration)
        {
            this.performer = actionPerformer;
            actionDestination = actionPerformer.gameObject.Position();

            this.duration = duration;
            timer = new Timer(duration * 1000);
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            isCountdownOver = true;
            timer.Enabled = false;
        }

        public override void Execute()
        {
            OnComplete();
        }

        public override void OnComplete()
        {
            Debug.Log("Timer completed");
            timer.Enabled = false;
            performer.currentPriority = Animal.Priority.None;
            performer.currentAction = null;
        }

        public override void Cancel()
        {
            Debug.Log("Timer cancelled");
            timer.Enabled = false;
            performer.currentPriority = Animal.Priority.None;
            performer.currentAction = null;
        }

        public override bool AreConditionsMet()
        {
            return isCountdownOver;
        }
    }
}
