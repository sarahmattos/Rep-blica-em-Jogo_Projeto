using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Game.Territorio;
using System.Collections.Generic;
using System.Linq;

namespace Game.UI
{
    public class UIRecursoCurrency : MonoBehaviour
    {
        [SerializeField] private UIRecursoData[] uiRecursosData;
        [SerializeField] private MovimentosSociais movimentosSociais;
        [SerializeField] private RecursosCartaManager recursosCartaManager;
        [SerializeField] private TMP_Text currencyText;
        [SerializeField] private Image currencyIcon;
        private Animator animator;

        private State ProjetoState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.PROJETO];
        private int NovosSaude => recursosCartaManager.novosSaude;
        private int NovosEdu => recursosCartaManager.novosEdu;


        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            ProjetoState.Saida += PlayExitAnim;
            recursosCartaManager.IniciaDistribuicaoRecursos += ConfigureForRecursosDeTroca;
            recursosCartaManager.FimDistricaoRecursos += OnFimDistribuicaoRecursos;
            recursosCartaManager.DistribuiuRecurso += OnDistribuiuRecurso;

            movimentosSociais.DistribuicaoRecurso += ConfigureCurrency;

            PlayExitAnim();

        }


        private void OnDestroy()
        {
            ProjetoState.Saida -= PlayExitAnim;
            recursosCartaManager.IniciaDistribuicaoRecursos -= ConfigureForRecursosDeTroca;
            recursosCartaManager.FimDistricaoRecursos -= OnFimDistribuicaoRecursos;
            recursosCartaManager.DistribuiuRecurso -= OnDistribuiuRecurso;

            movimentosSociais.DistribuicaoRecurso -= ConfigureCurrency;

        }

        private void OnDistribuiuRecurso(string recursoType)
        {
            if (recursoType == "Saúde") SetCurrentText(string.Concat("+", NovosSaude));
            if (recursoType == "Educação") SetCurrentText(string.Concat("+", NovosEdu));

        }

        private void OnFimDistribuicaoRecursos(string recursoType)
        {
            if (recursoType == "Saúde") ConfigureCurrency("Educação", NovosEdu);
            if (recursoType == "Educação") ConfigureCurrency("Saúde", NovosSaude);

            if(NovosEdu  == 0 && NovosSaude == 0) PlayExitAnim();

        }

        private void ConfigureCurrency(string recursoTipo, int quantidade)
        {
            currencyIcon.sprite = GetSprite(recursoTipo);
            currencyText.SetText(string.Concat("+", quantidade));

            PlayEnterAnim();
        }

        private void ConfigureForRecursosDeTroca(string recursoTipo, int quantidade)
        {
            currencyIcon.sprite = GetSprite(recursoTipo);
            currencyText.SetText(string.Concat("+", quantidade));

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


        public void SetCurrentText(string value)
        {
            currencyText.SetText(value);
        }

        private Sprite GetSprite(string recursoTipo)
        {
            foreach (UIRecursoData recursoData in uiRecursosData)
            {
                if (recursoData.RecursoTipo == recursoTipo)
                {
                    return recursoData.Icone;
                }
            }
            throw new NullReferenceException("Recurso tipo null (não exite).");
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
