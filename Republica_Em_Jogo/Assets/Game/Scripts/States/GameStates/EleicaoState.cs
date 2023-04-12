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
            Debug.Log(mostrouAviso);
           if(mostrouAviso==false){
                mostrouAviso=true;
                EleicaoManager.Instance.explicarEleicao();
                Debug.Log("entrou aqui");
            }
            //chamar conta
            
        }
        private void Start()
        {
            
            Debug.Log(transform.parent.gameObject.name);
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