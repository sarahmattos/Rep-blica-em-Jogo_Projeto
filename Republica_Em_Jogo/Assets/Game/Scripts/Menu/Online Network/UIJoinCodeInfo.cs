using Game.Tools;
using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Game.Networking
{
    public class UIJoinCodeInfo : NetworkSingleton<UIJoinCodeInfo>
    {
        [SerializeField] private TMP_Text text_joinCode;
        public NetworkVariable<FixedString32Bytes> joincode = new NetworkVariable<FixedString32Bytes>();
        private void Start()
        {
            OnlineRelayManager.Instance.joinCodeGenerated += SetJoinCodeText;
            OnlineConnection.Instance.joinCodeConexaoEstabelecida += SetJoinCodeText;

            //if (!NetworkManager.Singleton.IsHost) return;
            //NetworkManager.Singleton.OnClientConnectedCallback += 

        }


        private void OnDestroy()
        {
            OnlineRelayManager.Instance.joinCodeGenerated -= SetJoinCodeText;
            OnlineConnection.Instance.joinCodeConexaoEstabelecida -= SetJoinCodeText;


        }

        private void SetJoinCodeText(string code)
        {
            if (string.IsNullOrEmpty(code)) return;
            text_joinCode.SetText(code.ToUpper());
        }
    }
}
