using TMPro;
using UnityEngine;

namespace Game.Networking
{
    public class UIModoConexao : MonoBehaviour
    {
        [SerializeField] private TMP_Text textModoConexao;

        private void Start()
        {

            OfflineConnection.Instance.conexaoEstabelecida += OfflineConexao;
            OnlineConnection.Instance.conexaoEstabelecida += OnlineConexao ;
        }

        private void OnDestroy()
        {


            OfflineConnection.Instance.conexaoEstabelecida -= OfflineConexao;
            OnlineConnection.Instance.conexaoEstabelecida -= OnlineConexao;
        }

        private void OfflineConexao(bool obj)
        {
            textModoConexao.SetText("OFFLINE");
        }

        private void OnlineConexao(bool obj)
        {
            textModoConexao.SetText("ONLINE");

        }




    }
}
