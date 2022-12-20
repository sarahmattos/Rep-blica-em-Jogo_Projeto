using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Territorio
{
public class SetUpZona : MonoBehaviour
{
    public static SetUpZona instance;
    private ZonaTerritorial[] zonas;
    public ZonaTerritorial[] ZonaTerritorial => zonas;
    void Start()
    {
        instance= this;
    }

    private void Awake()
        {
            zonas = GetComponentsInChildren<ZonaTerritorial>();
            
        }
        public void eleitoresZona(int valor, string nome){
            foreach(ZonaTerritorial zona in zonas)
            {
                if(zona.Nome==nome){
                    zona.adicionarEleitoresZona(valor);
                }
            }

        }
        public void chamar(){
            eleitoresZona(2,"Oeste1");
        }
        
}
}
