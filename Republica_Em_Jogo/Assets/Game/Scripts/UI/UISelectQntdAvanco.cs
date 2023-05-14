using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UISelectQntdAvanco : MonoBehaviour
    {
        private Animator animator;

        [SerializeField] private GameObject[] buttonsOrdered;
        [SerializeField] private State migraEleitorState;
        [SerializeField] private State AvancoState;
        public event Action<int> quantidadeEscolhida;



        private void Awake()
        {
            animator = GetComponent<Animator>();
        }



        public void ChooseQntdEleitor(int value)
        {
            quantidadeEscolhida.Invoke(value);
        }



    }
}
