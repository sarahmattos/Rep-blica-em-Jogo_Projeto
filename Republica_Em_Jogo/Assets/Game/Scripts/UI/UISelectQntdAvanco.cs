using System;
using System.Collections;
using System.Collections.Generic;
using Game.Territorio;
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
            for (int i = 0; i < MaxEleitoresMigrar - 1; i++)
            {
                buttonsOrdered[i].interactable = true;
            }
        }


        //Click dos botoes [1, 2 e 3] da interface
        public void ChooseQntdEleitor(int value)
        {
            migraEleitorAvancoState.MigrarEleitores(value);
        }



    }
}
