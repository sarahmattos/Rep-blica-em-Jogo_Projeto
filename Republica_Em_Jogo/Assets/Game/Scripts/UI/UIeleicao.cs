using System;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Game.Player;

namespace Game
{
    public class UIeleicao : MonoBehaviour
    {
        [SerializeField] private GameObject UIeleicaoObjsParent;
        [SerializeField] private TMP_Text textPlayers;
        [SerializeField] private TMP_Text textCadeiras;
        public static UIeleicao Instance;
        private State eleicaoState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.ELEIÇÕES);


        private void Start()
        {
            GameStateHandler.Instance.GetState(GameState.INICIALIZAÇÃO).Saida += AtualizarTextPlayers;
            UIeleicaoObjsParent.SetActive(false);
            eleicaoState.Entrada += OnEleicaoEntrada;
            eleicaoState.Saida += OnEleicaoSaida;
            Instance = this;
        }
        private void OnDestroy()
        {
            GameStateHandler.Instance.GetState(GameState.INICIALIZAÇÃO).Saida -= AtualizarTextPlayers;
            eleicaoState.Entrada -= OnEleicaoEntrada;
            eleicaoState.Saida -= OnEleicaoSaida;
        }

        private void OnEleicaoEntrada()
        {
            EleicaoManager.Instance.CalculoEleicao();
            UIeleicaoObjsParent.SetActive(true);
        }

        private void OnEleicaoSaida()
        {
            EleicaoManager.Instance.setCameraPosition(false);
            EleicaoManager.Instance.inEleicao = false;
            UIeleicaoObjsParent.SetActive(false);
        }
        public void MostrarCadeiras(string valor)
        {
            textCadeiras.text = valor;
        }

        public void AtualizarTextPlayers()
        {

            string playersName = "";
            foreach (int playerID in TurnManager.Instance.ordemPlayersID)
            {
                PlayerStats playerStats = PlayerStatsManager.Instance.GetPlayerStats(playerID);
                playersName += string.Concat(GameDataconfig.Instance.TagPlayerColorizada(playerStats), ": \n");
            }
            textPlayers.SetText(playersName);
        }


    }
}

