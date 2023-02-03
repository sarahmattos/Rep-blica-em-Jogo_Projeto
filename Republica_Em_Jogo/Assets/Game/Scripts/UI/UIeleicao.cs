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
        private State eleicaoState => GameStateHandler.Instance.StatePairValue[GameState.ELEICOES];

       
        
        private void Start()
        {
            UIeleicaoObjsParent.SetActive(false);
            eleicaoState.Entrada += OnEleicaoEntrada;
            eleicaoState.Saida += OnEleicaoSaida;
            Instance = this;
        }
        private void Update()
        {
           
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
            UIeleicaoObjsParent.SetActive(false);
        }
        public void MostrarCadeiras(string valor)
        {
            cadeirasUi.text = valor;
            Debug.Log("entrou ui");
            EleicaoManager.Instance.DefaultServerRpc();
            EleicaoManager.Instance.resetauxs();
        }


    }
}

