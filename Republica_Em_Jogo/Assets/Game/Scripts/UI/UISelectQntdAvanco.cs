using System;
using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
using Game.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UISelectQntdAvanco : MonoBehaviour
    {
        private Animator animator;
        [SerializeField] private Button[] buttonsOrdered;
        [SerializeField] private MigraEleitorAvancoState migraEleitorAvancoState;
        private int MaxEleitoresMigrar => migraEleitorAvancoState.MaxQntdEleitoresMigrar;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            migraEleitorAvancoState.MigraEleitores += EnableButtonsByEleitoresDisponiveis;
            migraEleitorAvancoState.MigraEleitores += PlayEnterAnim;
            migraEleitorAvancoState.MigrouEleitores += PlayExitAnim;
            PlayExitAnim(0, new Bairro());

        }


        private void OnDestroy()
        {
            migraEleitorAvancoState.MigraEleitores -= EnableButtonsByEleitoresDisponiveis;
            migraEleitorAvancoState.MigraEleitores -= PlayEnterAnim;
            migraEleitorAvancoState.MigrouEleitores -= PlayExitAnim;

        }

        private void PlayEnterAnim()
        {
            animator.Play("MostrarUiSelectEleitorMigrar");

        }

        private void PlayExitAnim(int _, Bairro __)
        {
            animator.Play("EsconderUiSelectEleitorMigrar");

        }

        private void EnableButtonsByEleitoresDisponiveis()
        {
            for (int i = 0; i < 3; i++)
            {
                bool podeAtivarButtonByIndex = i < (MaxEleitoresMigrar);
                buttonsOrdered[i].interactable = podeAtivarButtonByIndex;
            }
        }


        //Click dos botoes [1, 2 e 3] da interface
        public void ChooseQntdEleitor(int value)
        {
            migraEleitorAvancoState.MigrarEleitores(value);
        }



    }
}
