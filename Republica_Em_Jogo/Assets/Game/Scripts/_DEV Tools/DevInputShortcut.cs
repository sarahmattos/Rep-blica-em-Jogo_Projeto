using System.Collections;
using System.Collections.Generic;
using Game.UI;
using Unity.Netcode;
using UnityEngine;

namespace Game.Tools
{
    [System.Serializable]
    public class DevInputShortcut
    {
        [Header("Tecla padrão para ser pressionada junto com a tecla de atalho.")]
        [SerializeField] private KeyCode defaultInput = KeyCode.LeftShift;
        
        [Header("Tecla de atalho pra habilitar os comandos de dev secretos ;D")]
        [SerializeField] private KeyCode enableDisableToolsKey = KeyCode.Escape;

        [Header("Tecla de atalho.")]
        [SerializeField] private KeyCode enableNextStateButton = KeyCode.F1;
        [SerializeField] private KeyCode eraseLogger = KeyCode.Backspace;
        public KeyCode EnableDisableToolsKey => enableDisableToolsKey;
        public KeyCode DefaultInput => defaultInput;


        public void InputCalls()
        {
            EnableNextStateButton();
            EraseLogger();


        }




        private void EnableNextStateButton()
        {
            if (Input.GetKey(defaultInput) && Input.GetKeyDown(enableNextStateButton))
            {
                UICoreLoop.Instance.NextStateButton.gameObject.SetActive(true);
                UICoreLoop.Instance.NextStateButton.enabled = true;
                UICoreLoop.Instance.NextStateButton.interactable = true;
            }
        }

        private void EraseLogger()
        {
            if (Input.GetKey(defaultInput) && Input.GetKeyDown(eraseLogger))
            {
                Logger.Instance.ResetLogger();
            }
        }



    }
}
