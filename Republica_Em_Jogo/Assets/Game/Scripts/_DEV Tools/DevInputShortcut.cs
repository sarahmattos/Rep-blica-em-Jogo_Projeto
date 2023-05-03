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
        [SerializeField] private KeyCode defaultInput1 = KeyCode.LeftControl;
        [SerializeField] private KeyCode enableNextStateButton;



        public void InputCalls()
        {
            EnableNextStateButton();



        }




        private void EnableNextStateButton()
        {
            if (Input.GetKeyDown(defaultInput1) && Input.GetKeyDown(enableNextStateButton))
            {
                UICoreLoop.Instance.NextStateButton.gameObject.SetActive(true);
                UICoreLoop.Instance.NextStateButton.enabled = true;
            }
        }




    }
}
