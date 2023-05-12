using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Game.UI
{
    public class UIRecursoCurrency : MonoBehaviour
    {
        [SerializeField] private UIRecursoData[] uiRecursosData;
        [SerializeField] private MovimentosSociais movimentosSociais;
        [SerializeField] private TMP_Text currencyText;
        [SerializeField] private Image currencyIcon;

        private Animator animator;

        private State ProjetoState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.PROJETO];

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }


        private void Start()
        {
            ProjetoState.Saida += PlayExitAnim;
            movimentosSociais.DistribuicaoRecurso += ConfigureCurrency;
            PlayExitAnim();

        }


        private void OnDestroy()
        {
            ProjetoState.Saida -= PlayExitAnim;
            movimentosSociais.DistribuicaoRecurso -= ConfigureCurrency;

        }

        private void ConfigureCurrency(string recursoTipo, int quantidade)
        {
            currencyIcon.sprite = GetSprite(recursoTipo);
            currencyText.SetText("+",quantidade);

            PlayEnterAnim();
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


        public void SetRecursoCurrentText(string value)
        {
            currencyText.SetText(value);
        }

        private Sprite GetSprite(string recursoTipo) {
            foreach(UIRecursoData recursoData in uiRecursosData) {
                if(recursoData.RecursoTipo == recursoTipo) {
                    return recursoData.Icone;
                }
            }
            throw new NullReferenceException("Recurso tipo não exite.");
        }




    }


    [System.Serializable]
    public struct UIRecursoData
    {
        [SerializeField] private string recursoTipo;
        [SerializeField] private Sprite icone;

        public string RecursoTipo => recursoTipo;
        public Sprite Icone => icone;
    }

}
