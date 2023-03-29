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
        public bool LoggerIsActive => loggerIsActive;

        public event Action<bool> logButtonClicked;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            displayerButton = GetComponentInChildren<Button>();

        }
        private void Start()
        {
            displayerButton.onClick.AddListener(SwitchLogger);
        }


        private void SwitchLogger()
        {
            loggerIsActive = !loggerIsActive;
            animator.SetBool("ShowLog", loggerIsActive);
            logButtonClicked?.Invoke(loggerIsActive);
        }



    }
}
