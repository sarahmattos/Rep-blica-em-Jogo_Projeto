using System.Collections;
using System.Collections.Generic;
using Game.Tools;
using UnityEngine;

namespace Game.Player
{
    public class TargetCameraControl : MonoBehaviour
    {

        private Vector3 mouseWorldPosStart;
        [SerializeField][Range(1, 50)]  private int scrollWheelSpeed;
        [SerializeField] [Range(1, 20)] private int directionalInputSpeed;
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
        }
        private void Pan()
        {
            if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
            {
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            IncreasePositionLimited(mouseWorldPosDiff * scrollWheelSpeed);
            // transform.position += mouseWorldPosDiff * speed;
            // transform.ClampPositionXZ(boundary.Bounds);

            }
        }

        private void ProcessDirectionalInputs()
        {
            Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            IncreasePositionLimited(inputDirection*directionalInputSpeed);

        }

        private void IncreasePositionLimited(Vector3 position)
        {
            transform.position += position*Time.deltaTime;
            transform.ClampPositionXZ(boundary.Bounds);
        }



    }
}
