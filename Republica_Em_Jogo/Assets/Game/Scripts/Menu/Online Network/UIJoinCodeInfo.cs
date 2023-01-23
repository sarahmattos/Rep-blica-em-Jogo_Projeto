using Game.Tools;
using System;
using TMPro;
using UnityEngine;

namespace Game.Networking
{
    public class UIJoinCodeInfo : Singleton<UIJoinCodeInfo>
    {
        [SerializeField] private TMP_Text text_joinCode;

        private void Start()
        {
            OnlineRelayManager.Instance.joinCodeGenerated += SetJoinCodeText;
            OnlineConnection.Instance.joinCodeConexaoEstabelecida += SetJoinCodeText;
        }


        private void OnDestroy()
        {
            OnlineRelayManager.Instance.joinCodeGenerated -= SetJoinCodeText;
            OnlineConnection.Instance.joinCodeConexaoEstabelecida -= SetJoinCodeText;

        }

        private void SetJoinCodeText(string code)
        {
            text_joinCode.SetText(code.ToUpper());
        }
    }
}
