using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timer
{
    public class Timer
    {
        private float maxTimerValue;

        public float MaxTimerValue
        {
            get
            {
                return maxTimerValue;
            }
            set
            {
                maxTimerValue = value;
            }
        }

        //private float currentTimerValue;
        //public float CurrentTimerValue
        //{
        //    get;
        //}

        public Timer(float maxTimerValue)
        {
            this.maxTimerValue = maxTimerValue;
            StartTimer();
        }

        void StartTimer()
        {

        }

        IEnumerator TimerRoutine()
        {
            yield return new WaitForSeconds(maxTimerValue);
        }


    }

}

