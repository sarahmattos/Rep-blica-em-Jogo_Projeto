using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using Game.UI;
using Game.Territorio;
using System.Linq;
using System;
using Unity.Collections;

namespace Game.Player
{

    public class PlayerStats : NetworkBehaviour
    {
        private NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] private Color cor;
        [SerializeField] private int maxTerritorio;
        [SerializeField] private string objetivo;
        [SerializeField] private string objetivoCarta;
        [SerializeField] private List<string> recursoCarta = new List<string>();
        [SerializeField] public int numSaude;
        [SerializeField] public int numEducacao;
        [SerializeField] private int saudeRecurso;
        [SerializeField] private int educacaoRecurso;
        [SerializeField] private string nome;
        [SerializeField] private int eleitoresTotais;
        private NetworkVariable<int> numCadeiras = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public int NumCadeiras => numCadeiras.Value;
        public void SetNumCadeiras(int value) => numCadeiras.Value = value;
        public int bairrosTotais => bairrosInControl.Count;
        private List<Bairro> bairrosInControl = new List<Bairro>();
        public List<Bairro> BairrosInControl { get => bairrosInControl; set => bairrosInControl = value; }

        public int eleitoresNovos;
        public RecursoCartaObjeto recursoManager;

        public int playerID => (int)OwnerClientId;
        public string PlayerName => playerName.Value.ToString();

        public Color Cor { get => cor; }
        public string Objetivo { get => objetivo; }
        public string ObjetivoCarta { get => objetivoCarta; }
        public string Nome { get => nome; }
        public int EleitoresTotais { get => eleitoresTotais; }
        public int SaudeRecurso { get => saudeRecurso; }
        public int EducacaoRecurso { get => educacaoRecurso; }
        public State GameplayLoadState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.GAMEPLAY_SCENE_LOAD);

        public event Action<int> DefiniuEleitoresNovos;
        public event Action<int> DistribuiuEleitor; // int -> eleitoresNovos
        public event Action FimDistricaoEleitores;

        private void Awake()
        {
            BairrosInControl = new List<Bairro>();

        }
        private void Start()
        {
            GameplayLoadState.Saida += InicializaPlayerStats;
            GameplayLoadState.Saida += InscreveReceberbairrosPlayerIDControl;
        }

        public override void OnDestroy()
        {
            GameplayLoadState.Saida -= InicializaPlayerStats;
            GameplayLoadState.Saida -= DesinscreveReceberbairrosPlayerIDControl;
        }

        public override void OnNetworkSpawn()
        {
            SetPlayerName(PlayerNameHandler.Instance.GetInputNameValue);
        }


        // [ClientRpc]
        private void SetPlayerName(string playerName)
        {
            if(!IsOwner) return;
            this.playerName.Value = playerName;
        }


        public void SetEleitoresNovos(int value)
        {
            eleitoresNovos = value;
            DefiniuEleitoresNovos?.Invoke(eleitoresNovos);
        }

        public void RemoveEleitoresNovos()
        {
            eleitoresNovos--;
            DistribuiuEleitor?.Invoke(eleitoresNovos);

            if (eleitoresNovos > 0) return;
            FimDistricaoEleitores?.Invoke();
        }


        private void InscreveReceberbairrosPlayerIDControl()
        {
            foreach (ZonaTerritorial zonas in SetUpZona.Instance.Zonas)
            {
                foreach (Bairro bairro in zonas.Bairros)
                {
                    bairro.bairroPlayerLocalNoControl += ArmazenaBairroInControl;
                    bairro.bairroPlayerLocalForaControl += RetiraBairroInControl;
                }
            }
        }

        private void DesinscreveReceberbairrosPlayerIDControl()
        {
            foreach (ZonaTerritorial zonas in SetUpZona.Instance.Zonas)
            {
                foreach (Bairro bairro in zonas.Bairros)
                {
                    bairro.bairroPlayerLocalNoControl -= ArmazenaBairroInControl;
                    bairro.bairroPlayerLocalForaControl -= RetiraBairroInControl;
                }
            }
        }

        private void ArmazenaBairroInControl(Bairro bairro, int playerID)
        {
            if (playerID == this.playerID)
                BairrosInControl.Add(bairro);
        }

        private void RetiraBairroInControl(Bairro bairro, int playerID)
        {
            if (playerID == this.playerID)
                BairrosInControl.Remove(bairro);
        }

        //randomiza qual carta de recurso
        public void recursoDistribuicao(int quantidade)
        {
            for (int i = 0; i < quantidade; i++)
            {
                string rnd = recursoManager.tipoRecurso[UnityEngine.Random.Range(0, recursoManager.tipoRecurso.Length)];
                recursoCarta.Add(rnd);
                if (rnd == "Saúde")
                {
                    numSaude++;
                }
                if (rnd == "Educação")
                {
                    numEducacao++;
                }
            }
        }

        //divide numero de bairros para adicionar novos eleitores
        public void inicioRodada(int eleitoresAdicionais)
        {
            int totalEleitoresNovos = (int)(Mathf.Floor(bairrosTotais / 2)) + eleitoresAdicionais;
            SetEleitoresNovos(totalEleitoresNovos);
            if (eleitoresNovos < 3) SetEleitoresNovos(3);

        }

        public void InicializaPlayerStats()
        {
            cor = GameDataconfig.Instance.PlayerColorOrder[playerID];
            maxTerritorio = GameDataconfig.Instance.TerritoriosTotal;
            nome = string.Concat(GameDataconfig.Instance.TagParticipante, " ", playerID);
            objetivoCarta = objetivosDatabase.Instance.objetivoComplemento;
            objetivo = objetivosDatabase.Instance.objetivoZona;


        }

        public void ContaEleitoresInBairros()
        {
            eleitoresTotais = 0;

            for (int i = 0; i < BairrosInControl.Count; i++)
            {
                eleitoresTotais += BairrosInControl[i].SetUpBairro.Eleitores.contaEleitores;
                //Debug.Log(bairrosInControl[i].Nome + " " + bairrosInControl[i].SetUpBairro.Eleitores.contaEleitores);
            }

        }

        // public void ContaBairros()
        // {
        //     bairrosTotais = BairrosInControl.Count;
        // }


    }
}

