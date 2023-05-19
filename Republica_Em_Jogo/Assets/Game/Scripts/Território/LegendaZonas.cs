using System.Collections;
using System.Collections.Generic;
using Game.Tools;
using UnityEngine;

namespace Game.Territorio
{
    public class LegendaZonas : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        public State InicializacaoState => GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZAÇÃO);

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        void Start()
        {
            canvasGroup.alpha = 0;
            InicializacaoState.Entrada += SetMaxCanvasGroup;
        }

        private void OnDestroy()
        {
            InicializacaoState.Entrada -= SetMaxCanvasGroup;

        }


        private async void SetMaxCanvasGroup()
        {
            await canvasGroup.EaseOutAlpha(1);
        }


    }
}
