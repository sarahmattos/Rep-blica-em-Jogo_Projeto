using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{


    public class BairroInteragir : MonoBehaviour
    {
        public event Action onClick;
        private Vector3 escalaInicial;
        public Vector3 targetScale => new Vector3(escalaInicial.x, escalaInicial.y, escalaInicial.z*3); 

        void Start()
        {
            escalaInicial = transform.localScale;
        }

        
        void OnMouseEnter()
        {
            transform.localScale =  targetScale;
        }

        void OnMouseExit()
        {
            transform.localScale = escalaInicial;

        }
        
        void OnMouseUpAsButton()
        {
            onClick?.Invoke();
        }

        




 
    }
}
