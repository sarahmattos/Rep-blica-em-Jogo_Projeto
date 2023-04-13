using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Territorio
{
    public enum PointerState {
        ENTER,
        EXIT
    }
    [RequireComponent(typeof(InteragivelVisualiza))]
    public class Interagivel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
    {
        [SerializeField]
        private bool habilitado;
        public event Action<Bairro> Click;
        public event Action<Bairro> MouseDown;
        public event Action<bool> MudaHabilitado;
        public event Action MouseExit;
        public event Action MouseEnter;
        private Bairro bairro;
        public event Action<bool> SelectBairro;
        public PointerState PointerState { get; set;}

        void Awake()
        {
            bairro = GetComponentInParent<Bairro>();
        }

        private void Start()
        {
            bairro.selected.OnValueChanged += OnChangeSelectBairro;
            PointerState = PointerState.EXIT;
        }

        private void OnDestroy()
        {
            bairro.selected.OnValueChanged += OnChangeSelectBairro;

        }

        private void OnChangeSelectBairro(bool previousBool, bool nextBool)
        {
            SelectBairro?.Invoke(nextBool);
        }


        public void MudarHabilitado(bool value)
        {
            MudaHabilitado?.Invoke(value);
            habilitado = value;
            if (!value)
                MouseExit?.Invoke();
        }

        public void ChangeSelectedBairro(bool value)
        {
            bairro.ChangeSelectBairroServerRpc(value);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {

            if (!habilitado) return;
            MouseEnter?.Invoke();
            PointerState = PointerState.ENTER;

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
            PointerState = PointerState.EXIT;

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(!habilitado) return;
            MouseDown?.Invoke(bairro);
        }

        

        
    }
}
