using Game.Territorio;
using Game.Tools;

namespace Game
{
    public class SelectBairroRemanejamentoState : State
    {
        private RemanejamentoState remanejamentoState;
        private RemanejamentoData remanejamentoData;

        private void Start()
        {
            remanejamentoState = GetComponentInParent<RemanejamentoState>();
            remanejamentoData = remanejamentoState.RemanejamentoData;
        }

        public override void EnterState()
        {
            remanejamentoData.ResetSelectedBairros();
            if (TurnManager.Instance.LocalIsCurrent)
            {
                remanejamentoData.ParBairroEleitorigualUm.Keys.MudarHabilitado(true);
                remanejamentoData.ParBairroEleitorigualUm.Keys.MudarInativity(false);
                remanejamentoData.BairrosNaoRemanejaveis.MudarInativity(true);
                InscreverClickInteragivelBairros();
                GameStateEmitter.SendMessage("Selecione um bairro.");
            }



        }

        public override void ExitState()
        {
            if (!TurnManager.Instance.LocalIsCurrent) return;
            remanejamentoData.ParBairroEleitorigualUm.Keys.MudarHabilitado(false);
            DesinscreverClickInteragivelBairros();

        }

        private void InscreverClickInteragivelBairros()
        {
            foreach (Bairro bairro in remanejamentoData.ParBairroEleitorigualUm.Keys)
            {
                bairro.Interagivel.Click += OnBairroClicado;
            }
        }


        private void DesinscreverClickInteragivelBairros()
        {
            foreach (Bairro bairro in remanejamentoData.ParBairroEleitorigualUm.Keys)
            {
                bairro.Interagivel.Click -= OnBairroClicado;
            }
        }

        private void OnBairroClicado(Bairro bairro)
        {
            bairro.Interagivel.ChangeSelectedBairro(true);
            remanejamentoData.BairroEscolhido = bairro;
            remanejamentoState.StateMachineController.NextStateServerRpc();
        }


    }
}
