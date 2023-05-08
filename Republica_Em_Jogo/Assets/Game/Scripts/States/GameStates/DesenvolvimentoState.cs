using System;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(RodadaController))]
    public class DesenvolvimentoState : State
    {
        private RodadaController rodadaController;
        private GameStateHandler gameState => GameStateHandler.Instance;
        public RodadaController RodadaController => rodadaController;

        private void Start()
        {
            rodadaController = GetComponent<RodadaController>();
        }
        public override void EnterState()
        {
            TurnManager.Instance.SetIndexPlayerTurn(0);
            rodadaController.InscreverEvents();

            if (!IsHost) return;
            rodadaController.rodadaMaxAlcancada += GoToEleicao;
        }

        public override void ExitState()
        {
            rodadaController.DesinscreverEvents();
            if (!IsHost) return;
            rodadaController.rodadaMaxAlcancada += GoToEleicao;




        }


        private void GoToEleicao()
        {
            gameState.StateMachineController.ChangeStateServerRpc((int)GameState.ELEICOES);
        }





    }

}