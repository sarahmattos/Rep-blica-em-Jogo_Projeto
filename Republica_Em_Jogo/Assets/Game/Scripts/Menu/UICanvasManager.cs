using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Game.Networking
{

    public class UICanvasManager : MonoBehaviour
    {
        [SerializeField] private Canvas CanvasInicial;
        [SerializeField] private Canvas CanvasPosConexao;

        [SerializeField] private TMP_Text textModoConexao;

        private void Start()
        {
            CanvasPosConexao.enabled = false;
            CanvasInicial.enabled = true;

            OfflineConnection.Instance.conexaoEstabelecida += OnConexaoEstabelecida;
            OfflineConnection.Instance.conexaoEstabelecida += OfflineSetup;
            OnlineConnection.Instance.conexaoEstabelecida += OnConexaoEstabelecida;
            OnlineConnection.Instance.conexaoEstabelecida += OnlineSetup;

        }

        private void OnDestroy()
        {
            OfflineConnection.Instance.conexaoEstabelecida -= OnConexaoEstabelecida;
            OfflineConnection.Instance.conexaoEstabelecida -= OfflineSetup;
            OnlineConnection.Instance.conexaoEstabelecida -= OnConexaoEstabelecida;
            OnlineConnection.Instance.conexaoEstabelecida -= OnlineSetup;

        }

        private void OnConexaoEstabelecida(bool value)
        {
            CanvasInicial.enabled = !value;
            CanvasPosConexao.enabled = value;
        }

        private void OnlineSetup(bool value)
        {
            UIJoinCodeInfo.Instance.gameObject.SetActive(value);
            IPManager.Instance.gameObject.SetActive(!value);
        }
        private void OfflineSetup(bool value)
        {
            UIJoinCodeInfo.Instance.gameObject.SetActive(!value);
            IPManager.Instance.gameObject.SetActive(value);
        }


    }
}
