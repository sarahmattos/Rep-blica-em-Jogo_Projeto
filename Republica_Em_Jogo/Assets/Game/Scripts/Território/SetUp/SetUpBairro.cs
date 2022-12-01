using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game.Territorio
{
    public class SetUpBairro : MonoBehaviour
    {
        private Eleitores eleitores;
        private Recursos recursos;

        public Eleitores Eleitores { get => eleitores; set => eleitores = value; }
        public Recursos Recursos { get => recursos; set => recursos = value; }

        private void Awake()
        {
            eleitores = GetComponentInChildren<Eleitores>();
            recursos = GetComponentInChildren<Recursos>();
        }


    }

}
