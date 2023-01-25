using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Territorio
{


    public class Interagivel : MonoBehaviour
    {
        public event Action click;
        public event Action mouseExit;
        public event Action mouseEnter;
        
        #region  apenas para testes
        private Vector3 escalaInicial;
        // public Vector3 targetScale => new Vector3(escalaInicial.x, escalaInicial.y, escalaInicial.z*3); 
        public Color cor1;
        public Color cor2;
        private Material material;
        #endregion
        void Start()
        {
            material = GetComponent<MeshRenderer>().material;
            escalaInicial = transform.localScale;
        }

        
        void OnMouseEnter()
        {
            material.color = cor2;
            mouseEnter?.Invoke();
            // transform.localScale =  targetScale;
        }

        void OnMouseExit()
        {
            material.color = cor1;
            mouseExit?.Invoke();
            // transform.localScale = escalaInicial;

        }
        
        void OnMouseUpAsButton()
        {
            click?.Invoke();
        }

        




 
    }
}
