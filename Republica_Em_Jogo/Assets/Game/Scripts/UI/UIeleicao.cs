using System;
using UnityEngine;
using TMPro;
using Game.Player;
using Game.Tools;

namespace Game
{
    public class UIeleicao : Singleton<UIeleicao>
    {
        [SerializeField] private GameObject UIeleicaoObjsParent;
        [SerializeField] private TMP_Text textPlayers;
        [SerializeField] private TMP_Text textCadeiras;
        private State eleicaoState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.ELEIÇÕES);


        private void Start()
        {
            eleicaoState.Entrada += AtualizarTextPlayers;
            eleicaoState.Entrada += AtualizarTextCadeiras;
            UIeleicaoObjsParent.SetActive(false);
            eleicaoState.Entrada += OnEleicaoEntrada;
            eleicaoState.Saida += OnEleicaoSaida;
        }
        private void OnDestroy()
        {
            eleicaoState.Entrada -= AtualizarTextPlayers;
            eleicaoState.Entrada -= AtualizarTextCadeiras;
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

        public void AtualizarTextPlayers()
        {

            string text = "";
            foreach (int playerID in TurnManager.Instance.ordemPlayersID)
            {
                PlayerStats playerStats = PlayerStatsManager.Instance.GetPlayerStats(playerID);
                text += string.Concat(GameDataconfig.Instance.TagPlayerColorizada(playerStats), ": \n");
            }
            textPlayers.SetText(text);
        }

        public void AtualizarTextCadeiras()
        {
            string text = "";
            foreach (int playerID in TurnManager.Instance.ordemPlayersID)
            {
                PlayerStats playerStats = PlayerStatsManager.Instance.GetPlayerStats(playerID);
                int playerCadeiras = EleicaoManager.Instance.CalculoCadeiras.Calcular(playerStats);
                text += string.Concat(playerCadeiras, " cadeiras \n");
            }
            textCadeiras.SetText(text);
        }


    }
}

