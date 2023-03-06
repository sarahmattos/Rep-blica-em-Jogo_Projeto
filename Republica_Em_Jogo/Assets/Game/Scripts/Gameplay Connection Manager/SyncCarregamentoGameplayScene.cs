using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class SyncCarregamentoGameplayScene : MonoBehaviour
    {
        [SerializeField] private Canvas CanvasUiCarregamento;
        [SerializeField] private TMP_Text LoadingMessageText;
        [SerializeField] private string message;

        public State GameplaySceneLoad => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.GAMEPLAY_SCENE_LOAD);

        private void Start()
        {
            CanvasUiCarregamento.enabled = true;
            LoadingMessageText.SetText(message);
            GameplaySceneLoad.Saida += OnGameplaySceneLoad;
        }

        private void OnDestroy()
        {
            GameplaySceneLoad.Saida -= OnGameplaySceneLoad;

        }


        private void OnGameplaySceneLoad()
        {
            CanvasUiCarregamento.enabled = false;
            LoadingMessageText.SetText("");
        }




    }
}
