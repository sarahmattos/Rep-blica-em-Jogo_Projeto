using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
 using Game.Territorio;

public class Educaçao : NetworkBehaviour
{
    // Start is called before the first frame update
    private NetworkVariable<int> quantidadeEducação = new NetworkVariable<int>(0);
    private TMP_Text text_edu;
    private RecursosCartaManager rc;
    public bool playerControlRecurso=false;
    private Bairro bairro;

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

        }

     private void OnMouseDown()
    {
        bairro.VerificaControl();
        if(bairro.playerInControl==true){
            if(rc.novosEdu>0){
                rc.novosEdu--;
                AtualizarValorUIServerRpc();
            }
        }
        
        
    }
    private void OnEnable()
        {
            quantidadeEducação.OnValueChanged += (int  previousValue, int  newValue) =>
            {
                text_edu.SetText(newValue.ToString());
            };
        }
}
