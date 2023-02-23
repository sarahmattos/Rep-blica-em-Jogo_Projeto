using System.Collections;
using System.Collections.Generic;
using Game.Player;
using Game.Territorio;
using UnityEngine;
using System.Linq;

namespace Game
{
    public class RecompensaState : State
    {

        [SerializeField] private AvancoState avancoState;
        private AvancoData avancoData => avancoState.AvancoData;
        private const int recompensaEleitores = 2;
        private int qntdEleitorAplicado = 0;
        public bool TemRecompensa =>  (avancoData.BairrosAdquiridos) > 0 ? true : false;
        public bool AplicouTodosEleitores => qntdEleitorAplicado == recompensaEleitores;
        public PlayerStats PlayerStatsAtual =>  PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual();
        public List<Bairro> bairrosDoPlayer => PlayerStatsAtual?.BairrosInControl;
        

        public override void EnterState()
        {
            Tools.Logger.Instance.LogInfo("Enter State: RECOMPENSA");
            if(!TemRecompensa) {
                CoreLoopStateHandler.Instance.NextStateServerRpc();
                Tools.Logger.Instance.LogInfo("Como não avançou em nenhum bairro, não há recompensa nesta rodada.");
                return;
            } 
            qntdEleitorAplicado = 0;
            HabilitarBairros(true);
            InscreverBairros();

        }

        public override void ExitState()
        {
            Tools.Logger.Instance.LogInfo("Exit RECOMPENSA");
            HabilitarBairros(false);
            DesincreverBairros();
        }

        private void HabilitarBairros(bool value) {
            foreach(Bairro bairro in bairrosDoPlayer) {
                bairro.Interagivel.MudarHabilitado(value);
            }
        }

        private void InscreverBairros() {
            foreach (Bairro bairro in bairrosDoPlayer)
            {
                bairro.Interagivel.click += OnClick;
            }
        }

        private void DesincreverBairros() {
            foreach (Bairro bairro in bairrosDoPlayer)
            {
                bairro.Interagivel.click -= OnClick;
            }
        }

        private void AplicarRecompensa(Bairro bairro) {
            bairro.SetUpBairro.Eleitores.AcrescentaEleitorServerRpc(1);
            qntdEleitorAplicado++;     

        }

        private void OnClick(Bairro bairro) {
            AplicarRecompensa(bairro);

            if(AplicouTodosEleitores)  CoreLoopStateHandler.Instance.NextStateServerRpc();
            
        }








    }

}
