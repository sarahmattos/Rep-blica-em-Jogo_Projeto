using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameHandler : MonoBehaviour
    {
        //private string playerName;
        private TMP_InputField inputField;
        //public string PlayerName { get => playerName; set => playerName = value; }
        public string GetPlayerName => PlayerPrefs.GetString("playerName");

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
            inputField.text = GetPlayerName;
        }



    }

}
