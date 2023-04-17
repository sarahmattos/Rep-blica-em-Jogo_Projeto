using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Game.UI;
using Game.Player;

namespace Game.Territorio
{

    public class Bairro : NetworkBehaviour
    {

        [SerializeField] private string nome;
        private NetworkVariable<int> playerIDNoControl = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public string Nome { get => nome; }
        public NetworkVariable<int> PlayerIDNoControl => playerIDNoControl;
        [SerializeField] private Bairro[] vizinhos;
        public Bairro[] Vizinhos => vizinhos;
        public event Action playerControlMuda;
        public event Action<Bairro, int> bairroPlayerLocalForaControl;
        public event Action<Bairro, int> bairroPlayerLocalNoControl;
        public NetworkVariable<bool> OnFocus = new NetworkVariable<bool>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        public bool playerInControl = false;
        public bool bairroNaZonaEscolhida = false;
        private Interagivel interagivel;
        [SerializeField] private SetUpBairro setUpBairro;
        public SetUpBairro SetUpBairro { get => setUpBairro; }
        public Interagivel Interagivel => interagivel;
        private HudStatsJogador hs;
        public Educaçao edu;
        public Saúde saude;
        private Recursos recurso;

        private void Awake()
        {
            interagivel = GetComponentInChildren<Interagivel>();
            setUpBairro = GetComponentInChildren<SetUpBairro>();
            edu = GetComponentInChildren<Educaçao>();
            saude = GetComponentInChildren<Saúde>();
            hs = FindObjectOfType<HudStatsJogador>();
            recurso = GetComponentInChildren<Recursos>();
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeSelectBairroServerRpc(bool value)
        {
            OnFocus.Value = value;
        }

        public void MudaValorEleitor(int valor)
        {
            setUpBairro.Eleitores.AcrescentaEleitorServerRpc(valor);
            //hs.AtualizarPlayerStatsBairro();
        }


        private void OnEnable()
        {
            playerIDNoControl.OnValueChanged += onPlayerControlMuda;

        }

        private void OnDisable()
        {
            playerIDNoControl.OnValueChanged -= onPlayerControlMuda;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetPlayerControlServerRpc(int playerID)
        {
            playerIDNoControl.Value = playerID;
            // setUpBairro.Eleitores?.MudaValorEleitores(1);
        }


        private void onPlayerControlMuda(int previousValue, int newValue)
        {
            bairroPlayerLocalForaControl?.Invoke(this, previousValue);
            bairroPlayerLocalNoControl?.Invoke(this, newValue);

        }


        //verifica se bairro pertence ao jogador
        public bool VerificaControl()
        {
            if (playerIDNoControl.Value == (int)NetworkManager.Singleton.LocalClientId)
            {
                playerInControl = true;
            }
            else
            {
                playerInControl = false;
            }
            return playerInControl;
        }

        //chamado pelo "MostrarNomeBairro" qnd clicado em um bairro
        public void EscolherBairroEleitor()
        {
            if (VerificaControl())
            {
                //retirar
                if (hs.playerDiminuiEleitor == true)
                {
                    if (setUpBairro.Eleitores.contaEleitores > 1)
                    {
                        if (NetworkManager.Singleton.IsClient) MudaValorEleitor(-1);
                        //dimiui eleitor novo e aumenta eleito total
                        hs.contagemEleitores();
                        //recupera quantos eleitores novos
                        hs.valorEleitorNovo();
                        hs.text_naotemeleitorpraretirar.text = "Retirada de eleitores finalizada!";
                        if (hs.eleitoresNovosAtual < 1) hs.AtualizaUIAposDistribuicao();

                        //teste
                        PlayerStats ps = hs.GetPlayerStats();
                        if (ps.EleitoresTotais <= ps.BairrosInControl.Count)
                        {
                            hs.text_naotemeleitorpraretirar.text = "Não possui eleitores suficientes para retirada!";
                            hs.AtualizaUIAposDistribuicao();
                        }
                    }
                }
                else
                {
                    //colocar
                    if (setUpBairro.Eleitores.contaEleitores > 0)
                    {
                        if (NetworkManager.Singleton.IsClient) MudaValorEleitor(1);
                        hs.contagemEleitores();
                        hs.valorEleitorNovo();
                        if (hs.eleitoresNovosAtual < 1) hs.AtualizaUIAposDistribuicao();
                        //hs.AtualizarPlayerStatsBairro();
                    }
                }

            }
        }
        public int checaNumerodeEducacao()
        {
            return recurso.educacao;
        }
        public int checaNumerodeSaude()
        {
            return recurso.saude;
        }

    }

}
