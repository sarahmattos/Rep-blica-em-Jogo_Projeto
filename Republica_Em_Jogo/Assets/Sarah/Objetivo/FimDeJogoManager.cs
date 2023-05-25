using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Collections;
using Game.UI;
using Game.Player;
using Game.Territorio;
using System;
using Game.Tools;

namespace Game
{
    public class FimDeJogoManager : NetworkBehaviour
    {
        [SerializeField] private EventosParaFimJogo eventosParaFimJogo;
        private NetworkVariable<FixedString4096Bytes> VitoriaTextServer = new NetworkVariable<FixedString4096Bytes>();
        [SerializeField] private TMP_Text text_vitoria, text_vitoria2;
        [SerializeField] private GameObject vitoriaUi;
        private SetUpZona setUpZona;
        private HudStatsJogador hs;
        public static FimDeJogoManager Instance;
        private PlayerStats ps;
        public bool vitoria;

        public void Start()
        {
            Instance = this;
            setUpZona = GameObject.FindObjectOfType<SetUpZona>();
            hs = FindObjectOfType<HudStatsJogador>();

            eventosParaFimJogo.notify += VerificarObjetivoConcluido;
            eventosParaFimJogo.notify += VerifcarConquistouTodoTerritorio;

            VitoriaTextServer.OnValueChanged += ConfigureUIVitoria;

        }

        private void OnDestroy()
        {
            eventosParaFimJogo.notify -= VerificarObjetivoConcluido;
            eventosParaFimJogo.notify -= VerifcarConquistouTodoTerritorio;

            VitoriaTextServer.OnValueChanged -= ConfigureUIVitoria;


        }

        [ServerRpc(RequireOwnership = false)]
        public void SetMessageFimJogoServerRpc(string textoTotal2)
        {
            VitoriaTextServer.Value = textoTotal2;
        }

        private void ConfigureUIVitoria(FixedString4096Bytes previousValue, FixedString4096Bytes newValue)
        {

            vitoriaUi.SetActive(true);
            if (vitoria)
            {
                text_vitoria.text = "Vitória";
                text_vitoria2.text = newValue.ToString();

            }
            else
            {
                text_vitoria.text = "Derrota";
                text_vitoria2.text = newValue.ToString();
            }
        }


        public void VerificarObjetivoConcluido()
        {
            ps = hs.GetPlayerStats();
            // if (setUpZona.tenhoZona.Count == 0) Debug.Log("Não ganhou ainda!");
            for (int i = 0; i < setUpZona.tenhoZona.Count; i++)
            {
                if (setUpZona.tenhoZona[i].Nome == ps.Objetivo)
                {
                    if (setUpZona.tenhoZona[i].ContaRecursosEducacao() >= 2 && setUpZona.tenhoZona[i].ContaRecursosSaude() >= 2)
                    {
                        string message = GameDataconfig.Instance.TagParticipante + " "
                        + PlayerStatsManager.Instance.GetLocalPlayerStats().PlayerName + " venceu conquistando a zona "
                        + setUpZona.tenhoZona[i].Nome + " com recursos suficientes!";

                        AnuncinarVitória(message);
                    }
                    else
                    {
                        // Debug.Log("Não tem recursos suficientes na zona de objetivo ainda!");
                    }

                }
                // Debug.Log("Não tem zona do objetivo ainda!");
            }

        }

        public void VerifcarConquistouTodoTerritorio()
        {
            if (GameDataconfig.Instance.DevConfig.VenceConquistandoTudo == false) return;
            if (PlayerStatsManager.Instance.GetLocalPlayerStats()?.bairrosTotais == SetUpZona.Instance.AllBairros.Count)
            {
                AnuncinarVitória("Conquista total do território!");
            }
        }

        public void AnuncinarVitória(string message)
        {
            vitoria = true;
            SetMessageFimJogoServerRpc(message);
        }

        public void VoltarMenu()
        {
            //Application.LoadLevel("MainMenuScene");
            Application.Quit();
        }
    }

}
