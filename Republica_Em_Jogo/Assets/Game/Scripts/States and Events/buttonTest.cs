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
    public class buttonTest : MonoBehaviour
    {
        [SerializeField] private Button nextState;
        [SerializeField] private Button nextTurn;
        [SerializeField] private TMP_Text textState;


        private void Start()
        {
            nextState.gameObject.SetActive(false);
            nextTurn.gameObject.SetActive(false);

            nextTurn.onClick.AddListener(() => {
                TurnManager.Instance.NextTurnServerRpc();
            });

            nextState.onClick.AddListener(() => {
                DesenvolvimentoStateHandler.Instance.NextDesenvStateServerRpc();
            });

            TurnManager.Instance.isLocalPlayerTurn += OnPlayerTurnUpdate;
            GameStateHandler.Instance.desenvolvimento += OnDesenvolvimento;
            DesenvolvimentoStateHandler.Instance.desenvStateIndex.OnValueChanged += UpdateTextDesenv;
        }


        private void OnDestroy()
        {
            TurnManager.Instance.isLocalPlayerTurn -= OnPlayerTurnUpdate;
            GameStateHandler.Instance.desenvolvimento -= OnDesenvolvimento;
            DesenvolvimentoStateHandler.Instance.desenvStateIndex.OnValueChanged -= UpdateTextDesenv;

        }


        private void OnDesenvolvimento()
        {
            if (TurnManager.Instance.LocalIsCurrent) nextState.gameObject.SetActive(true);
        }

        private void OnPlayerTurnUpdate(bool value)
        {
            Logger.Instance.LogError("Current:" + TurnManager.Instance.GetCurrentPlayer);
            Logger.Instance.LogError("Sou o atual: " + value);
            nextState.gameObject.SetActive(value);
            nextTurn.gameObject.SetActive(value);
            
        }

        private void UpdateTextDesenv(int previousValue, int newValue)
        {
            textState.SetText(string.Concat("Player ", TurnManager.Instance.GetCurrentPlayer, " no estado: ", (DesenvolState)DesenvolvimentoStateHandler.Instance.desenvStateIndex.Value));
        }


    }
}

