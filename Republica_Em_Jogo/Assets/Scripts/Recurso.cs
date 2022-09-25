using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recurso : MonoBehaviour
{
    public static  Recurso instance;
    public int id;
    public string tipo;
    void Start()
    {
        instance = this;
    }
    void Update()
    {
        
    }
}
