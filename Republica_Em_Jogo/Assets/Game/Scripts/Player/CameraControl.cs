using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float rotationSpeed = 500.0f;
    private Vector3 mouseWorldPosStart;
    private float zoomScale = 10.0f;
    private float zoomMin = 3f;
    private float zoomMan = 10.0f;
    public float[] xClamp = new float[2];
    public float[] zClamp = new float[2];
    [SerializeField] Transform target;
    void Update()
    {
        /*if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Mouse2))
        {
            CamOrbit();
        }*/
        
        if (Input.GetMouseButtonDown(2) && !Input.GetKey(KeyCode.LeftShift))
        {
           // if (transform.position.x > -9 && transform.position.x < 11)
           // {
                //if (transform.position.y > -2 && transform.position.y < 2) {
                    
                    mouseWorldPosStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //}
                // Debug.Log(Input.GetAxis("Mouse Y"));
            //}
            //else
            //{
               // transform.position = new Vector3(0f, 0f, 0f);
           // }
    
        }
        if (Input.GetMouseButton(2) && !Input.GetKey(KeyCode.LeftShift))
        {
            Pan();
        }
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }
    private void CamOrbit()
    {
        if(Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, -verticalInput);
            transform.Rotate(Vector3.forward, -horizontalInput, Space.World);
        }
    }
    private void Pan()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector3 mouseWorldPosDiff = Input.mousePosition;
            target.position+= mouseWorldPosDiff;
                
           // transform.position += mouseWorldPosDiff;
            float posX = Mathf.Clamp(target.position.x , xClamp[0] ,xClamp[1]);
            float posZ = Mathf.Clamp(target.position.z , zClamp[0] ,zClamp[1]);
            transform.position =Vector3.Lerp(
                transform.position,
             new Vector3(posX,transform.position.y,posZ),
             Time.deltaTime* 50
             );;
            
        }
    }
    private void Zoom(float zoomDiff)
    {
        if(zoomDiff != 0)
        {
            mouseWorldPosStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - zoomDiff * zoomScale, zoomMin, zoomMan);
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position += mouseWorldPosDiff;
        }
    }
}
