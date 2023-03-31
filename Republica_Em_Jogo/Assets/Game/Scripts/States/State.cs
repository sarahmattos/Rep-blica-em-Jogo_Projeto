using Game.Tools;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Game
{



    public abstract class State : NetworkBehaviour
    {
        public Action Entrada;
        public Action Saida;
        public abstract void EnterState();
        public abstract void ExitState();

        public void InvokeEntrada()
        {
            EnterState();
            Entrada?.Invoke();

        }

        public void InvokeSaida()
        {
            ExitState();
            Saida?.Invoke();


        }






    }



}


