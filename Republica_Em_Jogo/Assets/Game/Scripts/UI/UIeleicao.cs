using System;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

namespace Game
{
    public class UIeleicao : MonoBehaviour
    {
        [SerializeField] private GameObject UIeleicaoObjsParent;
        [SerializeField] private TMP_Text cadeirasUi;
        public static UIeleicao Instance;
        private State eleicaoState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.ELEICOES);

        
        private void Start()
        {
            UIeleicaoObjsParent.SetActive(false);
            eleicaoState.Entrada += OnEleicaoEntrada;
            eleicaoState.Saida += OnEleicaoSaida;
            Instance = this;
        }
        private void OnDestroy()
        {
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
            UIeleicaoObjsParent.SetActive(false);
        }
        public void MostrarCadeiras(string valor)
        {
            cadeirasUi.text = valor;
        }


    }
}

