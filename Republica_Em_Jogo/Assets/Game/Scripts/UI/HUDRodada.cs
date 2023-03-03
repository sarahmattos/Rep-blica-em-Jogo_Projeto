using UnityEngine;
using TMPro;

namespace Game.UI
{
    public class HUDRodada : MonoBehaviour
    {
        private RodadaController rodadaController;
        private TMP_Text rodada_Text;

        private void Awake()
        {
            rodada_Text = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            GameStateHandler.Instance.StatePairValue[GameState.DESENVOLVIMENTO].Entrada += OnDesenvolvimentoStateEnter;
        }

        private void OnDestroy()
        {
            GameStateHandler.Instance.StatePairValue[GameState.DESENVOLVIMENTO].Entrada -= OnDesenvolvimentoStateEnter;
            rodadaController.rodadaMuda -= UpdateTextRodada;
        }

        public void OnDesenvolvimentoStateEnter()
        {
            rodadaController = FindObjectOfType<RodadaController>();
            rodadaController.rodadaMuda += UpdateTextRodada;

        }

        private void UpdateTextRodada(int value)
        {
            rodada_Text.SetText(string.Concat("Rodada: ", value));
        }


    }
}