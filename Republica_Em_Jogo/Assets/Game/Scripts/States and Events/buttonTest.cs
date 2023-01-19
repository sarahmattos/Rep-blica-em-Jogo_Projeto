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
    public class buttonTest : MonoBehaviour
    {
        [SerializeField] private Button nextState;
        [SerializeField] private Button nextTurn;
        [SerializeField] private TMP_Text textState;
        public State DesenvState => GameStateHandler.Instance.StatePairValue[GameState.DESENVOLVIMENTO];


        private void Awake()
        {
            nextState.gameObject.SetActive(false);
            nextTurn.gameObject.SetActive(false);

        }

        private void Start()
        {

            nextTurn.onClick.AddListener(() => {
                TurnManager.Instance.NextTurnServerRpc();
            });

            nextState.onClick.AddListener(() => {
                CoreLoopStateHandler.Instance.NextStateServerRpc();
            });

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


        private void OnDesenvolvimento()
        {
            if (TurnManager.Instance.LocalIsCurrent) nextState.gameObject.SetActive(true);
        }

        private void OnPlayerTurnUpdate(bool value)
        {
            nextState.gameObject.SetActive(value);
            nextTurn.gameObject.SetActive(value);
            

            UpdateTextDesenv(Extensoes.KeyByValue(CoreLoopStateHandler.Instance.StatePairValues, CoreLoopStateHandler.Instance.CurrentState)) ;

        }

        private void UpdateTextDesenv(CoreLoopState state)
        {
            textState.SetText(string.Concat("Player ", TurnManager.Instance.GetPlayerAtual, " no estado: ",state));
        }

    }
}

