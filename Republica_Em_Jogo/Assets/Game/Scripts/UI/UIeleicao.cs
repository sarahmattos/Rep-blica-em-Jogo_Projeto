using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class UIeleicao : MonoBehaviour
    {
        [SerializeField] private GameObject UIeleicaoObjsParent;
        private State eleicaoState => GameStateHandler.Instance.StatePairValue[GameState.ELEICOES];

       
        
        private void Start()
        {
            UIeleicaoObjsParent.SetActive(false);
            eleicaoState.Entrada += OnEleicaoEntrada;
            eleicaoState.Saida += OnEleicaoSaida;
        }

        private void OnDestroy()
        {
            eleicaoState.Entrada -= OnEleicaoEntrada;
            eleicaoState.Saida -= OnEleicaoSaida;
        }

        private void OnEleicaoEntrada()
        {
            UIeleicaoObjsParent.SetActive(true);
        }

        private void OnEleicaoSaida()
        {
            UIeleicaoObjsParent.SetActive(false);
        }



    }
}

