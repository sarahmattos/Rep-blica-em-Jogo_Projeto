using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using Game.UI;

namespace Game.Player {

    public class PlayerStats : NetworkBehaviour
    {

        [SerializeField] private Color cor;
        [SerializeField] private int maxTerritorio;
        [SerializeField] private Objetivo objetivo;
        [SerializeField] private string objetivoCarta;
        [SerializeField] private List<string> recursoCarta = new List<string>();
        [SerializeField] public int numSaude;
        [SerializeField] public int numEducacao;
        [SerializeField] private int saudeRecurso;
        [SerializeField] private int educacaoRecurso;
        [SerializeField] private string nome;
        [SerializeField] private int eleitoresTotais;
        public int bairrosTotais;
        public float eleitoresNovos;
        public RecursoCartaObjeto recursoManager;

        public int playerID => (int)OwnerClientId;
        public Color Cor { get => cor; }
        public Objetivo Objetivo { get => objetivo; }
        public string ObjetivoCarta { get => objetivoCarta; }
        public string Nome { get => nome; }
        public int EleitoresTotais { get => eleitoresTotais; }
        public int SaudeRecurso { get => saudeRecurso; }
        public int EducacaoRecurso { get => educacaoRecurso; }
        public State GameplayLoadState => GameStateHandler.Instance.StatePairValue[GameState.GAMEPLAY_SCENE_LOAD];
        public NetworkVariable<int> eleitoresNetwork = new NetworkVariable<int>(-1);

         private void OnEnable()
            {
                eleitoresNetwork.OnValueChanged += (int previousValue, int newValue) =>
                {
                    eleitoresTotais = newValue;
                };
            }
        [ServerRpc(RequireOwnership = false)]
        public void getEleitoresNetworkServerRpc(int eleitores)
        { 
            eleitoresNetwork.Value=eleitores;
        }

        private void Start()
        {
            GameplayLoadState.Saida += InicializaPlayerStats;
        }
        public override void OnDestroy()
        {
            GameplayLoadState.Saida -= InicializaPlayerStats;
        }

       //randomiza qual carta de recurso
        public void recursoDistribuicao(int quantidade){
            for(int i = 0; i < quantidade; i++)
                {
                    string rnd = recursoManager.tipoRecurso[Random.Range(0, recursoManager.tipoRecurso.Length)];
                    recursoCarta.Add(rnd);
                    if(rnd == "Saúde"){
                        numSaude++;
                    }
                    if(rnd == "Educação"){
                        numEducacao++;
                    }
                }
        }
        
        //divide numero de bairros para adicionar novos eleitores
        public void inicioRodada()
         {
             eleitoresNovos = Mathf.Floor(bairrosTotais / 2);
             Debug.Log("eleitoresNovos "+eleitoresNovos);
         }

        public void InicializaPlayerStats()
        {
            Tools.Logger.Instance.LogInfo("inicializando player stats");
            cor = GameDataConfig.Instance.PlayerColorOrder[playerID];
            maxTerritorio = GameDataConfig.Instance.territoriosInScene;
            /*tive que colocar em outro lugar :eleitoresAtualizar();
            //eleitoresTotais = maxTerritorio / /*clientsConnected.Count; 2;
            */
            nome = string.Concat("jogador ", playerID);
            objetivoCarta = objetivosDatabase.Instance.objetivoComplemento;

        }
        
        public void eleitoresAtualizar(){
            eleitoresTotais++;
        }
        public void eleitoresDiminuir(){
            eleitoresTotais--;
        }
        public void bairrosAtualizar(){
            bairrosTotais++;
        }

    }
}

