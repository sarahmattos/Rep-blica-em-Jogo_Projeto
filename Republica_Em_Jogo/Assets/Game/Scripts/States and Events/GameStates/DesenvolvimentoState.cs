using System;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class DesenvolvimentoState : State
    {
        private NetworkVariable<int> rodada = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public int maxRodada => GameDataConfig.Instance.MaxRodadasParaEleicoes;
        private GameStateHandler StateHandler => GameStateHandler.Instance;        
        public override void EnterState()
        {
           Tools.Logger.Instance.LogInfo("EnterState: DESENVOLVIMENTO");
            if (!IsHost) return;
            TurnManager.Instance.turnoMuda += OnTurnoMuda;
            rodada.Value = 0;
        }

        public override void ExitState()
        {
            if (!IsHost) return;
            TurnManager.Instance.turnoMuda -= OnTurnoMuda;


        }

        private void Start()
        {
            rodada.OnValueChanged += RodadaMuda;
        }

        public override void OnDestroy()
        {
            rodada.OnValueChanged -= RodadaMuda;

        }


        private void OnTurnoMuda(int previousPlayer, int nextPlayer)
        {
            //rodada.Value = (rodada.Value % maxRodada) + 1;
            //if ((TurnManager.Instance.TurnCount % TurnManager.Instance.GetClientesCount) == 0)
            //{
            //    rodada.Value++;
            //}
            Debug.Log("Player anterior: "+previousPlayer);
            Debug.Log("next player: "+nextPlayer);

            if((previousPlayer) == TurnManager.Instance.UltimoPlayer)
            {
                rodada.Value++;
            }

            if(nextPlayer % (TurnManager.Instance.GetClientesCount-1) == 0) {
                Debug.Log("inicio de uma rodada");
            }
        }

        private void RodadaMuda(int previousValue, int newValue)
        {
            if (previousValue == maxRodada)
            {
                StateHandler.ChangeStateServerRpc((int)GameState.ELEICOES);

            }
        }





    }

}