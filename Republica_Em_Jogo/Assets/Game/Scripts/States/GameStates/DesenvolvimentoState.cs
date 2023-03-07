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

        private void Start()
        {
            rodadaController = GetComponent<RodadaController>();
        }
        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("EnterState: DESENVOLVIMENTO");
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