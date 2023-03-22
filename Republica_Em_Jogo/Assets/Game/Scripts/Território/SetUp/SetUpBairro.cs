using UnityEngine;

namespace Game.Territorio
{
    public class SetUpBairro : MonoBehaviour
    {
        private Eleitores eleitores;
        private Recursos recursos;

        public Eleitores Eleitores => eleitores;
        public Recursos Recursos => recursos;
        private Transform transformCam;
        private void Awake()
        {
            eleitores = GetComponentInChildren<Eleitores>();
            recursos = GetComponentInChildren<Recursos>();
        }
        private void Start()
        {
            transformCam  = FindObjectOfType<Camera>().transform;
        }
        void Update()
        {
             Vector3 cameraPosition = transformCam.position;
            // Calcular a direção do objeto em relação à câmera
            Vector3 direction = transform.position - cameraPosition;
            direction.y = 0;

            // Criar uma rotação a partir da direção calculada
            Quaternion rotation = Quaternion.LookRotation(direction);
            //float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Atribuir a rotação ao objeto
           // transform.rotation = rotation;
           transform.localRotation  = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        }

    }

}
