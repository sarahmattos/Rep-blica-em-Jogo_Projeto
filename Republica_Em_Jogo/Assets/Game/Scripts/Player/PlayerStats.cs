using Unity.Netcode;
using UnityEngine;

namespace Game.Player {

    public class PlayerStats : NetworkBehaviour
    {

        [SerializeField] private Color cor;
        [SerializeField] private int maxTerritorio;
        [SerializeField] private Objetivo objetivo;
        [SerializeField] private string objetivoCarta;

        [SerializeField] private string nome;
        [SerializeField] private int eleitoresTotais;
        float eleitoresNovos;

        private void Awake()
        {
            //para os clients inscreverem m�todos no initializePlayers
            GameStateHandler.Instance.initializePlayers += InitializeStats;

        }
        void OnGUI()
        {
            //apenas teste
            if (GUI.Button(new Rect(10, 10, 150, 100), "Distribuicao eleitores"))
            {
              inicioRodada();
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
            GameStateHandler.Instance.initializePlayers += InitializeStats;
        }

        public int playerID => (int)OwnerClientId;
        
        public Color Cor { get => cor; }
        public Objetivo Objetivo { get => objetivo;}
        public string ObjetivoCarta { get => objetivoCarta; }
        public string Nome { get => nome;}
        public int EleitoresTotais { get => eleitoresTotais; }
        
        public override void OnNetworkSpawn()
        {
            GameStateHandler.Instance.initializePlayers += InitializeStats;
        }

        public override void OnNetworkDespawn()
        {
            GameStateHandler.Instance.initializePlayers -= InitializeStats;

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

