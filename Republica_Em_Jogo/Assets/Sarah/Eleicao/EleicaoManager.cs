using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Unity.Netcode;
using Game.Territorio;
using Game.Tools;
using Unity.Collections;
using Game.UI;

namespace Game
{
    public class EleicaoManager : NetworkBehaviour
    {

        private NetworkVariable<FixedString4096Bytes> EleicaoText = new NetworkVariable<FixedString4096Bytes>();
        private NetworkVariable<int> conectados = new NetworkVariable<int>();
        public HudStatsJogador hs;
        public static EleicaoManager Instance;
        public int somaEleitores;
        public int[] cadeirasCamara;
        public int cadeirasTotais, minCadeirasVotacao;
        [SerializeField] private List<Bairro> todosBairros;
        private ZonaTerritorial[] zonasTerritoriais;
        public int numConectados;
        public int[] eleitoresPlayers;
        private SetUpZona setUpZona;
        private string cadeiras;
        float valorPeao;
        [SerializeField] GameObject[] peosCamara;
        [SerializeField] GameObject[] Cameras;
        public bool inEleicao = false;
        ///[SerializeField] Transform posicaoCameraEleicao;
        //public Vector3 posicaoAntiga;

        public string explicaTexto, explicaTextoCorpo;
        private UICoreLoop uiCore;
        void Start()
        {
            cadeirasTotais = 12;
            zonasTerritoriais = FindObjectsOfType<ZonaTerritorial>();
            setUpZona = GameObject.FindObjectOfType<SetUpZona>();
            Instance = this;
            minCadeirasVotacao = 7;
            Material material = peosCamara[0].GetComponent<MeshRenderer>().material;
            uiCore = FindObjectOfType<UICoreLoop>();

        }
        [ServerRpc(RequireOwnership = false)]
        public void ClientsConectServerRpc()
        {
            conectados.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
        }
        public void cadeirasInicial()
        {
            cadeirasCamara = new int[numConectados];
            for (int i = 0; i < cadeirasCamara.Length; i++)
            {
                cadeirasCamara[i] = cadeirasTotais / numConectados;
                if (i == (int)NetworkManager.Singleton.LocalClientId)
                {
                    hs.AtualizarCadeirasUI((int)cadeirasCamara[i]);
                }
            }
            ColorirPeao();
        }
        public void ContaTotalEleitores()
        {
            todosBairros = GetBairros();
            somaEleitores = 0;
            foreach (Bairro bairro in todosBairros)
            {
                somaEleitores += bairro.SetUpBairro.Eleitores.contaEleitores;
            }

        }
        private List<Bairro> GetBairros()
        {
            List<Bairro> bairros = new List<Bairro>();
            for (int i = 0; i < zonasTerritoriais.Length; i++)
            {
                bairros.AddAll(zonasTerritoriais[i].Bairros);
            }
            return bairros;
        }
        public void CalcularCadeiras()
        {
            Debug.Log("cadeirastotals:" + cadeirasTotais);
            Debug.Log("soma eleitores:" + somaEleitores);
            cadeiras = "";
            eleitoresPlayers = new int[numConectados];
            cadeirasCamara = new int[numConectados];
            setUpZona.SepararBairrosPorPlayer(eleitoresPlayers, numConectados);
            for (int i = 0; i < eleitoresPlayers.Length; i++)
            {
                float aux = ((float)eleitoresPlayers[i] * (float)cadeirasTotais) / (float)somaEleitores;

                cadeirasCamara[i] = (int)Mathf.Round(aux);

                Debug.Log("player: " + i);
                Debug.Log("resultado cadeiras: " + aux);
                Debug.Log("arredondado: " + cadeirasCamara[i]);

                if (i == (int)NetworkManager.Singleton.LocalClientId)
                {
                    hs.AtualizarCadeirasUI(cadeirasCamara[i]);
                }

                if (!NetworkManager.Singleton.IsServer) return;
                PlayerStats playerStats = PlayerStatsManager.Instance.GetPlayerStats(i);
                cadeiras += string.Concat(playerStats.NumCadeiras, "  cadeiras \n");


            }

            if (!NetworkManager.Singleton.IsServer) return;
            EleicaoText.Value = cadeiras;

        }
        public void CalculoEleicao()
        {

            ContaTotalEleitores();
            CalcularCadeiras();
            setCameraPosition(true);
            inEleicao = true;
        }
        public void setCameraPosition(bool _eleicao)
        {
            if (_eleicao)
            {
                Cameras[0].SetActive(false);
                Cameras[1].SetActive(true);
            }
            else
            {
                Cameras[0].SetActive(true);
                Cameras[1].SetActive(false);
            }

        }
        public void explicarEleicao()
        {
            uiCore.ExplicaStateUi.SetActive(true);
            uiCore.ExplicaStateTextTitulo.text = explicaTexto;
            uiCore.ExplicaStateTextCorpo.text = explicaTextoCorpo;
        }
        public void escondeExplicarEleicao()
        {
            uiCore.ExplicaStateUi.SetActive(false);
        }
        public void ColorirPeao()
        {
            // Debug.Log("coloriu");
            for (int i = 0; i < cadeirasCamara.Length; i++)
            {
                if (i == 0)
                {
                    valorPeao = cadeirasCamara[i] * i;
                }
                else
                {
                    valorPeao = cadeirasCamara[i - 1] * i;
                }

                PlayerStats[] allPlayerStats = FindObjectsOfType<PlayerStats>();
                foreach (PlayerStats stats in allPlayerStats)
                {
                    if (stats.playerID == i)
                    {
                        for (int j = 0; j < cadeirasCamara[i]; j++)
                        {

                            Material material = peosCamara[j + (int)valorPeao].GetComponent<MeshRenderer>().material;
                            material.SetColor("_BaseColor", stats.Cor);
                        }
                    }
                }
            }
        }
        private void OnEnable()
        {
            //jogadores conectados
            EleicaoText.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
            {
                if (newValue != "")
                {
                    UIeleicao.Instance.MostrarCadeiras(newValue.ToString());
                    ColorirPeao();
                    //Debug.Log(newValue.ToString());
                }
            };
            conectados.OnValueChanged += (int previousValue, int newValue) =>
            {
                if (newValue != 0)
                {
                    numConectados = newValue;
                    cadeirasInicial();
                }
            };
        }

    }
}
