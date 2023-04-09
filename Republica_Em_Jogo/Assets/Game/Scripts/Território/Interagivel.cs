using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
        public event Action<bool> SelectBairro;

        void Awake()
        {
            bairro = GetComponentInParent<Bairro>();
        }

        private void Start()
        {
            MudarHabilitado(false);
            bairro.selected.OnValueChanged += OnChangeSelectBairro;
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
