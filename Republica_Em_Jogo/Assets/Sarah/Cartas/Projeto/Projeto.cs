using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Collections;
using Game.Territorio;
using Game.Player;
using Game.UI;
using Game;
using System;

//namespace Game.Territorio
//{
public class Projeto : NetworkBehaviour
{
    [Header("NetworkVariables")]
    private NetworkVariable<FixedString4096Bytes> projetoNetworkTexto = new NetworkVariable<FixedString4096Bytes>();
    private NetworkVariable<FixedString4096Bytes> zonaNetworkName = new NetworkVariable<FixedString4096Bytes>();
    private NetworkVariable<int> recompensaNetworkNum = new NetworkVariable<int>(-1);
    private NetworkVariable<int> idPlayer = new NetworkVariable<int>(-1);
    private NetworkVariable<int> votacaoRespostaFavor = new NetworkVariable<int>(0);
    private NetworkVariable<int> votacaoResposta = new NetworkVariable<int>(0);
    private NetworkVariable<int> numPlayers = new NetworkVariable<int>(-1);

    [Header("Referencias")]
    [SerializeField] private ProjetoObject projetoManager;
    private HudStatsJogador hs;
    private SetUpZona setUpZona;
    private ZonaTerritorial zt;
    private ControlePassarState cp;
    private Baralho baralho;
    [SerializeField] private UIEleitorCurrency uIEleitorCurrency;
    [SerializeField] private JogadorForaControl jogadorForaControl;

    [Header("Ui")]
    [SerializeField] private TMP_Text text_projetoCarta;
    [SerializeField] private TMP_Text text_avisoAprovacaoProjeto;
    public GameObject projetoUI;
    [SerializeField] private GameObject restoUI;
    [SerializeField] private GameObject infoZonas;
    [SerializeField] private GameObject bntsUi, btns2, fecharBtn;
    [SerializeField] private GameObject verMapaBtn;
    public GameObject verProjetoBtn;

    [Header("Variaveis")]
    private int numRecompensa, numRecompensaDefault, sim, quantVotos, numPlayer;
    private string recompensaText, zonaNameLocal, mostrarResposta, proposta;
    [HideInInspector]
    public int clienteLocal = -1;
    private bool projetoNaoAprovado = false;
    [HideInInspector]
    public bool playerInZona = false;
    [HideInInspector]
    public bool distribuicaoProjeto = false;
    [HideInInspector]
    public bool aprovado = false;
    [HideInInspector]
    public bool inVotacao = false;

    public event Action<string> ProjetoAprovado;

    //Client cashing
    private string clientDados;
    private string textoTotal = "";

    public TMP_Text Text_avisoAprovacaoProjeto => text_avisoAprovacaoProjeto;

    public void Awake()
    {
        setUpZona = GameObject.FindObjectOfType<SetUpZona>();
        zt = GameObject.FindObjectOfType<ZonaTerritorial>();
        hs = FindObjectOfType<HudStatsJogador>();
        cp = FindObjectOfType<ControlePassarState>();
        baralho = FindObjectOfType<Baralho>();
    }

    //********Seção funções RPC**************************************************
    //reseta valores para nova busca
    [ServerRpc(RequireOwnership = false)]
    public void DefaultValuesServerRpc()
    {
        recompensaNetworkNum.Value = -1;
        votacaoRespostaFavor.Value = 0;
        votacaoResposta.Value = 0;
        projetoNetworkTexto.Value = "";
        zonaNetworkName.Value = "";
        idPlayer.Value = -1;
    }

    //pede pro host avaliar a votação
    [ServerRpc(RequireOwnership = false)]
    public void UpdateVotacaoServerRpc(int valor, int _numcadeiras)
    {
        if (valor == 0)
        {
            votacaoRespostaFavor.Value += _numcadeiras;
            votacaoResposta.Value++;
        }
        if (valor == 1)
        {
            votacaoResposta.Value++;
        }

    }

    //host atualiza zona escolhida
    [ServerRpc(RequireOwnership = false)]
    public void UpdateZonaServerRpc(string clientDados)
    {
        zonaNetworkName.Value = clientDados;
    }

    //hosta atualiza a carta de projeto para os clientes
    [ServerRpc(RequireOwnership = false)]
    public void UpdateClientPositionServerRpc(string clientDados, int clientId, int num)
    {
        projetoNetworkTexto.Value = clientDados;
        recompensaNetworkNum.Value = num;
        idPlayer.Value = clientId;
        numPlayers.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
    }


    //********Seção networkvariables onChange*******************************************
    //verifica valores das variaves network se mudaram
    private void OnEnable()
    {
        //jogadores conectados
        numPlayers.OnValueChanged += (int previousValue, int newValue) =>
        {
            numPlayer = newValue;
        };

        //id jogador
        idPlayer.OnValueChanged += (int previousValue, int newValue) =>
        {
            if (newValue != -1)
            {

                //interface geral
                clienteLocal = newValue;
                projetoUI.SetActive(true);
                verProjetoBtn.SetActive(false);
                fecharBtn.SetActive(false);
                verMapaBtn.SetActive(true);
                //interface para quem está escolhendo zona
                bntsUi.SetActive(true);
                infoZonas.SetActive(true);
                // text_avisoAprovacaoProjeto.text = "Escolha uma zona:";

                //interface para quem está esperando zona ser escolhida
                if (newValue != (int)NetworkManager.Singleton.LocalClientId)
                {
                    infoZonas.SetActive(false);
                    bntsUi.SetActive(false);
                    text_avisoAprovacaoProjeto.text = "Aguardando zona ser escolhida.";
                    text_avisoAprovacaoProjeto.gameObject.SetActive(true);
                }
            }
        };
        //valor da recompensa
        recompensaNetworkNum.OnValueChanged += (int previousValue, int newValue) =>
        {
            if (newValue != -1)
            {
                projetoUI.SetActive(true);
                numRecompensa = newValue;
                numRecompensaDefault = numRecompensa;
            }
        };
        //texto do projeto
        projetoNetworkTexto.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
        {
            if (newValue != "")
            {
                projetoUI.SetActive(true);
                text_projetoCarta.text = newValue.ToString();
            }
        };
        //zona escolhida
        zonaNetworkName.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
        {
            zonaNameLocal = newValue.ToString();
            if (newValue != "")
            {

                //desativa zonas para ser escolhidas
                bntsUi.SetActive(false);

                //interface para quem escolheu zona e está esperando votação
                if (clienteLocal == (int)NetworkManager.Singleton.LocalClientId)
                {
                    infoZonas.SetActive(false);
                    //se tiver 7 cadeiras passa direto pra projeto aprovado
                    PlayerStats ps = hs.GetPlayerStats();
                    if (ps.NumCadeiras.Value >= EleicaoManager.Instance.minCadeirasVotacao)
                    {
                        projetoAprovado();

                    }
                    else
                    {
                        text_avisoAprovacaoProjeto.text = "ZONA ESCOLHIDA: \n\n " + newValue.ToString() + "\n \n" + "Aguardando votação... ";
                        text_avisoAprovacaoProjeto.gameObject.SetActive(true);
                        verMapaBtn.SetActive(false);
                        verProjetoBtn.SetActive(false);
                        UpdateVotacaoServerRpc(0, (int)ps.NumCadeiras.Value);
                        inVotacao = true;
                    }


                    //interface para quem está votando
                }
                else
                {
                    if (EleicaoManager.Instance.cadeirasCamara[clienteLocal] >= EleicaoManager.Instance.minCadeirasVotacao)
                    {
                        projetoAprovado();
                    }
                    else
                    {
                        if (jogadorForaControl.JogadorLocalRemovido)
                        {
                            VotacaoAFavorAutomatica();
                            text_avisoAprovacaoProjeto.SetText("Seu partido não está apto para votar.");
                            text_avisoAprovacaoProjeto.gameObject.SetActive(true);
                            fecharBtn.SetActive(true);
                        }
                        else
                        {
                            infoZonas.SetActive(true);
                            text_avisoAprovacaoProjeto.text = "ZONA ESCOLHIDA: \n\n " + newValue.ToString() + "\n \n" + "VOTE: ";
                            text_avisoAprovacaoProjeto.gameObject.SetActive(true);
                            btns2.SetActive(true);
                            inVotacao = true;
                        }

                    }


                }

            }

        };

        //votacao a favor
        votacaoRespostaFavor.OnValueChanged += (int previousValue, int newValue) =>
        {
            sim = newValue;
        };

        //votacao contra
        votacaoResposta.OnValueChanged += (int previousValue, int newValue) =>
        {
            quantVotos = newValue;
        };
    }

    //********Seção update************************************************************
    public void Update()
    {
        if (inVotacao == true)
        {
            if (numPlayer > 1)
            {
                //se todos votaram
                if (quantVotos >= numPlayer)
                {

                    //desativa botão de votação
                    btns2.SetActive(false);
                    //ativa botao de fechar interface
                    fecharBtn.SetActive(true);
                    infoZonas.SetActive(false);
                    //se teve mais sim, foi aprovado
                    if (sim >= EleicaoManager.Instance.minCadeirasVotacao)
                    {
                        projetoAprovado();
                    }
                    else
                    {
                        //se teve mais não ou empate, foi reprovado
                        text_avisoAprovacaoProjeto.text = "PROJETO NÃO APROVADO";
                        verMapaBtn.SetActive(false);
                        inVotacao = false;
                        projetoNaoAprovado = true;
                    }
                }
            }
        }
    }

    //********Seção funções player******************************************************
    //sortea os valores do projeto
    public void sortearProjeto()
    {
        defaultValues();
        proposta = projetoManager.proposta[UnityEngine.Random.Range(0, projetoManager.proposta.Length)];
        numRecompensa = projetoManager.numRecompensa[UnityEngine.Random.Range(0, projetoManager.numRecompensa.Length)];
        recompensaText = projetoManager.recompensaText;
        textoTotal = proposta + "\n \n" + recompensaText + "" + numRecompensa.ToString();
        atualizarProjeto(textoTotal);
        text_avisoAprovacaoProjeto.gameObject.SetActive(false);

    }

    //atualiza a carta de projeto ou pede pro host fazer isso
    public void atualizarProjeto(string textoTotal2)
    {
        if (NetworkManager.Singleton.IsClient)
        {
            int id = (int)NetworkManager.Singleton.LocalClientId;
            UpdateClientPositionServerRpc(textoTotal2, id, numRecompensa);
            //Debug.Log("cliente");
        }
        if (NetworkManager.Singleton.IsServer)
        {
            projetoNetworkTexto.Value = textoTotal2;
            idPlayer.Value = (int)NetworkManager.Singleton.LocalClientId;
            recompensaNetworkNum.Value = numRecompensa;
            numPlayers.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
            //Debug.Log("server");

        }
    }
    private IEnumerator EsperaEVai1(float s)
    {
        yield return new WaitForSeconds(s);
        if (playerInZona == true)
        {
            distribuicaoProjeto = true;
            hs.playerRecebeEleitor = true;
            setUpZona.eleitoresZona(numRecompensa, zonaNameLocal);
            hs.updateRecursoCartaUI(numRecompensaDefault);
            playerInZona = false;
        }
        zonaNameLocal = "";
        clienteLocal = -1;
        numRecompensa = -1;
    }
    //chamado apos projeto ser aprovado
    public void eleitoresZonaFinal()
    {
        //adiciona eleitores aos jogadores que tem bairros da zona
        //verifica se player tem bairro na zona escolhida
        //dá carta de recurso para jogadores que possuem bairros na zona
        //reseta algumas variáveis

        StartCoroutine(EsperaEVai1(0.1f));

    }

    //ao apertar botao de fechar interface   
    public void fechar()
    {
        //desatuva interface
        //se o projeto n foi aprovado e eu sou o client id eu passo de state
        fecharBtn.SetActive(false);
        projetoUI.SetActive(false);
        if (projetoNaoAprovado)
        {
            if (clienteLocal == (int)NetworkManager.Singleton.LocalClientId)
            {
                CoreLoopStateHandler.Instance.NextStateServerRpc();
            }
            projetoNaoAprovado = false;
            zonaNameLocal = "";
            clienteLocal = -1;
            numRecompensa = -1;
        }

        if (aprovado == true)
        {
            eleitoresZonaFinal();
            aprovado = false;
        }

        if (playerInZona) uIEleitorCurrency.PlayEnterAnim();


    }

    //reseta variaveis oou pede pro hosta fazer isso
    public void defaultValues()
    {
        sim = 0;
        quantVotos = 0;
        if (NetworkManager.Singleton.IsServer)
        {
            recompensaNetworkNum.Value = -1;
            votacaoRespostaFavor.Value = 0;
            votacaoResposta.Value = 0;
            projetoNetworkTexto.Value = "";
            zonaNetworkName.Value = "";
            idPlayer.Value = -1;
        }
        if (NetworkManager.Singleton.IsClient)
        {
            DefaultValuesServerRpc();
        }
    }

    //funcao disparada ao apertar no nome de uma zona
    public void escolherZona(string zonaName)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            zonaNetworkName.Value = zonaName;
        }
        if (NetworkManager.Singleton.IsClient)
        {
            UpdateZonaServerRpc(zonaName);
        }

    }

    private void VotacaoAFavorAutomatica() => votacao(0);


    //funcao disparada ao apertar no botao de a favor ou contra a votação
    public void votacao(int resposta)
    {

        if (NetworkManager.Singleton.IsClient)
        {
            PlayerStats ps = hs.GetPlayerStats();
            UpdateVotacaoServerRpc(resposta, (int)ps.NumCadeiras.Value);
        }
        //texto interface recebe valores e botoees somem
        if (resposta == 0) mostrarResposta = "a favor";
        if (resposta == 1) mostrarResposta = "contra";
        btns2.SetActive(false);
        if (numPlayer > 2)
        {
            text_avisoAprovacaoProjeto.text = "Seu partido votou: \n\n" + mostrarResposta + "\n\n" + "Aguardando outros partidos...";
            text_avisoAprovacaoProjeto.gameObject.SetActive(true);
            verMapaBtn.SetActive(false);

        }
    }

    //funcao ao projeto ser aprovado
    public void projetoAprovado()
    {
        // HabilitarBairrosPlayerLocal(true);
        fecharBtn.SetActive(true);
        text_avisoAprovacaoProjeto.text = "PROJETO APROVADO \n Zona escolhida: \n\n" + zonaNameLocal + "\n\n" + "RECOMPENSA \n" + numRecompensaDefault + " carta(s) e " + numRecompensaDefault + " eleitor(es)";
        verMapaBtn.SetActive(false);
        text_avisoAprovacaoProjeto.gameObject.SetActive(true);

        aprovado = true;
        ProjetoAprovado?.Invoke(zonaNameLocal);
        setUpZona.playerZona(NetworkManager.Singleton.LocalClientId, zonaNameLocal);
        if (playerInZona == true)
        {
            cp.distribuicaoProjeto = true;
            cp.AumentaValServerRpc();
        }
        inVotacao = false;
    }



}

