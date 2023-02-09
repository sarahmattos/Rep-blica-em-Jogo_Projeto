using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Territorio
{


    public class Interagivel : MonoBehaviour
    {
        [SerializeField]    private bool habilitado;

        public event Action<Bairro> click;
        public event Action mouseExit;
        public event Action mouseEnter;    

        private new Collider collider;

        private Material material;
        private Bairro bairro;

        public Material Material  => material; 
        

        void Start()
        {
            material = GetComponent<MeshRenderer>().material;
            bairro = GetComponentInParent<Bairro>();
        }


        void OnMouseEnter()
        {
            if(!habilitado) return;
            mouseEnter?.Invoke();
        }

        void OnMouseExit()
        {
            if(!habilitado) return;
            mouseExit?.Invoke();
        }

        void OnMouseUpAsButton()
        {
            if(!habilitado) return;
            click?.Invoke(bairro);
        }

        public void MudarHabilitado(bool value) {
            habilitado = value;
            if(!value) mouseExit?.Invoke();
        }


    }
}
