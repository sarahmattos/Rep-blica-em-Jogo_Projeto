using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eleitores : MonoBehaviour
{
    public Material[] material;
    public Color cor;
    public int id;
    void Start()
    {
    }
    void Update()
    {
        
    }
    public void mudarCor()
    {
        this.GetComponent<Renderer>().material.color = cor;
    }
}
