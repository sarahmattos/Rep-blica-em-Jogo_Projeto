using Game.Tools;
using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Game.Networking
{
    public class OnlineRelayManager : Singleton<OnlineRelayManager>
    {
        [SerializeField] private string environment = "production";
        public Action<string> joinCodeGenerated;
        public Action<bool> connecting;

        public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
        public bool IsRelayEnalbed => Transport != null && Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;
        
        public async Task<RelayHostData>  SetupRelay()
        {
            connecting?.Invoke(true);

            //Conectando e autenticando o jogador
            InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);
            await UnityServices.InitializeAsync(options);
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            //Gerando e alocando os dados da partida
            Allocation allocation = await Relay.Instance.CreateAllocationAsync(GameDataconfig.Instance.MaxConnections);

            RelayHostData relayHostData = new RelayHostData
            {
                Key = allocation.Key,
                Port = (ushort)allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                IPv4Address = allocation.RelayServer.IpV4,
                ConnectionData = allocation.ConnectionData
            };

            relayHostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.AllocationID);

            //Enviando os dados.
            Transport.SetRelayServerData(relayHostData.IPv4Address, relayHostData.Port, relayHostData.AllocationIDBytes,
                relayHostData.Key, relayHostData.ConnectionData);

            Tools.Logger.Instance.LogInfo($"AllocationID: {relayHostData.AllocationID}");
            Tools.Logger.Instance.LogInfo($"C�digo da sala: {relayHostData.JoinCode}");
            
            joinCodeGenerated?.Invoke(relayHostData.JoinCode);
            connecting?.Invoke(false);
            return relayHostData;



        }


        public async Task<RelayJoinData> JoinRelay(string joinCode)
        {
            connecting?.Invoke(true);

            //Conectando e autenticando o jogador

            InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);
            await UnityServices.InitializeAsync(options);
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            //Alocando pelo joinCode fornecido.
            JoinAllocation joinAllocation =await Relay.Instance.JoinAllocationAsync(joinCode);
            RelayJoinData relayJoinData = new RelayJoinData
            {
                Key = joinAllocation.Key,
                Port = (ushort)joinAllocation.RelayServer.Port,
                AllocationID = joinAllocation.AllocationId,
                AllocationIDBytes = joinAllocation.AllocationIdBytes,
                ConnectionData = joinAllocation.ConnectionData,
                HostConnectionData = joinAllocation.HostConnectionData,
                IPv4Address = joinAllocation.RelayServer.IpV4,
                JoinCode = joinCode
            };

            //Verificando e conectando..
            Transport.SetRelayServerData(relayJoinData.IPv4Address, relayJoinData.Port, relayJoinData.AllocationIDBytes,
                relayJoinData.Key, relayJoinData.ConnectionData, relayJoinData.HostConnectionData);


           Tools.Logger.Instance.LogInfo($"Connectado: {joinCode}");
            connecting?.Invoke(false);

            return relayJoinData;
        }

    }

}
