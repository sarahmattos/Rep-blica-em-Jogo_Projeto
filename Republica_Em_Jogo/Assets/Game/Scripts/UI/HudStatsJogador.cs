using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Player;
using Game.Territorio;
using System;
using Game.Tools;
using Unity.Collections;
using System.Collections;

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
        [SerializeField] private UIEleitorCurrency uIVoterCurrency;
        [SerializeField] private TMP_Text text_cadeiras;
        [SerializeField] public TMP_Text text_naotemeleitorpraretirar;
        [SerializeField] private GameObject acaboudistribuicaoUi;
        [SerializeField] private GameObject acaboudistribuicaoUi2;
        [SerializeField] private GameObject distribuaEleitorUi;
        [SerializeField] private GameObject btnsAux;
        [SerializeField] private GameObject corUi;
        [SerializeField] private GameObject[] olhoEsconde;
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
        private string textToDisplayEleitores => string.Concat("", playerStats.GetEleitoresTotais());
        [HideInInspector]
        public bool playerRecebeEleitor = true;
        [HideInInspector]
        public bool playerDiminuiEleitor = false;
        [HideInInspector]
        public bool distribuicaoGeral = false;
        [HideInInspector]
        public bool distribuicaoInicial = false;
        private bool ativa = true;
        private int aux;
        public event Action eleitoresNovosDeProjeto;
        private State inicalizacao => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZAÇÃO);
        private State DesenvolvimentoState => GameStateHandler.Instance.GetState(GameState.DESENVOLVIMENTO);
        private State GameplayerLoadScene => GameStateHandler.Instance.GetState(GameState.GAMEPLAY_SCENE_LOAD);
        private State EleicaoState => GameStateHandler.Instance.GetState(GameState.ELEIÇÕES);


        private void Start()
        {
            // inicalizacao.Saida += testeCor;
            TurnManager.Instance.turnoMuda += respostaVisualOrdem;
            DesenvolvimentoState.Entrada += () => respostaVisualOrdem(-1, TurnManager.Instance.PlayerAtualID);
            inicalizacao.Saida += UpdateUICadeiras;
            EleicaoState.Saida += () => text_cadeiras.SetText(string.Concat(playerStats.NumCadeiras.Value, "/", GameDataconfig.Instance.CadeirasTotal));




        }

        public override void OnDestroy()
        {
            TurnManager.Instance.vezDoPlayerLocal -= (bool value) => { button.SetActive(value); };
            // inicalizacao.Saida -= testeCor;
            TurnManager.Instance.turnoMuda -= respostaVisualOrdem;
            DesenvolvimentoState.Entrada -= () => respostaVisualOrdem(-1, TurnManager.Instance.PlayerAtualID);
            inicalizacao.Saida -= UpdateUICadeiras;
            EleicaoState.Saida -= () => text_cadeiras.SetText(string.Concat(playerStats.NumCadeiras.Value, "/", GameDataconfig.Instance.CadeirasTotal));



            base.OnDestroy();
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

        private void UpdateUICadeiras()
        {
            playerStats.NumCadeiras.OnValueChanged += (int _, int newValue) =>
            {
                text_cadeiras.SetText(string.Concat(newValue, "/", GameDataconfig.Instance.CadeirasTotal));
            };
            text_cadeiras.SetText(string.Concat(playerStats.NumCadeiras.Value, "/", GameDataconfig.Instance.CadeirasTotal));


        }


        public void testeCor()
        {
            ordemId = new List<int>();
            ordemId = getOrdemPlayersID();
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
            projeto = FindObjectOfType<Projeto>();
            setUpZona = GameObject.FindObjectOfType<SetUpZona>();
            eleicaoManager = FindObjectOfType<EleicaoManager>();
            state = GameStateHandler.Instance.GetState(GameState.INICIALIZAÇÃO);

            inicalizacao.Saida -= UpdateUICadeiras;
            GameplayerLoadScene.Saida += ConfigureHudStats;
            DesenvolvimentoState.Entrada += AtualizarPlayerStatsBairro;
        }

        public override void OnNetworkDespawn()
        {
            inicalizacao.Saida -= UpdateUICadeiras;
            GameplayerLoadScene.Saida -= ConfigureHudStats;
            DesenvolvimentoState.Entrada -= AtualizarPlayerStatsBairro;

        }

        private void ConfigureHudStats()
        {
            playerStats = PlayerStatsManager.Instance.GetLocalPlayerStats();
            InitializeHudStats();
        }

        private void InitializeHudStats()
        {
            // iconJogador.color = playerStats.Cor;
            // text_nomeJogador.SetText(GameDataconfig.Instance.TagPlayerColorizada(playerStats.playerID));
            text_nomeJogador.SetText(playerStats.PlayerName);
            text_nomeJogador.color = playerStats.Cor;
            text_eleitores.SetText(textToDisplayEleitores);
            text_objetivo.SetText(playerStats.ObjetivoCarta);
            text_cadeiras.SetText(string.Concat(playerStats.NumCadeiras.Value, "/", GameDataconfig.Instance.CadeirasTotal));

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
                text_eduCarta.SetText(playerStats.numEducacao.ToString());//"Edu: " +

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
            playerStats.RemoveEleitoresNovos();
            // playerStats.eleitoresNovos--;
            //text_eleitoresNovos.SetText(string.Concat("+",playerStats.eleitoresNovos.ToString()));
            if (playerDiminuiEleitor)
            {
                uIVoterCurrency.SetNegativeText((int)playerStats.eleitoresNovos);
                uIVoterCurrency.PlayEnterAnim();
            }
            else
            {
                uIVoterCurrency.SetPositiveText((int)playerStats.eleitoresNovos);
                uIVoterCurrency.PlayEnterAnim();

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
        public void sairIcone(){
             uIVoterCurrency.PlayExitAnim();
        }
        

        //para o bairro acessar quantos eleitores novos podem ser distribuidos
        public void valorEleitorNovo()
        {
            eleitoresNovosAtual = playerStats.eleitoresNovos;
        }

        //botao chama funcao de distribuicao de eleitor no inicio das rodadas
        public void ChamatPlayerInicioRodada()
        {
            StartCoroutine(EsperaEVai1(0));


        }
        private IEnumerator EsperaEVai1(float s)
        {
            yield return new WaitForSeconds(s);
            eleitoresAdicionais = 0;
            if (distribuicaoInicial == true)
            {
                checaZonasInteiras(true);
                //se for state do inicio
                //tem zona inteira ganha adicional
                for (int i = 0; i < setUpZona.tenhoZona.Count; i++)
                {
                    eleitoresAdicionais += setUpZona.tenhoZona[i].eleitoresAdicionais;
                    // Debug.Log("voce possui " + setUpZona.tenhoZona[i].eleitoresAdicionais + " eleitore(s) adicionais por conquistar a zona " + setUpZona.tenhoZona[i].Nome + " inteira!");
                }
                playerStats.inicioRodada(eleitoresAdicionais);
                distribuaEleitorUi.SetActive(true);
                distribuicaoGeral = true;
                text_distribuaEleitor.SetText("Distribua seus eleitores");
                //text_eleitoresNovos.SetText(string.Concat("+",playerStats.eleitoresNovos.ToString()));
                uIVoterCurrency.SetPositiveText((int)playerStats.eleitoresNovos);
                eleitoresNovosDeProjeto?.Invoke();

            }
        }

        public void ValorEleitoresNovos(int valor)
        {
            if (playerRecebeEleitor == true)
            {
                // playerStats.eleitoresNovos = valor;
                playerStats.SetEleitoresNovos(valor);
                playerRecebeEleitor = false;
                //
                distribuaEleitorUi.SetActive(true);
                distribuicaoGeral = true;
                if (playerDiminuiEleitor == true)
                {
                    text_distribuaEleitor.SetText("Retire seus eleitores");
                    uIVoterCurrency.SetNegativeText((int)playerStats.eleitoresNovos);

                }
                else
                {
                    text_distribuaEleitor.SetText("Distribua seus eleitores");
                    uIVoterCurrency.SetPositiveText((int)playerStats.eleitoresNovos);

                }

                eleitoresNovosDeProjeto?.Invoke();
            }

        }
        public PlayerStats GetPlayerStats()
        {
            return playerStats;
        }

        //TODO: Marcando aqui para refatorar depois
        public void AtualizarCadeirasUI(int valor)
        {
            playerStats.SetNumCadeiras(valor);
        }
        public void BntsAuxiliares()
        {
            btnsAux.SetActive(!btnsAux.activeSelf);
        }
        public void checaZonasInteiras(bool valorParticula)
        {
            setUpZona.PlayerTemZonaInteira(playerStats.playerID, valorParticula);
        }
        public void escondeObjetivo(GameObject go)
        {
            ativa = !ativa;
            if (ativa)
            {
                olhoEsconde[0].SetActive(true);
                olhoEsconde[1].SetActive(false);
                go.SetActive(true);
            }
            else
            {
                olhoEsconde[0].SetActive(false);
                olhoEsconde[1].SetActive(true);
                go.SetActive(false);
            }



        }

    }

}
