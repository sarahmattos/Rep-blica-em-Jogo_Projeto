using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Game.Territorio;
using System;

public class Educaçao : NetworkBehaviour
{
    // Start is called before the first frame update
    private NetworkVariable<int> quantidadeEducação = new NetworkVariable<int>(0);
    private TMP_Text text_edu;
    private RecursosCartaManager rc;
    public bool playerControlRecurso = false;
    private Bairro bairro;
    private Recursos recurso;
    public GameObject eduIcone;


    [ServerRpc(RequireOwnership = false)]
    public void AtualizarValorUIServerRpc()
    {
        quantidadeEducação.Value++;
    }
    private void Awake()
    {
        text_edu = GetComponentInChildren<TMP_Text>();
        rc = FindObjectOfType<RecursosCartaManager>();
        bairro = GetComponentInParent<Bairro>();
        recurso = GetComponentInParent<Recursos>();

    }
    private void Start()
    {
        eduIcone = this.transform.GetChild(0).gameObject;
        eduIcone.SetActive(false);
    }
    private void OnMouseDown()
    {
        //adicionarEdu

    }
    public void adicionarEdu()
    {
        if (bairro.VerificaControl())
        {
            if (rc.novosEdu > 0)
            {
                rc.RemoveNovosEdu();
                AtualizarValorUIServerRpc();
            }
        }
    }
    private void OnEnable()
    {
        quantidadeEducação.OnValueChanged += (int previousValue, int newValue) =>
        {

            recurso.educacao = newValue;
            text_edu.SetText(newValue.ToString());
            if (newValue > 0)
            {
                eduIcone = this.transform.GetChild(0).gameObject;
                eduIcone.SetActive(true);
            }

        };
    }
}
