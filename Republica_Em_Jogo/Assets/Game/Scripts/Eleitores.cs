using Game.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Territorio
{
    public class Eleitores : MonoBehaviour
    {
        [SerializeField] Material material;
        [SerializeField] private int count = 0;

        private void Awake()
        {
            material = GetComponent<MeshRenderer>().material;
            ////material.MudarCor(Color.gray);
        }

    }


}
