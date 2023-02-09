using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using Game.UI;
using Game.Territorio;
using System.Linq;

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
        // public Bairro[] bairrosInControl;
        private List<Bairro> bairrosInControl = new List<Bairro>();
        public List<Bairro> BairrosInControl => bairrosInControl;
        // public int bairrosTotais;
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


        private void Awake()
        {
            bairrosInControl = new List<Bairro>();

        }
        private void Start()
        {
            
            GameplayLoadState.Saida += InicializaPlayerStats;
            GameplayLoadState.Saida += InscreveReceberbairrosPlayerIDControl;
        }

        public override void OnDestroy()
        {
            GameplayLoadState.Saida -= InicializaPlayerStats;
            GameplayLoadState.Saida -= InscreveReceberbairrosPlayerIDControl;
        }

        private void InscreveReceberbairrosPlayerIDControl() {
            foreach(ZonaTerritorial zonas in  SetUpZona.Instance.Zonas) {
                foreach(Bairro bairro in zonas.Bairros){
                    bairro.bairroPlayerLocalNoControl += ArmazenaBairroInControl;
                    bairro.bairroPlayerLocalForaControl += RetiraBairroInControl;
                }
            }
        }

        private void DesinscreveReceberbairrosPlayerIDControl() {
            foreach(ZonaTerritorial zonas in  SetUpZona.Instance.Zonas) {
                foreach(Bairro bairro in zonas.Bairros){
                    bairro.bairroPlayerLocalNoControl -= ArmazenaBairroInControl;
                    bairro.bairroPlayerLocalForaControl -= RetiraBairroInControl;
                }
            }
        }

        private void ArmazenaBairroInControl(Bairro bairro, int playerID)
        {        
            if(playerID == this.playerID)
                BairrosInControl.Add(bairro);
        }

        private void RetiraBairroInControl(Bairro bairro, int playerID)
        {
            if(playerID == this.playerID)
                BairrosInControl.Remove(bairro);
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
            eleitoresNovos = Mathf.Floor(bairrosInControl.Count / 2);
            Debug.Log("eleitoresNovos "+eleitoresNovos);
        }

        public void InicializaPlayerStats()
        {
            Tools.Logger.Instance.LogInfo("inicializando player stats");
            cor = GameDataConfig.Instance.PlayerColorOrder[playerID];
            maxTerritorio = GameDataConfig.Instance.territoriosInScene;
            nome = string.Concat("jogador ", playerID);
            objetivoCarta = objetivosDatabase.Instance.objetivoComplemento;

        }

        public void ContaEleitoresInBairros()
        {
            eleitoresTotais = 0;

            for (int i = 0; i < bairrosInControl.Count; i++)
            {
                eleitoresTotais += bairrosInControl[i].SetUpBairro.Eleitores.contaEleitores;
                //Debug.Log(bairrosInControl[i].Nome + " " + bairrosInControl[i].SetUpBairro.Eleitores.contaEleitores);
            }

        }


    }
}

