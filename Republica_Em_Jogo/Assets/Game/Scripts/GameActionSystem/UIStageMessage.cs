using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class UIStageMessage : MonoBehaviour
    {
        [SerializeField] private TMP_Text text_gameMessage;
        [SerializeField] private float messageTransitionTime = 0.5f;

        void Start()
        {
            text_gameMessage.SetText("");

            GameStateEmitter.Message += OnMessage;
        }

        private void OnDestroy()
        {
            GameStateEmitter.Message -= OnMessage;
            StopAllCoroutines();
        }

        private void OnMessage(string message)
        {
            StartCoroutine(AminMessage(message));

        }

        IEnumerator AminMessage(string message)
        {
            text_gameMessage.DOFade(0, messageTransitionTime);
            yield return new WaitForSeconds(messageTransitionTime);
            text_gameMessage.SetText(message);
            text_gameMessage.DOFade(1, messageTransitionTime);
        }

    }
}
