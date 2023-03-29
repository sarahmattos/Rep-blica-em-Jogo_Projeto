using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class InfoLogButton : MonoBehaviour
    {
        private LoggerDisplayer loggerDisplayer;
        private Button logButton;
        [SerializeField] private TMP_Text infoTextOnHoverButton;
        private bool loggerIsActive => loggerDisplayer.LoggerIsActive;

        private void Awake()
        {
            loggerDisplayer = GetComponentInParent<LoggerDisplayer>();
            logButton = GetComponent<Button>();

        }

        private void Start()
        {
            loggerDisplayer.logButtonClicked += SetInfoTextByStateButton;
        }

        private void OnDestroy()
        {
            loggerDisplayer.logButtonClicked -= SetInfoTextByStateButton;
        }


        // private void OnMouseEnter()
        // {
        //     if (!logButton.IsInteractable()) return;
        //     infoTextOnHoverButton.enabled = true;
        // }

        // private void OnMouseExit()
        // {
        //     infoTextOnHoverButton.enabled = false;
        // }

        private void SetInfoTextByStateButton(bool value)
        {
            if (value) infoTextOnHoverButton.SetText("Mostrar Log");
            else infoTextOnHoverButton.SetText("Esconder Log");
        }


    }
}
