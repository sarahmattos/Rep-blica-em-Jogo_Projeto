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
            StartCoroutine(EsperaEVai(3));
            Tools.Logger.Instance.LogWarning("EnterState: ELEICAO");
            //chamar conta
        }

        public override void ExitState()
        {
            StopAllCoroutines();

        }

        private IEnumerator EsperaEVai(int s)
        {
            yield return new WaitForSeconds(s);
            gameStateHandler.ChangeStateServerRpc((int)GameState.DESENVOLVIMENTO);
        }
    }

}