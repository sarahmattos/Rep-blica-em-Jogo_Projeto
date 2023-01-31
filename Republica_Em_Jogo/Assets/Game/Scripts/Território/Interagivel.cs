using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Territorio
{


    public class Interagivel : MonoBehaviour
    {
        [SerializeField]    private bool habilitado;

        public event Action click;
        public event Action mouseExit;
        public event Action mouseEnter;    

        private new Collider collider;
        public Color cor1;
        public Color cor2;
        private Material material;


        void Start()
        {
            material = GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {

            }
        }

        void OnMouseEnter()
        {
            if(!habilitado) return;
            
            material.color = cor2;
            mouseEnter?.Invoke();
            // transform.localScale =  targetScale;
        }

        void OnMouseExit()
        {
            if(!habilitado) return;
           
            material.color = cor1;
            mouseExit?.Invoke();
            // transform.localScale = escalaInicial;

        }

        void OnMouseUpAsButton()
        {
            if(!habilitado) return;
            click?.Invoke();
        }

        public void MudarHabilitado(bool value) {
            habilitado = value;
            if(!value) mouseExit?.Invoke();
        }


    }
}
