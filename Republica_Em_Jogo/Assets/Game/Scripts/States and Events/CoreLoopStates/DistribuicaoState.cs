using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;

namespace Game
{
    public class DistribuicaoState : State
    {
        [SerializeField] private RecursosCartaManager rc;
        [SerializeField] private HudStatsJogador hs;

        public void Start(){
            TurnManager.Instance.vezDoPlayerLocal+= quandoVezPlayerLocal;
            
        }
        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("EnterState: DISTRIBUI��O");

        }

        public override void ExitState()
        {
            hs.distribuicaoInicial=false;
        }
        public void OnDestroy(){
            TurnManager.Instance.vezDoPlayerLocal-= quandoVezPlayerLocal;
        }

        public void quandoVezPlayerLocal(bool value){
            if(value){
                hs.distribuicaoInicial=true;
                //rc.chamarDistribuicao=true;
                rc.conferirQuantidade();
                Debug.Log("deu certo");
            }
        }
    }

}
