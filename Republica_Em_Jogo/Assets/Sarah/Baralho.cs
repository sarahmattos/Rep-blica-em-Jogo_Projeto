using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Territorio
{
public class Baralho : MonoBehaviour
{
    private Renderer renderer;
    private Projeto projeto;

    void Start(){
    projeto= GameObject.FindObjectOfType<Projeto>();
     renderer = GetComponent<Renderer>();
    }
    
    private void OnMouseDown()
    {
        Debug.Log("clicado");
        projeto.sortearProjeto();
        //cartaProjetoTrue();
    }
    
    public void cartaProjetoTrue(){
        projeto.projetoUI.SetActive(true);
        projeto.verProjetoBtn.SetActive(false);
    }
    public void cartaProjetoFalse(){
        projeto.projetoUI.SetActive(false);
        projeto.verProjetoBtn.SetActive(true);
    }
}
}

