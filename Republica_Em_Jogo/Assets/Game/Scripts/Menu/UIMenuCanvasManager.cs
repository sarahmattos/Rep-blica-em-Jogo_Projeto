using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Game.Networking
{

    public class UIMenuCanvasManager : MonoBehaviour
    {
        [SerializeField] private Canvas CanvasInicial;
        [SerializeField] private Canvas CanvasPosConexao;
        [SerializeField] private Canvas CanvasCarregamento;
        [SerializeField] private Canvas canvasNameHandler;

        private void Start()
        {
            ResetCanvasRenders();

            OfflineConnection.Instance.conexaoEstabelecida += OnConexaoEstabelecida;
            OfflineConnection.Instance.conexaoEstabelecida += OfflineSetup;
            OnlineConnection.Instance.conexaoEstabelecida += OnConexaoEstabelecida;
            OnlineConnection.Instance.conexaoEstabelecida += OnlineSetup;
            OnConexao.Instance.Disconnect += ResetCanvasRenders;
            OnlineRelayManager.Instance.connecting += OnConnecting;
        }

        private void OnDestroy()
        {
            OfflineConnection.Instance.conexaoEstabelecida -= OnConexaoEstabelecida;
            OfflineConnection.Instance.conexaoEstabelecida -= OfflineSetup;
            OnlineConnection.Instance.conexaoEstabelecida -= OnConexaoEstabelecida;
            OnlineConnection.Instance.conexaoEstabelecida -= OnlineSetup;
            OnConexao.Instance.Disconnect -= ResetCanvasRenders;
            OnlineRelayManager.Instance.connecting -= OnConnecting;
            StopAllCoroutines();
        }

        public void ResetCanvasRenders()
        {
            CanvasPosConexao.enabled = false;
            CanvasInicial.enabled = true;
            canvasNameHandler.enabled = true;
            CanvasCarregamento.enabled = false;
        }


        private void OnConexaoEstabelecida(bool value)
        {
            CanvasInicial.enabled = !value;
            canvasNameHandler.enabled = !value;
            CanvasPosConexao.enabled = value;
            canvasNameHandler.enabled = value;


            if (!value) CanvasCarregamento.enabled = false;
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

        private void OnConnecting(bool value)
        {
            CanvasCarregamento.enabled = value;
            StartCoroutine(DesabilitaCarregamentoAtrasado(5));
        }

        private IEnumerator DesabilitaCarregamentoAtrasado(float s)
        {
            yield return new WaitForSeconds(s);
            CanvasCarregamento.enabled = false;
        }

    }
}
