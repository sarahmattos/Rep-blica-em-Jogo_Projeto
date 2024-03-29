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
        [SerializeField] private KeyCode botoesMultifuncoes = KeyCode.E;
        [SerializeField] private KeyCode showLogger = KeyCode.F2;
        public KeyCode EnableDisableToolsKey => enableDisableToolsKey;
        public KeyCode DefaultInput => defaultInput;


        public void InputCalls()
        {
            EnableNextStateButton();
            EraseLogger();
            HabilitarBotoesMultifuncoes();


        }




        private void EnableNextStateButton()
        {
            if (Input.GetKey(defaultInput) && Input.GetKeyDown(enableNextStateButton))
            {
                if(UICoreLoop.Instance == null) return;
                UICoreLoop.Instance?.NextStateButton.gameObject.SetActive(true);
                UICoreLoop.Instance.NextStateButton.enabled = true;
                UICoreLoop.Instance.NextStateButton.interactable = true;
            }
        }

        private void EraseLogger()
        {
            if (Input.GetKey(defaultInput) && Input.GetKeyDown(eraseLogger))
            {
                Logger.Instance?.ResetLogger();
            }
        }


        private void HabilitarBotoesMultifuncoes()
        {
            if (Input.GetKey(defaultInput) && Input.GetKeyDown(botoesMultifuncoes))
            {

                HudStatsJogador.Instance?.BntsAuxiliares();
            }
        }

        private void HabilitarLogger()
        {
            if (Input.GetKey(defaultInput) && Input.GetKeyDown(showLogger))
            {

                Logger.Instance.LoggerDisplayer.SwitchLoggerVisibility();
            }
        }




    }
}
