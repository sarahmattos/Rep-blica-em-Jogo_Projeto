using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game.Territorio
{
    public class InteragivelVisualiza : MonoBehaviour
    {
        private Interagivel interagivel;
        private Bairro bairro;
        private MeshRenderer meshRenderer;
        private Color colorMouseIn = Color.cyan;
        private Color colorMouseOut;
        private Color colorMouseClick = Color.white;
        private MaterialPropertyBlock propertyBlock;
        private static readonly int colorID = Shader.PropertyToID("_Color");

        //Apenas para testes
        private Vector3 scalaInicial;
        private void Awake()
        {
            interagivel = GetComponent<Interagivel>();
            meshRenderer = GetComponent<MeshRenderer>();
            bairro = GetComponentInParent<Bairro>();
            propertyBlock = new MaterialPropertyBlock();
        }

        private void Start()
        {
            scalaInicial = transform.localScale;
            interagivel.MouseEnter += MouseInVisualiza;
            interagivel.MouseExit += MouseOutVisualiza;
            interagivel.Click += MouseClickVisualiza;
            interagivel.MudaHabilitado += OnMudaHabilitado;
            interagivel.SelectBairro += OnSelectBairro;
            bairro.PlayerIDNoControl.OnValueChanged += ResetMouseOutColor;

        }

        private void OnDestroy()
        {
            interagivel.MouseEnter -= MouseInVisualiza;
            interagivel.MouseExit -= MouseOutVisualiza;
            interagivel.Click -= MouseClickVisualiza;
            interagivel.MudaHabilitado -= OnMudaHabilitado;
            interagivel.SelectBairro -= OnSelectBairro;
            bairro.PlayerIDNoControl.OnValueChanged -= ResetMouseOutColor;

        }

        private void MouseInVisualiza()
        {
            SetColor(colorMouseIn);
        }

        private void MouseOutVisualiza()
        {
            SetColor(colorMouseOut);
        }

        private void MouseClickVisualiza(Bairro _)
        {
            SetColor(colorMouseClick);
        }


        private void ResetMouseOutColor(int _, int playerID)
        {
            colorMouseOut = GameDataconfig.Instance.PlayerColorOrder[playerID];
            SetColor(colorMouseOut);

        }

        //Apenas para testes
        private void OnMudaHabilitado(bool value)
        {
            transform.localScale = value ?
                new Vector3(scalaInicial.x, scalaInicial.y, scalaInicial.z * 1.4f) :
                new Vector3(scalaInicial.x, scalaInicial.y, scalaInicial.z);
        }

        private void SetColor(Color color)
        {
            propertyBlock.SetColor(colorID, color);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }

        private void OnSelectBairro(bool value)
        {
            Tools.Logger.Instance.LogPlayerAction("Selecionou o bairro "+value+" : " + bairro.Nome);
        }





    }
}
