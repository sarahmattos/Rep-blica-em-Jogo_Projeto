using System;
using UnityEngine;


namespace Game.UI
{
    public class UIVoterCurrency : MonoBehaviour
    {
        private Animator animator;
        public State DistribuicaoState => CoreLoopStateHandler.Instance.StatePairValues[(int)CoreLoopState.DISTRIBUICAO];
        public State ProjetoState => CoreLoopStateHandler.Instance.StatePairValues[(int)CoreLoopState.DISTRIBUICAO];
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

    }
}
