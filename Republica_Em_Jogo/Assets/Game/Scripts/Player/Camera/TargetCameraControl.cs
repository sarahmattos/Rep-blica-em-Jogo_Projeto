using System.Collections;
using System.Collections.Generic;
using Game.Tools;
using UnityEngine;

namespace Game.Player
{
    public class TargetCameraControl : MonoBehaviour
    {

        private Vector3 mouseWorldPosStart;
        [SerializeField] private float speed;
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
        }
        private void Pan()
        {
            // if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
            // {
                Vector3 mouseWorldPosDiff = mouseWorldPosStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position += mouseWorldPosDiff * speed;
                transform.ClampPositionXZ(boundary.Bounds);

            // }
        }



    }
}
