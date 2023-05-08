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
        [SerializeField] private KeyCode defaultInput = KeyCode.LeftControl;
        [Header("keys for dev's shortchut.")]
        [SerializeField] private KeyCode enableNextStateButton;
        [SerializeField] private KeyCode eraseLogger = KeyCode.Backspace;



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
