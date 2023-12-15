using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class MaxConnectionsHandler : MonoBehaviour
    {

        [SerializeField] private TMP_Dropdown dropdown;
        public UnityEvent<int> OnDropdownValueChange;
        public TMP_Dropdown Dropdown => dropdown;

        [SerializeField] private bool changeWithAllHandlers;

        private void Start()
        {
            ConfigFirstStartGame();
            SetDropdownValueByPlayerPrefs();
            TryRemove1playerOption();
            dropdown.onValueChanged.AddListener(OnDropdownOptionChange);


        }

        private void ConfigFirstStartGame()
        {
            if (!PlayerPrefs.HasKey("maxConnections") && GameDataconfig.Instance.DevConfig.MostarOpcao1player)
            {
                PlayerPrefs.SetInt("maxConnections", 1);
                GameDataconfig.Instance.SetMaxConnections(1);
            }
        }



        private void TryRemove1playerOption()
        {
            if (!GameDataconfig.Instance.DevConfig.MostarOpcao1player)
            {
                dropdown.options.RemoveAt(0);

            }
        }

        private void OnDestroy()
        {
            dropdown.onValueChanged.RemoveListener(OnDropdownOptionChange);

        }

        private void SetDropdownValueByPlayerPrefs()
        {
            foreach (TMP_Dropdown.OptionData optionData in dropdown.options)
            {
                if (int.Parse(optionData.text) == PlayerPrefs.GetInt("maxConnections"))
                {
                    int indexOption = dropdown.options.IndexOf(optionData);
                    dropdown.SetValueWithoutNotify(indexOption);
                }

            }
        }
        public void OnDropdownOptionChange(int value)
        {
            int dropdownValue = int.Parse(dropdown.options[value].text);
            OnDropdownValueChange?.Invoke(dropdownValue);
        }




    }
}
