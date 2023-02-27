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

namespace  Game 
{
    public class UICoreLoop : MonoBehaviour
    {
        [SerializeField] private Button nextStateButton;
        [SerializeField] private Button nextTurnButton;
        [SerializeField] private TMP_Text logStateText;
        public State DesenvState => GameStateHandler.Instance.StatePairValue[GameState.DESENVOLVIMENTO];


        private void Awake()
        {
            nextStateButton.gameObject.SetActive(false);
            nextTurnButton.gameObject.SetActive(false);
        }

        private void Start()
        {
            nextTurnButton.onClick.AddListener(OnNextTurnButtonClick);
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

        public void OnNextTurnButtonClick()
        {
            if (CoreLoopStateHandler.Instance.CurrentStateIgualUltimoState)
            {
                CoreLoopStateHandler.Instance.NextStateServerRpc();
                return;
            }
            TurnManager.Instance.NextTurn();
            CoreLoopStateHandler.Instance.ChangeStateServerRpc(0);
        }

        public void OnNextStateButtonClick()
        {
            CoreLoopStateHandler.Instance.NextStateServerRpc();

        }


        private void OnDesenvolvimento()
        {
            if (TurnManager.Instance.LocalIsCurrent)
            {
                nextStateButton.gameObject.SetActive(true);
                nextTurnButton.gameObject.SetActive(true);
            }

        }

        private void OnPlayerTurnUpdate(bool value)
        {
            nextStateButton.gameObject.SetActive(value);
            nextTurnButton.gameObject.SetActive(value);
            

            UpdateTextDesenv(Extensoes.KeyByValue(CoreLoopStateHandler.Instance.StatePairValues, 
                CoreLoopStateHandler.Instance.CurrentState)) ;

        }

        private void UpdateTextDesenv(CoreLoopState state)
        {
            logStateText.SetText(string.Concat(GameDataConfig.Instance.TagParticipante," ", TurnManager.Instance.PlayerAtual, " no estado: ",state));
        }

    }
}

