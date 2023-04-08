using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Territorio
{
    [RequireComponent(typeof(InteragivelVisualiza))]
    public class Interagivel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private bool habilitado;

        public event Action<Bairro> Click;
        public event Action<bool> MudaHabilitado;
        public event Action MouseExit;
        public event Action MouseEnter;
        private Bairro bairro;
        private InteragivelVisualiza interagivelVisualiza;


        void Awake()
        {
            bairro = GetComponentInParent<Bairro>();
        }

        private void Start()
        {
            MudarHabilitado(false);
        }

        public void MudarHabilitado(bool value)
        {
            MudaHabilitado?.Invoke(value);
            habilitado = value;
            if (!value)
                MouseExit?.Invoke();

        }

        public void OnPointerEnter(PointerEventData eventData)
        {

            if (!habilitado) return;
            MouseEnter?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!habilitado) return;
            Click?.Invoke(bairro);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!habilitado) return;
            MouseExit?.Invoke();
        }
    }
}
