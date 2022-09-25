using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eleitores : MonoBehaviour
{
    public Material[] material1;
    void Start()
    {
        //this.GetComponent<Renderer>().material = material1[0];
       // Material[0] = Object.GetComponent<Renderer>.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void mudarCor(int corId)
    {
        //0 e 2
        this.GetComponent<Renderer>().material = material1[corId];
    }
}
