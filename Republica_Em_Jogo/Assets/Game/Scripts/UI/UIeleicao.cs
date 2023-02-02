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

        private State eleicaoState => GameStateHandler.Instance.StatePairValue[GameState.ELEICOES];

       
        
        private void Start()
        {
            UIeleicaoObjsParent.SetActive(false);
            eleicaoState.Entrada += OnEleicaoEntrada;
            eleicaoState.Saida += OnEleicaoSaida;
        }
        private void Update()
        {
            cadeirasUi.text= EleicaoManager.Instance.cadeirasCamara.ToString();
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



    }
}

