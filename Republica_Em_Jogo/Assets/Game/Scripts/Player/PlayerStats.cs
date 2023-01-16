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
        public State InicialializaState => GameStateHandler.Instance.GameStatePairValue[GameState.INICIALIZACAO];
        private void Start()
        {
            //para os clients inscreverem m�todos no initializePlayers    
            InicialializaState.Saida += InitializeStats;
            
        }
        void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "eleitores"))
            {
               inicioRodada();
            }
        }
       
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
        
        public void inicioRodada()
         {
             eleitoresNovos = Mathf.Floor(bairrosTotais / 2);
             eleitoresTotais += (int)eleitoresNovos;
             Debug.Log("eleitorestotais "+eleitoresTotais);
             
         }
        public override void OnDestroy()
        {
            //para os clients desinscrever m�todos no initializePlayers
            
            InicialializaState.Saida -= InitializeStats;
        }


        public override void OnNetworkSpawn()
        {
            InicialializaState.Saida += InitializeStats;
        }

        public override void OnNetworkDespawn()
        {
            InicialializaState.Saida -= InitializeStats;

        }

        public void InitializeStats()
        {
            Tools.Logger.Instance.LogInfo("inicializando player stats");
            cor = GameDataconfig.Instance.PlayerColorOrder[playerID];
            maxTerritorio = GameDataconfig.Instance.territoriosInScene;
            eleitoresTotais = maxTerritorio / /*clientsConnected.Count;*/  2;
            //eleitoresTotais = bairrosTotais;
            nome = string.Concat("jogador ", playerID);
            objetivoCarta = objetivosDatabase.Instance.objetivoComplemento;

        }
        


    }
}

