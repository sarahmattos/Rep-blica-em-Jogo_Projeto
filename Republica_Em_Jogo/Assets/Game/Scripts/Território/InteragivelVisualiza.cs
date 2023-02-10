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
        private Material material;
        private Color colorMouseIn = Color.cyan;
        private Color colorMouseOut;
        private Color colorMouseClick = Color.white;
        private Vector3 scalaInicial;
        private void Awake()
        {
            interagivel = GetComponent<Interagivel>();
            material = GetComponent<MeshRenderer>().material;
            bairro = GetComponentInParent<Bairro>();
        }

        private void Start()
        {
            scalaInicial = transform.localScale;
            interagivel.mouseEnter += MouseInVisualiza;
            interagivel.mouseExit += MouseOutVisualiza;
            interagivel.click += MouseClickVisualiza;
            interagivel.mudaHabilitado += OnMudaHabilitado;
            bairro.playerIDNoControl.OnValueChanged += ResetMouseOutColor;
        }

        private void OnDestroy()
        {
            interagivel.mouseEnter -= MouseInVisualiza;
            interagivel.mouseExit -= MouseOutVisualiza;
            interagivel.click -= MouseClickVisualiza;
            interagivel.mudaHabilitado -= OnMudaHabilitado;
            bairro.playerIDNoControl.OnValueChanged -= ResetMouseOutColor;

        }

        private void MouseInVisualiza() {
            material.color = colorMouseIn;
        }

        private void MouseOutVisualiza()
        {
            material.color = colorMouseOut;            
        }

        private void MouseClickVisualiza(Bairro _) {
            material.color = colorMouseClick;
        }


        private void ResetMouseOutColor(int _, int playerID) {
            colorMouseOut = GameDataConfig.Instance.PlayerColorOrder[playerID];
            material.color = colorMouseOut;

        }

        private void OnMudaHabilitado(bool value) {
            transform.localScale = value ? 
                new Vector3(scalaInicial.x, scalaInicial.y, scalaInicial.z*3) : 
                new Vector3(scalaInicial.x, scalaInicial.y, scalaInicial.z/3);
        }





    }
}
