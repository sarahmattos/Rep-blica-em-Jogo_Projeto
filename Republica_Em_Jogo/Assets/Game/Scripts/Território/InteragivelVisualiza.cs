using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using Game.Tools;
using System.Linq;

namespace Game.Territorio
{
    public class InteragivelVisualiza : NetworkBehaviour
    {
        public Interagivel interagivel;
        public Bairro bairro;
        public BairroColorModificador bairroColorModificador;

        [Header("Bairro on focus Property")]
        private Vector3 scaleInitial;
        [SerializeField] private float zPlus = 2;

        private void Awake()
        {
            interagivel = GetComponent<Interagivel>();
            bairro = GetComponentInParent<Bairro>();
            bairroColorModificador = GetComponent<BairroColorModificador>();

        }

        private void Start()
        {

            scaleInitial = transform.localScale;

            interagivel.MouseEnter += MouseEnterVisualiza;
            interagivel.MouseExit += MouseOutVisualiza;
            interagivel.MudaHabilitado += OnMudaHabilitado;
            interagivel.OnFocus += OnFocusBairroMuda;
            bairro.SetUpBairro.Eleitores.NumeroEleitores.OnValueChanged += OnNumeroEleitoresMuda;
            bairro.PlayerIDNoControl.OnValueChanged += bairroColorModificador.SetBairroColorByPlayer;
            interagivel.Inactivity += bairroColorModificador.SetInativityColor;
            GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZACAO).Entrada += Initialize;
        }


        private void OnDestroy()
        {
            interagivel.MouseEnter -= MouseEnterVisualiza;
            interagivel.MouseExit -= MouseOutVisualiza;
            interagivel.MudaHabilitado -= OnMudaHabilitado;
            interagivel.OnFocus -= OnFocusBairroMuda;
            bairro.SetUpBairro.Eleitores.NumeroEleitores.OnValueChanged -= OnNumeroEleitoresMuda;
            bairro.PlayerIDNoControl.OnValueChanged -= bairroColorModificador.SetBairroColorByPlayer;
            interagivel.Inactivity -= bairroColorModificador.SetInativityColor;
            GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZACAO).Entrada -= Initialize;
        }

        private void Initialize()
        {
            GetComponent<Outline>().enabled = true;
            bairroColorModificador.ResetColorMasking();
        }



        private void MouseEnterVisualiza()
        {
            bairroColorModificador.SetMouseEnterColor();
        }
        private void MouseOutVisualiza()
        {
            bairroColorModificador.SetMouseOutColor();
        }


        //quando o número de eleitores muda, significa que algum jogador clicou no bairro correspondente
        //ajuda a visualizar quando o número de eleitores mudou
        private void OnNumeroEleitoresMuda(int _, int __)
        {
            BlinkBairro();
        }

        private async void BlinkBairro()
        {
            await bairroColorModificador.BlinkBairroColorTask();
        }

        private void OnFocusBairroMuda(bool value)
        {
            Vector3 targetScale = Vector3.zero;
            if (value)
            {
                targetScale = new Vector3(scaleInitial.x, scaleInitial.y, scaleInitial.z * zPlus);
            }
            else
            {
                targetScale = new Vector3(scaleInitial.x, scaleInitial.y, scaleInitial.z);
                bairroColorModificador.ResetColorMasking();
            }
            SetScale(targetScale);
        }

        private void OnMudaHabilitado(bool value)
        {

            if (!value)
            {
                bairroColorModificador.SetMouseOutColor();
            }
        }

        private async void SetScale(Vector3 targetScale)
        {
            await transform.EaseOutScale(targetScale);

        }


    }


}
