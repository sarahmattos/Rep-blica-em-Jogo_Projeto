using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Player;
using Game.Territorio;
using Game.Tools;
using UnityEngine;

namespace Game
{
    // [System.Serializable]
    public class RemanejamentoData
    {

        private Dictionary<Bairro, int> bairrosRemanejaveis = new();
        public List<Bairro> BairrosPlayerAtual => PlayerStatsManager.Instance.GetPlayerStatsDoPlayerAtual().BairrosInControl;

        private Bairro bairroEscolhido;
        private Bairro vizinhoEscolhido;

        public Dictionary<Bairro, int> ParBairroEleitorigualUm => bairrosRemanejaveis;
        public Bairro BairroEscolhido { get => bairroEscolhido; set => bairroEscolhido = value; }
        public Bairro VizinhoEscolhido { get => vizinhoEscolhido; set => vizinhoEscolhido = value; }

        public List<Bairro> BairrosNaoRemanejaveis
        {
            get
            {
                return BairrosPlayerAtual.Except(ParBairroEleitorigualUm.Keys).ToList();
            }
        }


        public void ClearData()
        {
            bairrosRemanejaveis.Clear();
            ResetSelectedBairros();
        }

        public void ResetSelectedBairros()
        {
            bairroEscolhido?.Interagivel.ChangeSelectedBairro(false);
            VizinhoEscolhido?.Interagivel.ChangeSelectedBairro(false);
            BairroEscolhido = null;
            VizinhoEscolhido = null;
        }

        public void ArmazenarBairrosRemanejaveis()
        {
            foreach (Bairro bairro in BairrosPlayerAtual)
            {
                if (bairro.TemVizinhoPlayerIDIgual() && bairro.EleitoresMenorQue1())
                    bairrosRemanejaveis.Add(bairro, bairro.SetUpBairro.Eleitores.contaEleitores - 1);
            }
        }

        public void RemoveBairroParBairroEleitor(Bairro bairro)
        {
            bairrosRemanejaveis.Remove(bairro);
        }


        public int GetEleitor(Bairro bairro)
        {
            return ParBairroEleitorigualUm[bairro];


        }





    }
}
