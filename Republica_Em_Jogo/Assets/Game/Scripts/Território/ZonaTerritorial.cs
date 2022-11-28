
using Game.Tools;
using UnityEngine;

namespace Game.Territorio
{
    public class ZonaTerritorial : MonoBehaviour
    {

        [SerializeField] private Bairro[] bairros;

        public Bairro[] Bairros => bairros;

        private void Awake()
        {
            bairros = GetComponentsInChildren<Bairro>();
            bairros.Shuffle();
        }



    }


}
