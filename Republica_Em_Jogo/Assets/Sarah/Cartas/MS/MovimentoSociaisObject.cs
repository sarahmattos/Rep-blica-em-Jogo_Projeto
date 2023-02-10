using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovimentosSociaisAtributos", menuName = "Projeto/Novo MovimentosSociais")]
public class MovimentoSociaisObject : ScriptableObject
{
    public string[] movimento;
    public string[] recursoTipo;
    public int quantidadeRecurso;
    public int quantidadeEleitor;
}
