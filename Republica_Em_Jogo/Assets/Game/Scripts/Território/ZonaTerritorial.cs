
using Game.Tools;
using UnityEngine;

namespace Game.Territorio
{
    public class ZonaTerritorial : MonoBehaviour
    {

        [SerializeField] private Bairro[] bairros;
        int x;

        public Bairro[] Bairros => bairros;

        private void Awake()
        {
            bairros = GetComponentsInChildren<Bairro>();
        }

        private void verificarPlayerNasZonas(ulong client)
        {
            foreach(Bairro bairro in bairros)
            {
                if(bairro.playerIDNoControl.Value == (int)client)
                {
                    x++;
                }
            }
        }


    }


}
