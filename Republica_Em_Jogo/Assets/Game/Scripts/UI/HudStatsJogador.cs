using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Player;
using Game.Territorio;
using System;
using Game.Tools;

namespace Game.UI
{
    public class HudStatsJogador : NetworkSingleton<HudStatsJogador>
    {
        [Header("Ui")]
        public GameObject button; //botao para testar
        // [SerializeField] private Image iconJogador;
        [SerializeField] private TMP_Text text_nomeJogador;
        [SerializeField] private TMP_Text text_eleitores;
        [SerializeField] private TMP_Text text_objetivo;
        [SerializeField] private TMP_Text text_distribuaEleitor;
        [SerializeField] private TMP_Text text_distribuaEleitorFinal;
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private TMP_Text text_saudeCarta;
        [SerializeField] private TMP_Text text_eduCarta;
        [SerializeField] private TMP_Text text_bairros;
        // [SerializeField] private TMP_Text text_eleitoresNovos;
        [SerializeField] private UIVoterCurrency uIVoterCurrency;
        [SerializeField] private TMP_Text text_cadeiras;
        [SerializeField] public TMP_Text text_naotemeleitorpraretirar;
        [SerializeField] private GameObject acaboudistribuicaoUi;
        [SerializeField] private GameObject acaboudistribuicaoUi2;
        [SerializeField] private GameObject distribuaEleitorUi;
        [SerializeField] private GameObject btnsAux;
        [SerializeField] private GameObject corUi;
        [SerializeField] private Transform corUiPai;

        [Header("Referencias")]
        [SerializeField] private State state;
        private Projeto projeto;
        private EleicaoManager eleicaoManager;
        private SetUpZona setUpZona;
        int eleitoresAdicionais;


        [Header("Variaveis")]
        public int eduQuant;
        public int saudeQuant;
        public float eleitoresNovosAtual;
        public List<int> ordemId;
        [SerializeField] List<Color> cor;
        [SerializeField] List<GameObject> objetosCores;
        private string textToDisplayEleitores => string.Concat("", playerStats.EleitoresTotais);
        [HideInInspector]
        public bool playerRecebeEleitor = true;
        [HideInInspector]
        public bool playerDiminuiEleitor = false;
        [HideInInspector]
        public bool distribuicaoGeral = false;
        [HideInInspector]
        public bool distribuicaoInicial = false;
        private int aux;
        public event Action eleitoresNovosDeProjeto;
        private State inicalizacao => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZACAO);
        private State desenvolvimentoState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.DESENVOLVIMENTO);

        private void Start()
        {
            // inicalizacao.Saida += testeCor;
            TurnManager.Instance.turnoMuda += respostaVisualOrdem;
            desenvolvimentoState.Entrada += () => respostaVisualOrdem(-1, TurnManager.Instance.PlayerAtual);

        }

        public override void OnDestroy()
        {
            TurnManager.Instance.vezDoPlayerLocal -= (bool value) => { button.SetActive(value); };
            // inicalizacao.Saida -= testeCor;
            TurnManager.Instance.turnoMuda -= respostaVisualOrdem;
            desenvolvimentoState.Entrada -= () => respostaVisualOrdem(-1, TurnManager.Instance.PlayerAtual);


            base.OnDestroy();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                BntsAuxiliares();
            }
        }

        private List<int> getOrdemPlayersID()
        {
            List<int> aux = new List<int>();
            foreach (int id in TurnManager.Instance.ordemPlayersID)
            {
                aux.Add(id);
            }
            return aux;
        }


        public void testeCor()
        {
            ordemId = new List<int>();
            ordemId = getOrdemPlayersID();
            Debug.Log("testeColor");
            Debug.Log(ordemId.Count);
            if (aux == 0)
            {
                for (int i = 0; i < ordemId.Count; i++)
                {
                    PlayerStats[] allPlayerStats = FindObjectsOfType<PlayerStats>();
                    foreach (PlayerStats stats in allPlayerStats)
                    {
                        if (ordemId[i] == stats.playerID)
                        {
                            //cor.Add(stats.Cor);
                            // objetosCores.Add(InstantiateManager.Instance.InstanciarUi(stats.Cor, stats.playerID));
                        }
                    }

                }
            }


            aux++;

            // Sprite sprite = _go.GetComponent<Sprite>();
            //sprite.image. .cor=cor[0];
            //_go.image.cor=cor[0];
        }
        public void respostaVisualOrdem(int previousPlayer, int nextPlayer)
        {
            // if (ordemId.Count < 0) return;

            // Rect _previousPlayerRect = objetosCores[previousPlayer].GetComponent<RectTransform>().rect;
            // _previousPlayerRect.width = objetosCores[previousPlayer].GetComponent<RectTransform>().parent.gameObject.GetComponent<RectTransform>().rect.width / 2;

            // Rect _nextPlayerRect = objetosCores[nextPlayer].GetComponent<RectTransform>().rect;
            // _nextPlayerRect.width = objetosCores[nextPlayer].GetComponent<RectTransform>().parent.gameObject.GetComponent<RectTransform>().rect.width - 5;


        }
        public override void OnNetworkSpawn()
        {
            state = GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZACAO);
            GameStateHandler.Instance.StateMachineController.GetState((int)GameState.GAMEPLAY_SCENE_LOAD).Saida += FindingLocalPlayerStats;
            projeto = FindObjectOfType<Projeto>();
            setUpZona = GameObject.FindObjectOfType<SetUpZona>();
            eleicaoManager = FindObjectOfType<EleicaoManager>();
            GameStateHandler.Instance.StateMachineController.GetState((int)GameState.DESENVOLVIMENTO).Entrada += AtualizarPlayerStatsBairro;
        }

        public override void OnNetworkDespawn()
        {
            GameStateHandler.Instance.StateMachineController.GetState((int)GameState.GAMEPLAY_SCENE_LOAD).Saida -= FindingLocalPlayerStats;
            GameStateHandler.Instance.StateMachineController.GetState((int)GameState.DESENVOLVIMENTO).Entrada += AtualizarPlayerStatsBairro;

        }

        private void FindingLocalPlayerStats()
        {
            PlayerStats[] allPlayerStats = FindObjectsOfType<PlayerStats>();
            foreach (PlayerStats stats in allPlayerStats)
            {
                if (stats.IsLocalPlayer)
                {
                    playerStats = stats;
                    InitializeHudStats();
                }
            }
        }

        private void InitializeHudStats()
        {
            // iconJogador.color = playerStats.Cor;
            text_nomeJogador.SetText(playerStats.Nome);
            text_eleitores.SetText(textToDisplayEleitores);
            text_objetivo.SetText(playerStats.ObjetivoCarta);

        }

        //distribui carta de recurso (chamada pela classe projeto)
        public void updateRecursoCartaUI(int quantidade)
        {
            if (quantidade > -1)
            {

                //chama a funcao que randomiza
                playerStats.recursoDistribuicao(quantidade);

                //muda a interface
                text_saudeCarta.SetText(playerStats.numSaude.ToString());//"Saúde: " + 
                text_eduCarta.SetText( playerStats.numEducacao.ToString());//"Edu: " +

                //pega os valores da classe player stats, mais tarde é usado na troca
                atualizarRecursoAntesTroca();
            }
        }
        public void atualizarRecursoAntesTroca()
        {
            saudeQuant = playerStats.numSaude;
            eduQuant = playerStats.numEducacao;
        }
        // chamada dps de troca de cartas pelo script: RecursosCartaManager
        public void atualizarRecursoAposTroca()
        {

            //valores do playerstats recebem o valor de volta
            playerStats.numSaude = saudeQuant;
            playerStats.numEducacao = eduQuant;

            //interface muda
            text_saudeCarta.SetText(playerStats.numSaude.ToString());//"Saúde: " +
            text_eduCarta.SetText(playerStats.numEducacao.ToString());//"Edu: " + 
        }

        //a distribuicao inicial dos bairros atualiza o valor de bairros e eleitores totais
        public void AtualizarPlayerStatsBairro()
        {
            // setUpZona.ProcurarBairrosInZona();
            AtualizaEleitores();
            AtualizaBairros();

        }

        //atualiza texto eleitores
        public void AtualizaEleitores()
        {
            playerStats.ContaEleitoresInBairros();
            text_eleitores.SetText(textToDisplayEleitores);
        }

        //atualiza texto bairros
        public void AtualizaBairros()
        {
            // playerStats.ContaBairros();
            text_bairros.SetText(playerStats.BairrosInControl.Count.ToString());//"Bairros: " + 
        }

        //quando o jogador distribui seus eleitores
        public void contagemEleitores()
        {
            playerStats.eleitoresNovos--;
            //text_eleitoresNovos.SetText(string.Concat("+",playerStats.eleitoresNovos.ToString()));
            if (playerDiminuiEleitor)
            {
                uIVoterCurrency.ShowNegativeNovosEleitores((int)playerStats.eleitoresNovos);
            }
            else
            {
                uIVoterCurrency.ShowPositiveNovosEleitores((int)playerStats.eleitoresNovos);

            }

        }

        public void AtualizaUIAposDistribuicao()
        {
            distribuaEleitorUi.SetActive(false);
            distribuicaoGeral = false;
            if (playerDiminuiEleitor == true)
            {
                acaboudistribuicaoUi2.SetActive(true);
                playerDiminuiEleitor = false;

            }
            else
            {
                acaboudistribuicaoUi.SetActive(true);
            }

            if (projeto.distribuicaoProjeto == true)
            {
                projeto.distribuicaoProjeto = false;
                playerRecebeEleitor = true;
                setUpZona.ChamarReseteBairroNaZona();

            }
        }

        //para o bairro acessar quantos eleitores novos podem ser distribuidos
        public void valorEleitorNovo()
        {
            eleitoresNovosAtual = playerStats.eleitoresNovos;
        }

        //botao chama funcao de distribuicao de eleitor no inicio das rodadas
        public void ChamatPlayerInicioRodada()
        {
            checaZonasInteiras();
            eleitoresAdicionais = 0;
            if (distribuicaoInicial == true)
            {
                //se for state do inicio
                //tem zona inteira ganha adicional
                for (int i = 0; i < setUpZona.tenhoZona.Count; i++)
                {
                    eleitoresAdicionais += setUpZona.tenhoZona[i].eleitoresAdicionais;
                    Debug.Log("voce possui " + setUpZona.tenhoZona[i].eleitoresAdicionais + " eleitore(s) adicionais por conquistar a zona " + setUpZona.tenhoZona[i].Nome + " inteira!");
                }
                playerStats.inicioRodada(eleitoresAdicionais);
                distribuaEleitorUi.SetActive(true);
                distribuicaoGeral = true;
                text_distribuaEleitor.SetText("Distribua seus eleitores");
                //text_eleitoresNovos.SetText(string.Concat("+",playerStats.eleitoresNovos.ToString()));
                uIVoterCurrency.ShowPositiveNovosEleitores((int)playerStats.eleitoresNovos);
                eleitoresNovosDeProjeto?.Invoke();

            }

        }

        public void ValorEleitoresNovos(int valor)
        {
            if (playerRecebeEleitor == true)
            {
                playerStats.eleitoresNovos = valor;
                playerRecebeEleitor = false;
                //
                distribuaEleitorUi.SetActive(true);
                distribuicaoGeral = true;
                if (playerDiminuiEleitor == true)
                {
                    text_distribuaEleitor.SetText("Retire seus eleitores");
                    uIVoterCurrency.ShowNegativeNovosEleitores((int)playerStats.eleitoresNovos);

                }
                else
                {
                    text_distribuaEleitor.SetText("Distribua seus eleitores");
                    uIVoterCurrency.ShowPositiveNovosEleitores((int)playerStats.eleitoresNovos);

                }
                //text_eleitoresNovos.SetText(string.Concat("+",playerStats.eleitoresNovos.ToString()));
                // uIVoterCurrency.ShowPositiveNovosEleitores((int)playerStats.eleitoresNovos);
                eleitoresNovosDeProjeto?.Invoke();
            }

        }
        public PlayerStats GetPlayerStats()
        {
            return playerStats;
        }
        public void cadeirasUi(float valor)
        {
            playerStats.numCadeiras = valor;
            text_cadeiras.SetText(playerStats.numCadeiras.ToString());//"Cadeiras: " + "\n" + 
        }
        public void BntsAuxiliares()
        {
            btnsAux.SetActive(!btnsAux.activeSelf);
        }
        public void checaZonasInteiras()
        {
            setUpZona.PlayerTemZonaInteira(playerStats.playerID);
        }

    }

}
