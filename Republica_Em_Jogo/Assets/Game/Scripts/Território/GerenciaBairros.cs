using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Territorio
{
    public class GerenciaBairros : MonoBehaviour
    {
        [SerializeField] private List<Bairro> bairros;
        
        private void Start()
        {
            bairros = FindObjectsOfType<Bairro>().ToList();

        }


    }

}
