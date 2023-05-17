using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using Game.UI;
using TMPro;
using Game.Player;

namespace Game
{
    public class infoGeral : NetworkBehaviour
    {
        private NetworkVariable<FixedString4096Bytes> stringInfoPlayer0 = new NetworkVariable<FixedString4096Bytes>();
        private NetworkVariable<FixedString4096Bytes> stringInfoPlayer1 = new NetworkVariable<FixedString4096Bytes>();
        private NetworkVariable<FixedString4096Bytes> stringInfoPlayer2 = new NetworkVariable<FixedString4096Bytes>();
        private NetworkVariable<FixedString4096Bytes> stringInfoPlayer3 = new NetworkVariable<FixedString4096Bytes>();
        private NetworkVariable<int> pedeInfo = new NetworkVariable<int>();
        string info0, info1, info2, info3;
        [SerializeField] private TMP_Text teste0, teste1, teste2, teste3;
        [SerializeField] private GameObject infoGo;
        PlayerStats ps;
        HudStatsJogador hs;
        string textoTotal;
        void Start()
        {
            hs = FindObjectOfType<HudStatsJogador>();
        }
        [ServerRpc(RequireOwnership = false)]
        public void pedeInfoServerRpc(int valor)
        {
            pedeInfo.Value = valor;
        }
        [ServerRpc(RequireOwnership = false)]
        public void AtualizaInfo0GeralServerRpc(string _texto)
        {
            stringInfoPlayer0.Value = _texto;
        }
        [ServerRpc(RequireOwnership = false)]
        public void AtualizaInfo1GeralServerRpc(string _texto)
        {
            stringInfoPlayer1.Value = _texto;
        }
        [ServerRpc(RequireOwnership = false)]
        public void AtualizaInfo2GeralServerRpc(string _texto)
        {
            stringInfoPlayer2.Value = _texto;
        }
        [ServerRpc(RequireOwnership = false)]
        public void AtualizaInfo3GeralServerRpc(string _texto)
        {
            stringInfoPlayer3.Value = _texto;
        }
        private void OnEnable()
        {
            stringInfoPlayer0.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
                {
                    info0 = newValue.ToString();
                    updateInfoUi();
                };
        
            stringInfoPlayer1.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
            {
                info1 = newValue.ToString();
                updateInfoUi();
            };
            stringInfoPlayer2.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
            {
                info2 = newValue.ToString();
                updateInfoUi();
            };
            stringInfoPlayer3.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
            {
                info3 = newValue.ToString();
                updateInfoUi();
            };
            pedeInfo.OnValueChanged += (int previousValue, int newValue) =>
            {
                getInfos();
            };
        }
        private void Update()
        {
            

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                chamarInfo();
                updateUi(true);
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                updateUi(false);
            }
        }
        public void chamarInfo()
        {
            pedeInfoServerRpc(2);
        }
        public void getInfos()
        {
            ps = hs.GetPlayerStats();
            //  textoTotal = ps.Nome+ ": "+ps.EleitoresTotais+" eleitores, "+ps.BairrosInControl.Count.ToString()+
            //" bairros, "+ps.numSaude+" cartas de saúde, "+ps.numEducacao+" cartas de educação, "+ps.numCadeiras+
            // " cadeiras";
            textoTotal = GameDataconfig.Instance.TagPlayerColorizada(ps) + ":       " + ps.numSaude + "          " + ps.numEducacao + "          " + ps.numCadeiras + "        " +
            ps.EleitoresTotais + "        " + ps.BairrosInControl.Count.ToString();
            if (ps.playerID == 0) AtualizaInfo0GeralServerRpc(textoTotal);
            if (ps.playerID == 1) AtualizaInfo1GeralServerRpc(textoTotal);
            if (ps.playerID == 2) AtualizaInfo2GeralServerRpc(textoTotal);
            if (ps.playerID == 3) AtualizaInfo3GeralServerRpc(textoTotal);
            pedeInfoServerRpc(-1);

        }
        public void updateInfoUi()
        {
            teste0.text = info0;
            teste1.text = info1;
            teste2.text = info2;
            teste3.text = info3;
        }
        public void updateUi(bool valor)
        {
            infoGo.SetActive(valor);
        }

    }
}
