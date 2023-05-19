using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game
{
    public class DisplayUiAvanco : MonoBehaviour
    {


        private State avancoState => CoreLoopStateHandler.Instance.StatePairValues[CoreLoopState.AVANÇO];

        private List<GameObject> objetosFilhos;

        private void Awake()
        {
            objetosFilhos = GetOjsFilhos();

        }

        private void Start()
        {
            SetObjFilhosAtivo(false);
            avancoState.Entrada += OnEntrada;
            avancoState.Saida += OnSaida;
            
        }
        
        private void OnDestroy()
        {
            avancoState.Entrada -= OnEntrada;
            avancoState.Saida -= OnSaida;
        }


        private void OnSaida()
        {
            SetObjFilhosAtivo(false);
        }

        private void OnEntrada()
        {
            SetObjFilhosAtivo(true);

        }


        private List<GameObject> GetOjsFilhos() {
            return (from Transform transform in gameObject.GetComponentsInChildren<Transform>()
                    select transform.gameObject).ToList();
        }


        private void SetObjFilhosAtivo(bool value) {
            
            foreach (GameObject child in objetosFilhos)
            {
                transform.gameObject.SetActive(value);
            }
        }




    }
}
