using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;

namespace Game
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private PairTimeState[] pairsTimeState;
        [SerializeField] private int currentTime = 0;
        private int Minutes => Mathf.FloorToInt(currentTime /60);
        private int Seconds => Mathf.FloorToInt(currentTime % 60);
        public event Action<int> timeChange;
        private TMP_Text text_Timer;



        void Start()
        {
            text_Timer = GetComponentInChildren<TMP_Text>();
            timeChange += SetTimerText;
            StartCoroutine(ClockUpdate());

            CoreLoopStateHandler.Instance.estadoMuda += OnCoreLoopStateMuda;
        }


        private void OnDestroy()
        {
            timeChange -= SetTimerText;
            CoreLoopStateHandler.Instance.estadoMuda -= OnCoreLoopStateMuda;

        }


        private IEnumerator ClockUpdate()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (currentTime > 0)
                {
                    currentTime--;
                    timeChange?.Invoke(currentTime);
                }
            }
        }

        private void SetTimerText(int currentTime)
        {
            string timeFormated = string.Format("{0}:{1:00}",Minutes, Seconds);
            text_Timer.SetText(timeFormated);
        }

        private void OnCoreLoopStateMuda(CoreLoopState state)
        {
            foreach (PairTimeState pair in pairsTimeState)
            {
                if (pair.CoreLoopState == state)
                {
                    
                    currentTime = pair.TimeInSeconds  + 1;
                    break;
                }
            }
        }



    }


    [System.Serializable]
    public struct PairTimeState
    {
        [SerializeField] private CoreLoopState coreLoopState;
        [SerializeField] private int timeInSeconds;

        public CoreLoopState CoreLoopState => coreLoopState;
        public int TimeInSeconds => timeInSeconds;
    }

}


