using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{

    public class ZoomTarget : MonoBehaviour
    {
        private Vector3 mouseWorldPosStart;
        [SerializeField] private Transform target;

        [Header("Zoom properties")]
        [SerializeField] private float zoomScale = 10.0f;
        [SerializeField] private float zoomMin = 3f;
        [SerializeField] private float zoomMax = 10.0f;

        private float orthographicSizeTarget;
        public float OrthographicSizeTarget => orthographicSizeTarget;


        private void Start()
        {
            orthographicSizeTarget = zoomMax;
        }

        void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
                Zoom(Input.GetAxis("Mouse ScrollWheel"));

        }


        private void Zoom(float zoomDiff)
        {
            if (zoomDiff != 0)
            {
                mouseWorldPosStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                orthographicSizeTarget = Mathf.Clamp(orthographicSizeTarget - zoomDiff * zoomScale, zoomMin, zoomMax);

                Vector3 mouseWorldPosDiff = mouseWorldPosStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.position += mouseWorldPosDiff;
                transform.position += mouseWorldPosDiff;

            }
        }




    }
}
