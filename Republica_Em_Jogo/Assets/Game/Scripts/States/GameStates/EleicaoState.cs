using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EleicaoState : State
    {
        private GameStateHandler gameStateHandler => GameStateHandler.Instance;
        private bool mostrouAviso=false;
        public override void EnterState()
        {
            EleicaoManager.Instance.inEleicao=true;
            StartCoroutine(EsperaEVai(5));
           if(mostrouAviso==false){
                mostrouAviso=true;
                EleicaoManager.Instance.explicarEleicao();
            }
            //chamar conta
            
        }

        public override void ExitState()
        {
            StopAllCoroutines();

        }

        private IEnumerator EsperaEVai(int s)
        {
            yield return new WaitForSeconds(s);
            EleicaoManager.Instance.escondeExplicarEleicao();
            gameStateHandler.StateMachineController.ChangeStateServerRpc((int)GameState.DESENVOLVIMENTO);
        }
    }

}