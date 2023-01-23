using System;
using TMPro;
using UnityEngine;

namespace Game.Networking
{
    public class UIJoinCodeInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text text_joinCode;
        private void Start()
        {
            OnlineRelayManager.Instance.joinCodeGenerated += OnJoinCodeGenerated;
        }

        private void OnDestroy()
        {
            OnlineRelayManager.Instance.joinCodeGenerated -= OnJoinCodeGenerated;

        }

        private void OnJoinCodeGenerated(string code)
        {
            text_joinCode.SetText(code);
        }
    }
}
