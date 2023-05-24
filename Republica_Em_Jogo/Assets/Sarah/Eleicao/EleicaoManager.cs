using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Unity.Netcode;
using Game.Territorio;
using Game.Tools;
using Unity.Collections;
using Game.UI;
using System;

namespace Game
{
    public class EleicaoManager : NetworkBehaviour
    {

        // private NetworkVariable<FixedString4096Bytes> EleicaoText = new NetworkVariable<FixedString4096Bytes>();
        private NetworkVariable<int> conectados = new NetworkVariable<int>();
        public HudStatsJogador hs;
        public static EleicaoManager Instance;
        public int somaEleitores;
        public int[] cadeirasCamara;
        public int minCadeirasVotacao;
        [SerializeField] private List<Bairro> todosBairros;
        private ZonaTerritorial[] zonasTerritoriais;
        public int numConectados;
        public int[] eleitoresPlayers;
        private SetUpZona setUpZona;
        private string textoUiCadeiras;
        float valorPeao;
        [SerializeField] GameObject[] peosCamara;
        [SerializeField] GameObject[] Cameras;
        public bool inEleicao = false;
        ///[SerializeField] Transform posicaoCameraEleicao;
        //public Vector3 posicaoAntiga;
        public CalculoCadeirasEleicao CalculoCadeiras { get; private set; } = new CalculoCadeirasEleicao();

        public string explicaTexto, explicaTextoCorpo;
        private UICoreLoop uiCore;
        void Start()
        {
            zonasTerritoriais = FindObjectsOfType<ZonaTerritorial>();
            setUpZona = GameObject.FindObjectOfType<SetUpZona>();
            Instance = this;
            minCadeirasVotacao = (int)GameDataconfig.Instance.CadeirasTotal / 2 + 1;
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
                cadeirasCamara[i] = GameDataconfig.Instance.CadeirasTotal / numConectados;
                if (i == (int)NetworkManager.Singleton.LocalClientId)
                {
                    hs.AtualizarCadeirasUI((int)cadeirasCamara[i]);
                }
            }
            // ColorirPeao();
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
            PlayerStats playerStatsLocal = hs.GetPlayerStats();
            int novasCadeiras = CalculoCadeiras.Calcular(playerStatsLocal);

            hs.AtualizarCadeirasUI(novasCadeiras);


            //TODO: Não mexi por precausão. Não sei até onde se estende o acoplamento destas propriedades e métodos.
            eleitoresPlayers = new int[numConectados];
            cadeirasCamara = new int[numConectados];
            setUpZona.SepararBairrosPorPlayer(eleitoresPlayers, numConectados);

            for (int i = 0; i < eleitoresPlayers.Length; i++)
            {
                int aux = eleitoresPlayers[i] * GameDataconfig.Instance.CadeirasTotal / somaEleitores;
                cadeirasCamara[i] = (int)Mathf.Round(aux);
                //if (i == (int)NetworkManager.Singleton.LocalClientId)
                //{
                //    hs.AtualizarCadeirasUI(cadeirasCamara[i]);
                //}
                //PlayerStats playerStats = PlayerStatsManager.Instance.GetPlayerStats(i);
                //cadeiras += string.Concat(playerStats.NumCadeiras, "  cadeiras \n");
            }

            // EleicaoText.Value = " _ ";

        }


        //Regras aqui: https://prmdcp2.wixsite.com/mppeb/blank-ghuf9
        // public int RecontagemDeCadeiras(PlayerStats playerStats)
        // {
        //     double quocienteEleitoral = playerStats.GetEleitoresTotais() / cadeirasTotais;
        //     int cadeirasArredondado = (int)Math.Round(quocienteEleitoral * GetTotalEleitoresInTerritorio());
        //     return cadeirasArredondado;
        // }



        private int GetTotalEleitoresInTerritorio()
        {
            int eleitoresInTerritorio = 0;
            foreach (Bairro bairro in setUpZona.AllBairros)
            {
                eleitoresInTerritorio += bairro.SetUpBairro.Eleitores.contaEleitores;
            }
            return eleitoresInTerritorio;
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



        private void OnEnable()
        {
            //jogadores conectados
            // EleicaoText.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
            // {
            //     ColorirPeao();

            // };

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
