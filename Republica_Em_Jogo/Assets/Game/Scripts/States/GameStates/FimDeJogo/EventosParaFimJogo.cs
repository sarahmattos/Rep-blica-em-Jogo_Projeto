using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EventosParaFimJogo : MonoBehaviour
    {
        [SerializeField] private AvancoState avanco;
        private State RecompensaState => CoreLoopStateHandler.Instance.GetState(CoreLoopState.RECOMPENSA);
        private State AvancoState => CoreLoopStateHandler.Instance.GetState(CoreLoopState.AVANCO);
        [SerializeField] private State SelectBairroAvancoState;


        public event Action notify;

        void Start()
        {
            RecompensaState.Entrada += NotifySubscribers;
            AvancoState.Saida += NotifySubscribers;
            SelectBairroAvancoState.Entrada += NotifySubscribers;


        }

        private void OnDestroy()
        {
            RecompensaState.Entrada -= NotifySubscribers;
            AvancoState.Saida -= NotifySubscribers;
            SelectBairroAvancoState.Entrada -= NotifySubscribers;
        }

        private void NotifySubscribers()
        {
            notify?.Invoke();
        }


    }
}
