using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eleitores : MonoBehaviour
{
    public Material[] material1;
    public string cores;
    public Color cor;
    public int id;
    void Start()
    {
        //this.GetComponent<Renderer>().material.color = Color.red;
        //cor = this.GetComponent<Renderer>().material.color;
        //this.GetComponent<Renderer>().material = material1[0];
        // Material[0] = Object.GetComponent<Renderer>.material;
    }

    // Update is called once per frame
    void Update()
    {
        //cor = Color.cores;
        this.GetComponent<Renderer>().material.color = cor;
    }
    public void mudarCor(int corId)
    {
        //0 e 2
        this.GetComponent<Renderer>().material = material1[corId];
    }
}
