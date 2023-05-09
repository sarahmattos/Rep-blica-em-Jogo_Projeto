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
        public Button NextStateButton => nextStateButton;
        private string TagPlayerAtualStilizado
        {
            get
            {
                return string.Concat(GameDataconfig.Instance.TagParticipante, " ", TurnManager.Instance.PlayerAtual);

            }
        }

        private void Start()
        {
            nextStateButton.onClick.AddListener(OnNextStateButtonClick);

            TurnManager.Instance.vezDoPlayerLocal += OnPlayerTurnUpdate;
            CoreLoopStateHandler.Instance.estadoMuda += UpdateTextDesenv;
            nextStateButton.gameObject.SetActive(false);

        }



        private void OnDestroy()
        {
            TurnManager.Instance.vezDoPlayerLocal -= OnPlayerTurnUpdate;
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


        private void OnPlayerTurnUpdate(bool value)
        {
            nextStateButton.gameObject.SetActive(value);

            UpdateTextDesenv(Tools.CollectionExtensions.KeyByValue(CoreLoopStateHandler.Instance.StatePairValues,
                CoreLoopStateHandler.Instance.CurrentState));

        }

        private void UpdateTextDesenv(CoreLoopState state)
        {
            UpdateTitleText(state);
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

        public void UpdateTitleText(CoreLoopState state)
        {
            string titleText = string.Concat(GameDataconfig.Instance.TagPlayerAtualColorizada(),"  ", state.ToString());
            logStateText.SetText(titleText);
        }


    }

}

