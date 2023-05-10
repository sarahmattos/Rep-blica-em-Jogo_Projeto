using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tools;

namespace Game.Player
{

    [RequireComponent(typeof(ZoomTarget))]
    public class CameraControl : MonoBehaviour
    {
        // private float rotationSpeed = 500.0f;
        [SerializeField] private Camera mainCamera;
        private ZoomTarget zoomTarget;

        [Header("move smooth props")]
        [SerializeField] Transform target;
        private float maxSpeed = 10000;
        private float smoothTime = 1f;
        private Vector3 currentVelocity = Vector3.zero;

        [Header("Zoom smooth props")]
        [SerializeField] private float maxSpeed2 = 100000;
        [SerializeField] private float smoothTime2 = 0.01f;
        private float currentVelocity2 = 0;

        private void Start()
        {
            zoomTarget = GetComponent<ZoomTarget>();
        }
        void Update()
        {
            //follow target
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref currentVelocity, smoothTime * Time.deltaTime, maxSpeed);
            //Update by zoomTarget
            mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, zoomTarget.OrthographicSizeTarget, ref currentVelocity2, smoothTime2 * Time.deltaTime, maxSpeed2);

        }


        // private void CamOrbit()
        // {
        //     if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        //     {
        //         float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        //         float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        //         transform.Rotate(Vector3.right, -verticalInput);
        //         transform.Rotate(Vector3.forward, -horizontalInput, Space.World);
        //     }
        // }


    }


}
