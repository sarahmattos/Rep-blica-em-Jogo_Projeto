using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class InstantiateManager : MonoBehaviour
    {
        public static InstantiateManager Instance;
        [SerializeField] private GameObject UiIconeCorJogador;
        // Start is called before the first frame update
        public State Inicializacaostate => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZACAO);


        void Start()
        {
            Instance = this;
            Inicializacaostate.Saida += OnInicializacaoStateSaida;

        }

        private void OnInicializacaoStateSaida()
        {
            for(int i =0;i<TurnManager.Instance.ordemPlayersID.Count;i++){
                foreach (PlayerStats playerStats in PlayerStatsManager.Instance.AllPlayerStats)
                {
                    if(playerStats.playerID == TurnManager.Instance.ordemPlayersID[i]){
                        InstanciarUi(playerStats.Cor, playerStats.playerID);
                    }
                    
                }
            }
            

        }



        public GameObject InstanciarUi(Color cor, int playerID)
        {
            GameObject _go = Instantiate(UiIconeCorJogador, transform);
            _go.GetComponent<UIIconeCorJogador>().Intialize(cor, playerID);
            // _go.transform.SetParent(_pai, false);
            // Image img = _go.GetComponent<Image>();  
            // img.color = cor;
            return _go;
        }
    }
}
