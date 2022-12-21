using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

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
        [SerializeField] private string nome;
        [SerializeField] private int eleitoresTotais;
        float eleitoresNovos;
        public RecursoCartaObjeto recursoManager;
        public int playerID => (int)OwnerClientId;

        public Color Cor { get => cor; }
        public Objetivo Objetivo { get => objetivo; }
        public string ObjetivoCarta { get => objetivoCarta; }
        public string Nome { get => nome; }
        public int EleitoresTotais { get => eleitoresTotais; }
        private void Awake()
        {
            //para os clients inscreverem m�todos no initializePlayers
            GameStateHandler.Instance.gameplaySceneLoad += InitializeStats;
            

        }
       
        void OnGUI()
        {
            //apenas teste
            if (GUI.Button(new Rect(10, 10, 150, 100), "Distribuicao eleitores"))
            {
              inicioRodada();
             }
             if (GUI.Button(new Rect(10, 120, 150, 100), "Distribuicao recursos"))
            {
             recursoDistribuicao(Random.Range(1, 3));
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
       
        private void inicioRodada()
         {
             eleitoresNovos = eleitoresTotais / 2;
             float eleitoresAdd = Mathf.Floor(eleitoresNovos);
             eleitoresTotais += (int)eleitoresAdd;
             Debug.Log("eleitorestotais "+eleitoresTotais);
             
         }
        public override void OnDestroy()
        {
            //para os clients desinscrever m�todos no initializePlayers
            GameStateHandler.Instance.gameplaySceneLoad += InitializeStats;
        }


        
        public override void OnNetworkSpawn()
        {
            GameStateHandler.Instance.gameplaySceneLoad += InitializeStats;
        }

        public override void OnNetworkDespawn()
        {
            GameStateHandler.Instance.gameplaySceneLoad -= InitializeStats;

        }

        public void InitializeStats()
        {
            cor = GameDataconfig.Instance.PlayerColorOrder[playerID];
            maxTerritorio = GameDataconfig.Instance.territoriosInScene;
            eleitoresTotais = maxTerritorio / /*clientsConnected.Count;*/  2;
            nome = string.Concat("jogador ", playerID);
            objetivoCarta = objetivosDatabase.Instance.objetivoComplemento;


        }
        


    }
}

