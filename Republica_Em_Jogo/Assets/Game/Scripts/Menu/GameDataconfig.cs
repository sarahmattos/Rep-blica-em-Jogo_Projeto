using Game.Networking;
using Game.Tools;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game {
    public class GameDataConfig : Singleton<GameDataConfig>
    {
        [SerializeField] private int maxConnections;
        [SerializeField] private string menuSceneName;
        [SerializeField] private string gameplaySceneName;
        [SerializeField] private List<Color> playerColorOrder;
        [SerializeField] private string tagParticipante;
        [SerializeField] private int maxRodadasParaEleicoes;
        [SerializeField] private bool podeMultiplasInstanciasLocais;

        public int territoriosInScene;
        public int MaxConnections => maxConnections;
        public List<Color> PlayerColorOrder => playerColorOrder;
        public string TagParticipante => tagParticipante;
        public int MaxRodadasParaEleicoes => maxRodadasParaEleicoes;

        public string MenuSceneName => menuSceneName;
        public string GameplaySceneName => gameplaySceneName;
        public bool PodeMultiplasInstanciasLocai => podeMultiplasInstanciasLocais;


        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }



    }

}
