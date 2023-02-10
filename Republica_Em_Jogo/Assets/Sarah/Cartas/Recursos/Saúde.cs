using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
 using Game.Territorio;

public class Saúde : NetworkBehaviour
{
    private NetworkVariable<int> quantidadeSaude = new NetworkVariable<int>(0);
    private TMP_Text text_saude;
    private RecursosCartaManager rc;
    public bool playerControlRecurso =false;
    private Bairro bairro;

    [ServerRpc(RequireOwnership = false)]
        public void AtualizarValorUIServerRpc()
        {
            quantidadeSaude.Value++;
        }

    private void Awake()
        {
            text_saude = GetComponentInChildren<TMP_Text>();
            rc = FindObjectOfType<RecursosCartaManager>();
            bairro = GetComponentInParent<Bairro>();
        }

     private void OnMouseDown()
    {
         if(bairro.VerificaControl()){
            if(rc.novosSaude>0){
                rc.novosSaude--;
                AtualizarValorUIServerRpc();
            }
        }
        
    }
    private void OnEnable()
        {
            quantidadeSaude.OnValueChanged += (int  previousValue, int  newValue) =>
            {
                text_saude.SetText(newValue.ToString());
            };
        }
}