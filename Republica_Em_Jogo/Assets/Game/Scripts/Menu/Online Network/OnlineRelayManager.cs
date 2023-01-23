using Game.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Game
{
    public class OnlineRelayManager : Singleton<OnlineRelayManager>
    {
        [SerializeField] private string environment = "production";
        public Action<string> joinCodeGenerated;


        public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
        public bool IsRelayEnalbed => Transport != null && Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;
        
        public async Task<RelayHostData>  SetupRelay()
        {
            Tools.Logger.Instance.LogInfo("Iniciando servidor...");

            InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);
            await UnityServices.InitializeAsync(options);

            if(!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

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

            Transport.SetRelayServerData(relayHostData.IPv4Address, relayHostData.Port, relayHostData.AllocationIDBytes,
                relayHostData.Key, relayHostData.ConnectionData);

            Tools.Logger.Instance.LogInfo($"AllocationID: {relayHostData.AllocationID}");
            Tools.Logger.Instance.LogInfo($"Código da sala: {relayHostData.JoinCode}");
            
            
            joinCodeGenerated?.Invoke(relayHostData.JoinCode);
            return relayHostData;



        }


        public async Task<RelayJoinData> JoinRelay(string joinCode)
        {
            InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);
            await UnityServices.InitializeAsync(options);

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

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

            Transport.SetRelayServerData(relayJoinData.IPv4Address, relayJoinData.Port, relayJoinData.AllocationIDBytes,
    relayJoinData.Key, relayJoinData.ConnectionData, relayJoinData.HostConnectionData);


           Tools.Logger.Instance.LogInfo($"Connectado: {joinCode}");

            return relayJoinData;
        }


        //TODO
        private void TrySignIn()
        {

        }


    }

}
