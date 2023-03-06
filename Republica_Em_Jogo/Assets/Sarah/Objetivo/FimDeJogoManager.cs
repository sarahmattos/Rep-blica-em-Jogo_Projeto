using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Territorio;

namespace Game
{
    public class FimDeJogoManager : MonoBehaviour
    {
        public List<ZonaTerritorial> tenhoBairro;
        public static FimDeJogoManager Instance;
        public void Start(){
            Instance = this;
        }
        public void TenhoUmaZonaInteria(List<ZonaTerritorial> _zonaObtidaPorCompleto){
            for(int i=0;i<+_zonaObtidaPorCompleto.Count;i++){
                Debug.Log("Tenho a zona "+_zonaObtidaPorCompleto[i].Nome+" inteira!");
                tenhoBairro.Add(_zonaObtidaPorCompleto[i]);
            }
            
        }
    }
}
