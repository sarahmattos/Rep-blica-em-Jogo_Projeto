using UnityEngine;

namespace Game.Territorio
{
    public class SetUpBairro : MonoBehaviour
    {
        private Eleitores eleitores;
        private Recursos recursos;

        public Eleitores Eleitores => eleitores;
        public Recursos Recursos => recursos;
        private void Awake()
        {
            eleitores = GetComponentInChildren<Eleitores>();
            recursos = GetComponentInChildren<Recursos>();
        }


    }

}
