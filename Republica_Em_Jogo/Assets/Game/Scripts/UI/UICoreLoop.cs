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

namespace Game
{
    public class UICoreLoop : MonoBehaviour
    {
        [SerializeField] private Button nextStateButton;
        [SerializeField] private TMP_Text logStateText;
        public State DesenvState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.DESENVOLVIMENTO);
        public State distribuicaoState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.DISTRIBUICAO];

        private void Awake()
        {
            nextStateButton.gameObject.SetActive(false);
        }

        private void Start()
        {
            nextStateButton.onClick.AddListener(OnNextStateButtonClick);
            distribuicaoState.Entrada += OnDistribuicaoEntrada;
            distribuicaoState.Saida += OnDistribuicaoSaida;
            TurnManager.Instance.vezDoPlayerLocal += OnPlayerTurnUpdate;
            DesenvState.Entrada += OnDesenvolvimento;
            CoreLoopStateHandler.Instance.estadoMuda += UpdateTextDesenv;

        }

        private void OnDistribuicaoSaida()
        {
            if (TurnManager.Instance.LocalIsCurrent)
                nextStateButton.gameObject.SetActive(true);
        }

        private void OnDistribuicaoEntrada()
        {
            if (TurnManager.Instance.LocalIsCurrent)
                nextStateButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            distribuicaoState.Entrada += OnDistribuicaoEntrada;
            distribuicaoState.Saida += OnDistribuicaoSaida;
            TurnManager.Instance.vezDoPlayerLocal -= OnPlayerTurnUpdate;
            DesenvState.Entrada -= OnDesenvolvimento;
            CoreLoopStateHandler.Instance.estadoMuda -= UpdateTextDesenv;

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
            }

        }

        private void OnPlayerTurnUpdate(bool value)
        {
            nextStateButton.gameObject.SetActive(value);


            UpdateTextDesenv(Extensoes.KeyByValue(CoreLoopStateHandler.Instance.StatePairValues,
                CoreLoopStateHandler.Instance.CurrentState));

        }

        private void UpdateTextDesenv(CoreLoopState state)
        {
            logStateText.SetText(string.Concat(GameDataConfig.Instance.TagParticipante, " ", TurnManager.Instance.PlayerAtual, " no estado: ", state));
        }

    }
}

