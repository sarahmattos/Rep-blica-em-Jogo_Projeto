using System;
using TMPro;
using UnityEngine;


namespace Game.UI
{
    public class UIVoterCurrency : MonoBehaviour
    {
        [SerializeField] private TMP_Text textEleitoresNovos;
        private Animator animator;
        private State DistribuicaoInicialState => GameStateHandler.Instance.StateMachineController.GetState((int)CoreLoopState.DISTRIBUICAO);
        public State DistribuicaoState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.DISTRIBUICAO];
        public State ProjetoState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.PROJETO];
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }


        private void Start()
        {
            DistribuicaoInicialState.Entrada += PlayEnterAnim;
            DistribuicaoInicialState.Saida += PlayExitAnim;

            DistribuicaoState.Entrada += PlayEnterAnim;
            DistribuicaoState.Saida += PlayExitAnim;
            HudStatsJogador.Instance.eleitoresNovosDeProjeto += PlayEnterAnim;

            ProjetoState.Saida += PlayExitAnim;
            PlayExitAnim();
        }

        private void OnDestroy()
        {
            DistribuicaoInicialState.Entrada -= PlayEnterAnim;
            DistribuicaoInicialState.Saida -= PlayExitAnim;

            DistribuicaoState.Entrada -= PlayEnterAnim;
            DistribuicaoState.Saida -= PlayExitAnim;

            HudStatsJogador.Instance.eleitoresNovosDeProjeto -= PlayEnterAnim;


            ProjetoState.Saida += PlayExitAnim;

        }

        public void PlayEnterAnim()
        {
            // if (!TurnManager.Instance.LocalIsCurrent) return;
            animator.Play("CurrencyEnter");
        }

        public void PlayExitAnim()
        {
            animator.Play("CurrencyExit");
        }

        public void ShowPositiveNovosEleitores(int value)
        {
            textEleitoresNovos.SetText(string.Concat("+", value));
        }

        public void ShowNegativeNovosEleitores(int value)
        {
            textEleitoresNovos.SetText(string.Concat("-", value));
        }

        public void SetVoterCurrentText(string value)
        {
            textEleitoresNovos.SetText(value);
        }

    }
}
