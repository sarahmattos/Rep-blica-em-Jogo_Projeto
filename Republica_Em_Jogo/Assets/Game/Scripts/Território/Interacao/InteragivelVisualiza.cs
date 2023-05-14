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
        private Interagivel interagivel;
        private Bairro bairro;
        private BairroColorModificador bairroColorModificador;

        [Header("Bairro on focus Property")]
        private Vector3 bairroScaleInitial;
        private Vector3 setupPositionInitial;

        [SerializeField] private float zBairroScalePlus = 2;
        [SerializeField] private float ySetupBairroPositionPlus = 3.5f;

        private void Awake()
        {
            interagivel = GetComponent<Interagivel>();
            bairro = GetComponentInParent<Bairro>();
            bairroColorModificador = GetComponent<BairroColorModificador>();

        }

        private void Start()
        {

            bairroScaleInitial = transform.localScale;
            setupPositionInitial = bairro.SetUpBairro.transform.localPosition;

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

        private void BlinkBairro()
        {
            bairroColorModificador.BlinkBairroColorTask();
        }

        private void OnFocusBairroMuda(bool value)
        {
            Vector3 bairroTargetScale = Vector3.zero;
            Vector3 setupTargetPosition = Vector3.zero;
            if (value)
            {
                bairroTargetScale = new Vector3(bairroScaleInitial.x, bairroScaleInitial.y, bairroScaleInitial.z * zBairroScalePlus);
                setupTargetPosition = new Vector3(setupPositionInitial.x, setupPositionInitial.y * ySetupBairroPositionPlus, setupPositionInitial.z);
            }
            else
            {
                bairroTargetScale = new Vector3(bairroScaleInitial.x, bairroScaleInitial.y, bairroScaleInitial.z);
                setupTargetPosition = new Vector3(setupPositionInitial.x, setupPositionInitial.y, setupPositionInitial.z);

                bairroColorModificador.ResetColorMasking();
            }
            SetBairroScale(bairroTargetScale);
            SetSetupBairroPosition(setupTargetPosition);
        }

        private void OnMudaHabilitado(bool value)
        {

            if (!value)
            {
                bairroColorModificador.SetMouseOutColor();
            }
        }

        private async void SetBairroScale(Vector3 targetScale)
        {
            await transform.EaseOutScale(targetScale);

        }

        private async void SetSetupBairroPosition(Vector3 targetPosition)
        {
            await bairro.SetUpBairro.transform.EaseOutLocalPosition(targetPosition);

        }
    }


}
