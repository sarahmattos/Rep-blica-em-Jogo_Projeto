using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CorrupcaoAtributos", menuName = "Projeto/Nova Corrupcao")]
public class CorrupcaoObject : ScriptableObject
{
    public string[] corrupcao;
    public string penalidade;
    //eleitor e cartas
    public string complementText;
}
