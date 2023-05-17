using Game.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameHandler : Singleton<PlayerNameHandler>
    {
        //private string playerName;
        private TMP_InputField inputField;
        public string GetInputNameValue => PlayerPrefs.GetString("playerName");
        // public string InputNameValue => inputField.text;

        private void Awake()
        {
            inputField = GetComponent<TMP_InputField>();
            inputField.onValueChanged.AddListener(UpdatePlayerName);
        }

        void UpdatePlayerName(string value)
        {
            PlayerPrefs.SetString("playerName", value);

        }

        private void Start()
        {
            inputField.text = GetInputNameValue;
        }



    }

}
