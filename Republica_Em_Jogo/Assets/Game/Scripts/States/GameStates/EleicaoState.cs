using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EleicaoState : State
    {
        private GameStateHandler gameStateHandler => GameStateHandler.Instance;

        public override void EnterState()
        {
            StartCoroutine(EsperaEVai(5));
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
            gameStateHandler.StateMachineController.ChangeStateServerRpc((int)GameState.DESENVOLVIMENTO);
        }
    }

}