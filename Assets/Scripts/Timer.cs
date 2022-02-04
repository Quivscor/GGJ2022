using System;
using System.Collections;
using System.Collections.Generic;

namespace DrunkenDwarves
{
    /// <summary>
    /// Basic clock that calls event when time reaches zero. Can execute multiple events. Owner has to use Timer.Update function in its
    /// update for it to work properly.
    /// </summary>
    public class Timer
    {
        private float duration;
        private float timeToFinish;
        private Action onTimerFinished;

        private bool isHasStopped;

        public Timer() { }

        public Timer(float duration, Action onTimerFinished)
        {
            isHasStopped = false;

            this.duration = duration;
            timeToFinish = duration;
            this.onTimerFinished += onTimerFinished;
        }

        public Timer(float duration, List<Action> onTimerFinished)
        {
            isHasStopped = false;

            this.duration = duration;
            timeToFinish = duration;
            foreach (Action a in onTimerFinished)
            {
                this.onTimerFinished += a;
            }
        }

        /// <summary>
        /// Resets timer back to full time duration
        /// </summary>
        public void RestartTimer()
        {
            isHasStopped = false;
            timeToFinish = duration;
        }

        /// <summary>
        /// Used to internally update timer
        /// </summary>
        /// <param name="deltaTime">Use Time.deltaTime or similar time based variable</param>
        public void Update(float deltaTime)
        {
            timeToFinish -= deltaTime;
            if (!isHasStopped && timeToFinish <= 0)
            {
                isHasStopped = true;
                onTimerFinished?.Invoke();
            }
        }
    }
}