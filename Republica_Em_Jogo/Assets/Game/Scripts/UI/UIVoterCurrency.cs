using System;
using TMPro;
using UnityEngine;


namespace Game.UI
{
    public class UIVoterCurrency : MonoBehaviour
    {
        [SerializeField] private TMP_Text textEleitoresNovos;
        private Animator animator;
        public State DistribuicaoState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.DISTRIBUICAO];
        public State ProjetoState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.PROJETO];
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        
        private void Start()
        {
            DistribuicaoState.Entrada += OnEnterState;
            HudStatsJogador.Instance.eleitoresNovosDeProjeto += OnEnterState;

            DistribuicaoState.Saida += OnExitState;
            ProjetoState.Saida += OnExitState;
        }

        private void OnDestroy()
        {
            DistribuicaoState.Entrada -= OnEnterState;
            HudStatsJogador.Instance.eleitoresNovosDeProjeto -= OnEnterState;


            DistribuicaoState.Saida -= OnExitState;
            ProjetoState.Saida += OnExitState;

        }

        private void OnEnterState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            animator.Play("VoterCurrencyEnter");
        }

        private void OnExitState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            animator.Play("VoterCurrencyExit");
        }

        public void ShowPositiveNovosEleitores(int value) {
            textEleitoresNovos.SetText(string.Concat("+",value));
        }
        public void ShowNegativeNovosEleitores(int value) {
            textEleitoresNovos.SetText(string.Concat("-",value));
        }

    }
}
