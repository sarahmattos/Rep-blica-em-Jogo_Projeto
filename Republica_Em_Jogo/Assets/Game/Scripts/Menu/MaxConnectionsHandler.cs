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

        [SerializeField]  TMP_Dropdown dropdown;
        public UnityEvent<int> OnDropdownValueChange;
        public TMP_Dropdown Dropdown => dropdown;
        private void Start()
        {
            dropdown.onValueChanged.AddListener(OnDropdownOptionChange);
        }

        private void OnDestroy()
        {
            dropdown.onValueChanged.RemoveListener(OnDropdownOptionChange);

        }
        public void OnDropdownOptionChange(int value)
        {
            int dropdownValue = int.Parse(dropdown.options[value].text);
            OnDropdownValueChange?.Invoke(dropdownValue);
        }

    }
}
