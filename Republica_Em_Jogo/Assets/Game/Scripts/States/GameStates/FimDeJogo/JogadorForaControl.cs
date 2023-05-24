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
        public event Action JogadorFoiRemovido;

        public bool JogadorLocalRemovido => jogadorLocalRemovido;
        private void Start()
        {
            uiPartidoFora.SetActive(false);

            //a verificação e remoção está sendo feita após a saida do avanço para evitar erro nos bairros
            CoreLoopStateHandler.Instance.GetState(CoreLoopState.AVANÇO).Saida += VerificarPlayerLocalNaoTemBairro;

        }



        private void OnDestroy()
        {
            CoreLoopStateHandler.Instance.GetState(CoreLoopState.AVANÇO).Saida += VerificarPlayerLocalNaoTemBairro;


        }

        private void VerificarPlayerLocalNaoTemBairro()
        {
            if (jogadorLocalRemovido) return;
            if (bairrosPlayerLocal.Count > 0) return;
            RemoverJogador();
        }

        private void RemoverJogador()
        {
            //PlayerStatsManager.Instance.RemovePlayerStatsClientRpc((int)NetworkManager.Singleton.LocalClientId);
            TurnManager.Instance.RemovePlayerIDServerRpc((int)NetworkManager.Singleton.LocalClientId);
            ActiveUIJogadorFora();
            jogadorLocalRemovido = true;
            JogadorFoiRemovido?.Invoke();
            PlayerStatsManager.Instance.GetLocalPlayerStats().SetPlayerForaJogo(true);
            Tools.Logger.Instance.LogInfo("JogadorLocalRemovido");

        }

        private void ActiveUIJogadorFora()
        {
            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;
            uiPartidoFora.SetActive(true);
        }



    }
}
