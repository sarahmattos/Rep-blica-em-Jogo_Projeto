using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostrarNomeBairro : MonoBehaviour
{
    private Renderer renderer;
    //[SerializeField] private Material highlightMaterial;
    [SerializeField] private GameObject nomeBairro;
    //[SerializeField] private Material defaultMaterial;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        //renderer.material = highlightMaterial;
        nomeBairro.SetActive(true);
    }
    private void OnMouseExit()
    {
        //renderer.material = defaultMaterial;
        nomeBairro.SetActive(false);
    }
}
