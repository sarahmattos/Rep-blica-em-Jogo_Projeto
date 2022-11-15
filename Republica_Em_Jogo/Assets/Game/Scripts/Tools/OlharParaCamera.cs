using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlharParaCamera : MonoBehaviour
{
    [SerializeField] private bool active;
   private Transform transformCam;

    private void Awake()
    {
        transformCam = FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(active)
        {
            Vector3 posicaoAlvo = transform.position + transformCam.forward;
            transform.LookAt(posicaoAlvo, transformCam.up);
        }

    }
}
