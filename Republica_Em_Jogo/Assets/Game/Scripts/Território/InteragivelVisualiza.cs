using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

namespace Game.Territorio
{
    public class InteragivelVisualiza : NetworkBehaviour
    {
        public Interagivel interagivel;
        public Bairro bairro;
        public BairroColorModificador bairroColorModificador;
        //Apenas para testes
        private Vector3 scalaInicial;

        private void Awake()
        {
            interagivel = GetComponent<Interagivel>();
            bairro = GetComponentInParent<Bairro>();
            bairroColorModificador = GetComponent<BairroColorModificador>();
            // propertyBlockABase = new MaterialPropertyBlock();

        }

        private void Start()
        {

            scalaInicial = transform.localScale;

            interagivel.MouseEnter += MouseEnterVisualiza;
            interagivel.MouseExit += MouseOutVisualiza;
            // interagivel.Click += OnClickBairro;
            interagivel.MudaHabilitado += OnMudaHabilitado;
            interagivel.SelectBairro += OnSelectBairro;
            bairro.SetUpBairro.Eleitores.NumeroEleitores.OnValueChanged += OnNumeroEleitoresMuda;
            bairro.PlayerIDNoControl.OnValueChanged += bairroColorModificador.SetBairroColorByPlayer;
            interagivel.Inactivity += bairroColorModificador.SetInativityColor;

            GameStateHandler.Instance.StateMachineController.GetState((int)GameState.INICIALIZACAO).Entrada += Initialize;
        }


        private void OnDestroy()
        {
            interagivel.MouseEnter -= MouseEnterVisualiza;
            interagivel.MouseExit -= MouseOutVisualiza;
            //interagivel.Click -= OnClickBairro;
            interagivel.MudaHabilitado -= OnMudaHabilitado;
            interagivel.SelectBairro -= OnSelectBairro;
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

        private void OnSelectBairro(bool value)
        {
            Tools.Logger.Instance.LogPlayerAction("Selecionou o bairro " + value + " : " + bairro.Nome);
        }

        private void OnMudaHabilitado(bool value)
        {
            transform.localScale = value ?
                new Vector3(scalaInicial.x, scalaInicial.y, scalaInicial.z * 1.4f) :
                new Vector3(scalaInicial.x, scalaInicial.y, scalaInicial.z);

            if (!value)
            {
                bairroColorModificador.SetMouseOutColor();
            }
        }


    }


}
