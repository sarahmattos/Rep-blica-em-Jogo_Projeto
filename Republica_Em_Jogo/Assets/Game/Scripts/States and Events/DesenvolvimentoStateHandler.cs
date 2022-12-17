using Game.Tools;
using System;
using Unity.Netcode;
using UnityEngine;
using Logger = Game.Tools.Logger;

namespace Game
{
    public enum DesenvolState
    {
        DISTRIBUICAO,
        AVANCAR,
        PROJETO,
        RECOMPENSA
    }

    public class DesenvolvimentoStateHandler : NetworkSingleton<DesenvolvimentoStateHandler>
    {
        public NetworkVariable<int> desenvStateIndex = new NetworkVariable<int>();

        public event Action distribuicao; //stateIndex = 0;
        public event Action avancar;//stateIndex = 1;
        public event Action projeto;//stateIndex = 2;
        public event Action recompensa;//stateIndex = 3;

        private void Awake()
        {
            desenvStateIndex.OnValueChanged += OnDesenvStateChanged;

        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        public override void OnDestroy()
        {
            desenvStateIndex.OnValueChanged -= OnDesenvStateChanged;
        }


        public override void OnNetworkSpawn()
        {
            desenvStateIndex.OnValueChanged += OnDesenvStateChanged;
            GameStateHandler.Instance.gameplaySceneLoad += onGameplayLoad;
        }

        public override void OnNetworkDespawn()
        {
            desenvStateIndex.OnValueChanged -= OnDesenvStateChanged;
            GameStateHandler.Instance.gameplaySceneLoad -= onGameplayLoad;
        }



        private void onGameplayLoad()
        {
            ChangeDesenvStateServerRpc(0);
        }

        private void OnDesenvStateChanged(int previousValue, int newValue)
        {
            switch (newValue)
            {
                case 0:
                    distribuicao?.Invoke();
                    break;
                case 1:
                    avancar?.Invoke();
                    break;
                case 2:
                    projeto?.Invoke();
                    break;
                case 3:
                    recompensa?.Invoke();
                    break;
            }

        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeDesenvStateServerRpc(int state)
        {

            desenvStateIndex.Value = state;
        }

        [ServerRpc(RequireOwnership = false)]
        public void NextDesenvStateServerRpc()
        {
            if (desenvStateIndex.Value < 3)
            {
                desenvStateIndex.Value = desenvStateIndex.Value + 1;
            } else
            {

                //TODO: aqui não ta legal
                TurnManager.Instance.NextTurnServerRpc();


                desenvStateIndex.Value = 0;
            }
            Logger.Instance.LogWarning(string.Concat("Player ", TurnManager.Instance.GetCurrentPlayer, ", no Stado ", (DesenvolState)desenvStateIndex.Value));


        }


    }

}
