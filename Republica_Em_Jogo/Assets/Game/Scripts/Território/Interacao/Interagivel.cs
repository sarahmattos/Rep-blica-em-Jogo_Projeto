using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Territorio
{
    public enum PointerState
    {
        ENTER,
        EXIT
    }

    [RequireComponent(typeof(InteragivelVisualiza))]
    public class Interagivel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        private bool habilitado;
        private Bairro bairro;
        public event Action<Bairro> Click;
        public event Action<bool> MudaHabilitado;
        public event Action MouseEnter;
        public event Action MouseExit;
        public event Action MouseDown;
        public event Action MouseUp;
        public event Action<bool> OnFocus;
        public event Action<bool> Inactivity;
        public PointerState PointerState { get; set; }
        public bool Habilitado => habilitado;


        void Awake()
        {
            bairro = GetComponentInParent<Bairro>();
        }

        private void Start()
        {
            PointerState = PointerState.EXIT;
            bairro.OnFocus.OnValueChanged += OnChangeSelectBairro;
        }

        private void OnDestroy()
        {
            bairro.OnFocus.OnValueChanged -= OnChangeSelectBairro;

        }

        private void OnChangeSelectBairro(bool previousBool, bool nextBool)
        {
            OnFocus?.Invoke(nextBool);
        }


        public void MudarHabilitado(bool value)
        {
            MudaHabilitado?.Invoke(value);
            habilitado = value;
            if (!value)
                MouseExit?.Invoke();
            // Inactivity?.Invoke(false);
        }

        public void MudarInativity(bool value)
        {
            Inactivity?.Invoke(value);
        }

        public void ChangeSelectedBairro(bool value)
        {
            bairro.ChangeSelectBairroServerRpc(value);

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerState = PointerState.ENTER;
            if (!habilitado) return;
            MouseEnter?.Invoke();

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!(eventData.button == PointerEventData.InputButton.Left)) return;
            if (!habilitado) return;
            Click?.Invoke(bairro);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerState = PointerState.EXIT;
            if (!habilitado) return;
            MouseExit?.Invoke();

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!habilitado) return;
            MouseDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!habilitado) return;
            MouseUp?.Invoke();
        }






    }
}
