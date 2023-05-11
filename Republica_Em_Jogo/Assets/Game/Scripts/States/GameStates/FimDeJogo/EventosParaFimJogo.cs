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
        private State SelecBairroAvancoState => avanco.StateMachineController.GetState((int)AvancoStatus.SELECT_BAIRRO);


        public event Action Subscribers;


        void Start()
        {
            RecompensaState.Entrada += NotifySubscribers;
            AvancoState.Saida += NotifySubscribers;
            SelecBairroAvancoState.Entrada += NotifySubscribers;


        }

        private void OnDestroy()
        {
            RecompensaState.Entrada -= NotifySubscribers;
            AvancoState.Saida -= NotifySubscribers;
            SelecBairroAvancoState.Entrada -= NotifySubscribers;
        }

        private void NotifySubscribers()
        {
            Subscribers?.Invoke();
        }


    }
}
