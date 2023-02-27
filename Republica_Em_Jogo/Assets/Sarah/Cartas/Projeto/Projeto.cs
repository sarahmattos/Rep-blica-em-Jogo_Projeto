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

    [Header("Ui")]
    [SerializeField] private TMP_Text text_projetoCarta;
    [SerializeField] private TMP_Text text_avisoProjeto;
    public GameObject projetoUI;
    [SerializeField] private GameObject restoUI;
    [SerializeField] private GameObject bntsUi, btns2, fecharBtn;
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

    //Client cashing
    private string clientDados;
    private string textoTotal = "";


    public void Awake()
    {
        setUpZona = GameObject.FindObjectOfType<SetUpZona>();
        zt = GameObject.FindObjectOfType<ZonaTerritorial>();
        hs = FindObjectOfType<HudStatsJogador>();
        cp = FindObjectOfType<ControlePassarState>();
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

                //interface para quem está escolhendo zona
                bntsUi.SetActive(true);
                text_avisoProjeto.text = "Escolha uma zona:";

                //interface para quem está esperando zona ser escolhida
                if (newValue != (int)NetworkManager.Singleton.LocalClientId)
                {
                    bntsUi.SetActive(false);
                    text_avisoProjeto.text = "Aguardando zona ser escolhida";
                }
            }
        };
        //valor da recompensa
        recompensaNetworkNum.OnValueChanged += (int previousValue, int newValue) =>
        {
            if (newValue != -1)
            {
                numRecompensa = newValue;
                numRecompensaDefault = numRecompensa;
            }
        };
        //texto do projeto
        projetoNetworkTexto.OnValueChanged += (FixedString4096Bytes previousValue, FixedString4096Bytes newValue) =>
        {
            if (newValue != "")
            {
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
                    //se tiver 7 cadeiras passa direto pra projeto aprovado
                    PlayerStats ps = hs.GetPlayerStats();
                    if (ps.numCadeiras >= EleicaoManager.Instance.minCadeirasVotacao)
                    {
                        projetoAprovado();

                    }
                    else
                    {
                        text_avisoProjeto.text = "\n" + "\n" + "\n" + "Zona: " + newValue.ToString() + "\n" + "Não possui cadeiras suficientes para aprovar sozinho(a)" + "\n" + "Aguardando votação... ";
                        UpdateVotacaoServerRpc(0, (int)ps.numCadeiras);
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
                        text_avisoProjeto.text = "\n" + "\n" + "Zona escolhida: " + newValue.ToString() + "\n" + "Vote: ";
                        btns2.SetActive(true);
                        inVotacao = true;
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

                    //se teve mais sim, foi aprovado
                    if (sim > EleicaoManager.Instance.minCadeirasVotacao)
                    {
                        projetoAprovado();
                    }
                    else
                    {
                        //se teve mais não ou empate, foi reprovado
                        text_avisoProjeto.text = "\n" + "\n" + "\n" + "PROJETO NÃO APROVADO";
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
        proposta = projetoManager.proposta[Random.Range(0, projetoManager.proposta.Length)];
        numRecompensa = projetoManager.numRecompensa[Random.Range(0, projetoManager.numRecompensa.Length)];
        recompensaText = projetoManager.recompensaText;
        textoTotal = proposta + "\n" + "\n" + recompensaText + "" + numRecompensa.ToString();
        atualizarProjeto(textoTotal);

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

    //chamado apos projeto ser aprovado
    public void eleitoresZonaFinal()
    {
        //adiciona eleitores aos jogadores que tem bairros da zona
        //verifica se player tem bairro na zona escolhida
        //dá carta de recurso para jogadores que possuem bairros na zona
        //reseta algumas variáveis
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

    //funcao disparada ao apertar no botao de a favor ou contra a votação
    public void votacao(int resposta)
    {

        if (NetworkManager.Singleton.IsClient)
        {
            PlayerStats ps = hs.GetPlayerStats();
            UpdateVotacaoServerRpc(resposta, (int)ps.numCadeiras);
        }
        //texto interface recebe valores e botoees somem
        if (resposta == 0) mostrarResposta = "a favor";
        if (resposta == 1) mostrarResposta = "contra";
        btns2.SetActive(false);
        if (numPlayer > 2)
        {
            text_avisoProjeto.text = "\n" + "\n" + "\n" + "Seu partido votou " + mostrarResposta + "\n" + "Aguardando outros partidos...";
        }
    }

    //funcao ao projeto ser aprovado
    public void projetoAprovado()
    {
        HabilitarBairrosPlayerLocal(true);
        fecharBtn.SetActive(true);
        text_avisoProjeto.text = "\n" + "\n" + "\n" + "PROJETO APROVADO na zona " + zonaNameLocal + "\n" + "Recompensa: " + numRecompensaDefault + " carta(s) e " + numRecompensaDefault + " eleitor(es)";
        aprovado = true;
        setUpZona.playerZona(NetworkManager.Singleton.LocalClientId, zonaNameLocal);
        if (playerInZona == true)
        {
            cp.distribuicaoProjeto = true;
            cp.AumentaValServerRpc();
        }
        inVotacao = false;
    }

    private void HabilitarBairrosPlayerLocal(bool value)
    {
        List<Bairro> bairros = PlayerStatsManager.Instance.GetLocalPlayerStats().BairrosInControl;
        foreach (Bairro bairro in bairros)
        {
            bairro.Interagivel.MudarHabilitado(value);
        }
    }

    
}

