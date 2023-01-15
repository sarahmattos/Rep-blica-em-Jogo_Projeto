using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class Educaçao : NetworkBehaviour
{
    // Start is called before the first frame update
    private NetworkVariable<int> quantidadeEducação = new NetworkVariable<int>(0);
    private TMP_Text text_edu;
    private RecursosCartaManager rc;

    [ServerRpc(RequireOwnership = false)]
        public void AtualizarValorUIServerRpc()
        {
            quantidadeEducação.Value++;
        }
    private void Awake()
        {
            text_edu = GetComponentInChildren<TMP_Text>();
            rc = FindObjectOfType<RecursosCartaManager>();

        }

     private void OnMouseDown()
    {
        if(rc.novosEdu>0){
            rc.novosEdu--;
            AtualizarValorUIServerRpc();
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
