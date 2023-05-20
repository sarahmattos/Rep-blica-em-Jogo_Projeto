using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tools
{
    public class LoggerDisplayer : MonoBehaviour
    {
        Animator animator;
        private bool loggerIsActive = true;
        private Button displayerButton;
        private Canvas CanvasLogger;
        public bool LoggerIsActive => loggerIsActive;

        public event Action<bool> logButtonClicked;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            displayerButton = GetComponentInChildren<Button>();
            CanvasLogger = GetComponent<Canvas>();

        }
        private void Start()
        {
            displayerButton.onClick.AddListener(SwitchLogger);

            CanvasLogger.enabled = GameDataconfig.Instance.DevConfig.MostrarLog;
        }


        private void SwitchLogger()
        {
            loggerIsActive = !loggerIsActive;
            animator.SetBool("ShowLog", loggerIsActive);
            logButtonClicked?.Invoke(loggerIsActive);
        }

        public void SwitchLoggerVisibility()
        {
            CanvasLogger.enabled = !CanvasLogger.enabled;
        }



    }
}
