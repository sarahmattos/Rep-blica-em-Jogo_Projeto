using System;
using System.Collections;
using System.Collections.Generic;
using Game.Player;
using Game.Territorio;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class JogadorForaControl : MonoBehaviour
    {
        private bool jogadorLocalRemovido = false;
        [SerializeField] private EventosParaFimJogo eventosParaFimDeJogo;
        // [SerializeField] private AvancoState avanco;
        // private State RecompensaState => CoreLoopStateHandler.Instance.GetState(CoreLoopState.RECOMPENSA);
        // private State AvancoState => CoreLoopStateHandler.Instance.GetState(CoreLoopState.AVANCO);
        // private State SelecBairroAvancoState => avanco.StateMachineController.GetState((int)AvancoStatus.SELECT_BAIRRO);

        [SerializeField] private GameObject uiPartidoFora;
        private List<Bairro> bairrosPlayerLocal => PlayerStatsManager.Instance.GetLocalPlayerStats().BairrosInControl;

        private void Start()
        {
            uiPartidoFora.SetActive(false);
            eventosParaFimDeJogo.notify += VerificarPlayerLocalNaoTemBairro;

        }



        private void OnDestroy()
        {
            eventosParaFimDeJogo.notify -= VerificarPlayerLocalNaoTemBairro;


        }

        private void VerificarPlayerLocalNaoTemBairro()
        {
            if (jogadorLocalRemovido) return;
            Tools.Logger.Instance.LogInfo("Bairros in control: " + bairrosPlayerLocal.Count);
            if (bairrosPlayerLocal.Count > 0) return;
            RemoverJogador();
        }

        private void RemoverJogador()
        {
            Tools.Logger.Instance.LogInfo("Jogador local remivdo");
            TurnManager.Instance.RemovePlayerIDServerRpc((int)NetworkManager.Singleton.LocalClientId);
            ActiveUIJogadorFora();
            jogadorLocalRemovido = true;

        }

        private void ActiveUIJogadorFora()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            uiPartidoFora.SetActive(true);
        }



    }
}
