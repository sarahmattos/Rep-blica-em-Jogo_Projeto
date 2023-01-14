using Game.Tools;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Game {


    
    public abstract class State : NetworkBehaviour
    {
        protected GameStateHandler stateHandler => GameStateHandler.Instance;
        public  Action Entrada;
        public  Action Saida;

        public abstract void EnterState();
        public abstract void ExitState();

        public void InvokeEntrada()
        {
            Entrada?.Invoke();
            EnterState();
        }

        public void InvokeSaida()
        {
            Saida?.Invoke();
            ExitState();

        }






    }



}


