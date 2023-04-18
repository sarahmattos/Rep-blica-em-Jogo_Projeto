using Game.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Logger = Game.Tools.Logger;

namespace Game.UI
{
    public class UICoreLoop : Singleton<UICoreLoop>
    {
        [SerializeField] private Button nextStateButton;
        [SerializeField] private TMP_Text logStateText;
        [SerializeField] public TMP_Text ExplicaStateTextTitulo;
        [SerializeField] public TMP_Text ExplicaStateTextCorpo;
        [SerializeField] public GameObject ExplicaStateUi;
        private RodadaController rodadaController;
        public State DesenvState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.DESENVOLVIMENTO);

        private string TagPlayerAtualStilizado
        {
            get
            {
                return string.Concat(GameDataconfig.Instance.TagParticipante, " ", TurnManager.Instance.PlayerAtual);

            }
        }


        private void Awake()
        {
            nextStateButton.gameObject.SetActive(false);
        }

        private void Start()
        {
            nextStateButton.onClick.AddListener(OnNextStateButtonClick);

            TurnManager.Instance.vezDoPlayerLocal += OnPlayerTurnUpdate;
            DesenvState.Entrada += OnDesenvolvimento;
            CoreLoopStateHandler.Instance.estadoMuda += UpdateTextDesenv;

        }



        private void OnDestroy()
        {
            TurnManager.Instance.vezDoPlayerLocal -= OnPlayerTurnUpdate;
            DesenvState.Entrada -= OnDesenvolvimento;
            CoreLoopStateHandler.Instance.estadoMuda -= UpdateTextDesenv;

        }

        // public void OnNextTurnButtonClick()
        // {
        //     if (CoreLoopStateHandler.Instance.CurrentStateIgualUltimoState)
        //     {
        //         CoreLoopStateHandler.Instance.NextStateServerRpc();
        //         return;
        //     }
        //     TurnManager.Instance.NextTurn();
        //     CoreLoopStateHandler.Instance.ChangeStateServerRpc(0);
        // }

        public void OnNextStateButtonClick()
        {
            CoreLoopStateHandler.Instance.NextStateServerRpc();

        }


        private void OnDesenvolvimento()
        {
            // if (TurnManager.Instance.LocalIsCurrent)
            // {
            nextStateButton.gameObject.SetActive(true);
            // }

        }

        private void OnPlayerTurnUpdate(bool value)
        {
            nextStateButton.gameObject.SetActive(value);

            UpdateTextDesenv(Extensoes.KeyByValue(CoreLoopStateHandler.Instance.StatePairValues,
                CoreLoopStateHandler.Instance.CurrentState));

        }

        private void UpdateTextDesenv(CoreLoopState state)
        {
            UpdateTitleTextWithPlayerTag(string.Concat(" entrou no estado ", state));
        }

        public void MostrarAvisoEstado(string avisoTitulo,string avisoCorpo)
        {
            rodadaController = FindObjectOfType<RodadaController>();
            int rodada = rodadaController.Rodada;
            if (rodada <= 1)
            {
                ExplicaStateTextTitulo.text = avisoTitulo;
                ExplicaStateTextCorpo.text = avisoCorpo;
                if (TurnManager.Instance.LocalIsCurrent) ExplicaStateUi.SetActive(true);
            }
        }

        public void UpdateTitleText(string message)
        {
            logStateText.SetText(message);
        }

        public void UpdateTitleTextWithPlayerTag(string message)
        {
            UpdateTitleText(string.Concat(GameDataconfig.Instance.TagPlayerAtualColorizada(), message));
        }



    }

}

