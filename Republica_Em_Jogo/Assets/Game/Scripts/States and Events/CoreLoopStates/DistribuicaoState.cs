using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DistribuicaoState : State
    {
        [SerializeField] private RecursosCartaManager rc;

        public void Start(){
            TurnManager.Instance.vezDoPlayerLocal+= quandoVezPlayerLocal;
        }
        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("EnterState: DISTRIBUI��O");
        }

        public override void ExitState()
        {

        }
        public void OnDestroy(){
            TurnManager.Instance.vezDoPlayerLocal-= quandoVezPlayerLocal;
        }

        public void quandoVezPlayerLocal(bool value){
            if(value){
                rc.conferirQuantidade();
                Debug.Log("deu certo");
            }
        }
    }

}
