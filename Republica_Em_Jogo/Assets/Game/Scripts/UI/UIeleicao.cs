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
        private State eleicaoState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.ELEICOES);


        private void Start()
        {
            GameStateHandler.Instance.GetState(GameState.INICIALIZACAO).Saida += AtualizarTextPlayers;
            UIeleicaoObjsParent.SetActive(false);
            eleicaoState.Entrada += OnEleicaoEntrada;
            eleicaoState.Saida += OnEleicaoSaida;
            Instance = this;
        }
        private void OnDestroy()
        {
            GameStateHandler.Instance.GetState(GameState.INICIALIZACAO).Saida -= AtualizarTextPlayers;
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
            foreach (PlayerStats playerStats in PlayerStatsManager.Instance.AllPlayerStats)
            {
                playersName += string.Concat(GameDataconfig.Instance.TagPlayerColorizada(playerStats), ": \n");
            }
            textPlayers.SetText(playersName);
        }


    }
}

