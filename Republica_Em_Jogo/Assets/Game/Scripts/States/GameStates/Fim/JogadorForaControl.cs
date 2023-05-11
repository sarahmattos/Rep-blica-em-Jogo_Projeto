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
        [SerializeField] private bool jogadorLocalRemovido = false;
        [SerializeField] private AvancoState avanco;
        private State RecompensaState => CoreLoopStateHandler.Instance.GetState(CoreLoopState.RECOMPENSA);
        private State AvancoState => CoreLoopStateHandler.Instance.GetState(CoreLoopState.AVANCO);
        private State SelecBairroAvancoState => avanco.StateMachineController.GetState((int)AvancoStatus.SELECT_BAIRRO);

        [SerializeField] private GameObject uiPartidoFora;

        private void Start()
        {
            jogadorLocalRemovido = false;
            uiPartidoFora.SetActive(false);

            RecompensaState.Saida += VerificaLocalInGame;
            avanco.Saida += VerificaLocalInGame;
            SelecBairroAvancoState.Entrada += VerificaLocalInGame;
        }



        private void OnDestroy()
        {
            RecompensaState.Saida -= VerificaLocalInGame;
            AvancoState.Saida -= VerificaLocalInGame;
            SelecBairroAvancoState.Entrada -= VerificaLocalInGame;

        }

        private void VerificaLocalInGame()
        {
            Tools.Logger.Instance.LogWarning("vERIFICOU SE JOGADOR ESTÁ FORA");
            List<Bairro> bairrosPlayerLocal = PlayerStatsManager.Instance.GetLocalPlayerStats().BairrosInControl;
            if (bairrosPlayerLocal.Count > 0) return;
            if (jogadorLocalRemovido == true) return;
            RemoverJogador();
        }

        private void RemoverJogador()
        {
            TurnManager.Instance.RemovePlayerIDServerRpc((int)NetworkManager.Singleton.LocalClientId);
            ActiveUIJogadorFora();
        }

        private void ActiveUIJogadorFora()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            uiPartidoFora.SetActive(true);
        }



    }
}
