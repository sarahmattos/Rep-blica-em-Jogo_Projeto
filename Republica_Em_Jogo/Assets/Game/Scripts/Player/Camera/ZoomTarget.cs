using System;
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
            PerformMouseZoom();
            PerformMobileZoom();
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


        private void PerformMouseZoom()
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                Zoom(Input.GetAxis("Mouse ScrollWheel"));
            }
        }


        private void PerformMobileZoom()
        {
            if (Input.touchCount == 2)
            {
                Touch touchOne = Input.GetTouch(0);
                Touch touchTwo = Input.GetTouch(1);

                Vector2 touchOnePrevPosition = touchOne.position - touchOne.deltaPosition;
                Vector2 touchTwoPrevPosition = touchTwo.position - touchTwo.deltaPosition;

                float prevMagnitude = (touchOnePrevPosition - touchTwoPrevPosition).magnitude;
                float currentMangitude = (touchOne.position - touchTwo.position).magnitude;

                float difference = currentMangitude - prevMagnitude;
                Zoom(difference * 0.001f);

            }
        }


    }
}
