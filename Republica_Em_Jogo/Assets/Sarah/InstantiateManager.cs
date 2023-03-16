using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InstantiateManager : MonoBehaviour
    {
        public static InstantiateManager Instance;
        // Start is called before the first frame update
        Image img;
        void Start()
        {
            Instance=this;
        }

        public GameObject instanciarUi(GameObject go, Transform _pai,Color cor){
            GameObject _go = Instantiate(go,go.transform.position,go.transform.rotation);
            _go.transform.SetParent(_pai, false);
             img = _go.GetComponent<Image>();  
            img.color=cor;
            return _go;
        }
    }
}
