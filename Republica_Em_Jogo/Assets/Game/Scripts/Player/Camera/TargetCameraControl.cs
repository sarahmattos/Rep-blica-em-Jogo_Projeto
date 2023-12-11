using System;
using System.Collections;
using System.Collections.Generic;
using Game.Tools;
using UnityEngine;

namespace Game.Player
{
    public class TargetCameraControl : MonoBehaviour
    {

        private Vector3 mouseWorldPosStart;
        [SerializeField][Range(1, 50)] private int scrollWheelSpeed;
        [SerializeField][Range(1, 20)] private int directionalInputSpeed;
        // public float xClamp => boundary.Size.x;
        // public float zClamp => boundary.Size.z;

        [SerializeField] private Boundary boundary;

        void Update()
        {
            if (Input.GetMouseButtonDown(2) && !Input.GetKey(KeyCode.LeftShift))
            {

                mouseWorldPosStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }


            if (Input.GetMouseButton(2) && !Input.GetKey(KeyCode.LeftShift))
            {
                Pan();
            }

            ProcessDirectionalInputs();
            PerformMobileMovement();
        }

        private void PerformMobileMovement()
        {


            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector3 touchDirection = new Vector3(Input.GetTouch(0).deltaPosition.x, 0, Input.GetTouch(0).deltaPosition.y);

                    IncreasePositionBounded(-1 * touchDirection);
                }


            }

        }

        private void Pan()
        {
            if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
            {
                Vector3 mouseWorldPosDiff = mouseWorldPosStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                IncreasePositionBounded(mouseWorldPosDiff * scrollWheelSpeed);

            }
        }

        private void ProcessDirectionalInputs()
        {
            Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            IncreasePositionBounded(inputDirection * directionalInputSpeed);

        }

        public void IncreasePositionBounded(Vector3 position)
        {
            transform.position += position * Time.deltaTime;
            transform.ClampPositionXZ(boundary.Bounds);
        }



    }
}
