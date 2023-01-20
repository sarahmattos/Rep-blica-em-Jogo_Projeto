using Game.Territorio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ProjetoState : State
    {
        [SerializeField] private Baralho baralho;

        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("Enter State: PROJETO");
            baralho.enabled = true;

        }

        public override void ExitState()
        {
            baralho.enabled = false;

        }

        private void Start()
        {
            baralho.enabled = false;
        }
    }

}
