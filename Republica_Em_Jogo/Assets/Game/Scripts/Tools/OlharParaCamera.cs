using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class OlharParaCamera : MonoBehaviour
{
    [SerializeField] private bool active;
    private float tempoAtualizaRotacao = 0.3f;
    private Transform transformCam;

    private void Awake()
    {
        transformCam = FindObjectOfType<Camera>().transform;
    }

    private void Start()
    {
        //SE A CAMERA ROTACIONA DURANTE O JOGO:
        // StartCoroutine(LookRotationRotina());

        //DO CONTRARIO, APENAS:
        SetTransformLookAt();

    }
    private void SetTransformLookAt()
    {
        Vector3 posicaoAlvo = transform.position + transformCam.forward;
        transform.LookAt(posicaoAlvo, transformCam.up);
    }


    // private void OnDestroy()
    // {
    //     StopCoroutine(LookRotationRotina());

    // }

    // private IEnumerator LookRotationRotina()
    // {
    //     while (active)
    //     {
    //         yield return new WaitForSeconds(tempoAtualizaRotacao);
    //         SetTransformLookAt();
    //     }
    // }




    // void LateUpdate()
    // {
    //     if(active)
    //     {

    //     }

    // }

}
