using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "projetoAtributos", menuName = "Projeto/Novo Projeto")]
public class ProjetoObject : ScriptableObject
{
    public string[] proposta;
    public int[] numRecompensa;
    public string recompensaText;
}
