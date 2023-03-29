using Unity.Netcode;
using UnityEngine;
using TMPro;
using Logger = Game.Tools.Logger;
using Game.Tools;
using System;

namespace Game.Networking { 
public class OnlineConnection : Singleton<OnlineConnection>
{

        [SerializeField] private TMP_InputField inputJoinCode;

        public Action<bool> conexaoEstabelecida;
        public Action<string> joinCodeConexaoEstabelecida;
        async public void CreateGame()
        {
            if(OnlineRelayManager.Instance.IsRelayEnalbed)
            {
                await OnlineRelayManager.Instance.SetupRelay();
            }

            if(NetworkManager.Singleton.StartHost())
            {
                conexaoEstabelecida?.Invoke(true);                
            } else
            {
                conexaoEstabelecida?.Invoke(false);
            }
        }

        async public void JoinGame()
        {
            if (OnlineRelayManager.Instance.IsRelayEnalbed && !string.IsNullOrEmpty(inputJoinCode.text))
            {
                await OnlineRelayManager.Instance.JoinRelay(inputJoinCode.text);
            } else
            {
                conexaoEstabelecida?.Invoke(false);

            }


            if (NetworkManager.Singleton.StartClient())
            {
                conexaoEstabelecida?.Invoke(true);
                joinCodeConexaoEstabelecida?.Invoke(inputJoinCode.text);
                Logger.Instance.LogInfo(string.Concat("Entrando na sala: ",inputJoinCode.text));
            }
            else
            {
                conexaoEstabelecida?.Invoke(false);
                Logger.Instance.LogInfo(string.Concat("Falha ao entrar na sala: ",inputJoinCode.text));
            }

        }



    #region last implementation
        //private OnlineConnection instance;
        //public OnlineConnection Instance { get => instance; }

        //private void Awake()
        //{
        //    instance = this;
        //}
        //private void Start()
        //{
        //    AuthenticatingPlayer();
        //    #region para lembrar
        //    //    AuthenticationService.Instance.SignedIn += () => {
        //    //    if (UnityServices.InitializeAsync().IsCompleted)
        //    //        StartCoroutine(ConfigureTransportAndStartNgoAsHost());
        //    //};

        //    NetworkManager.Singleton.OnServerStarted += () =>{   };
        //    NetworkManager.Singleton.OnClientConnectedCallback += (ulong message) => {  };
        //    #endregion depois
        //}

        //public async void AuthenticatingPlayer()
        //{
        //    var playerID = "";
        //    try
        //    {
        //        await UnityServices.InitializeAsync();
        //        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //        playerID = AuthenticationService.Instance.PlayerId;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.Log(e);
        //    }
        //    Logger.Instance.LogError("PlayerID: " + playerID);
        //}


        ////CREATE AN ALLOCATION AND REQUEST A JOIN CODE - USADO EM CONFIGURE THE TRANSPORT AND START NGO
        //public static async Task<(string ipv4address, ushort port, byte[] allocationIdBytes, byte[] connectionData, byte[] key, string joinCode)> 
        //    AllocateRelayServerAndGetJoinCode(int maxConnections, string region = null)
        //{
        //    Allocation allocation;
        //    string createJoinCode;
        //    try
        //    {
        //        allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, region);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError($"Relay create allocation request failed {e.Message}");
        //        throw;
        //    }

        //    Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        //    Debug.Log($"server: {allocation.AllocationId}");

        //    try
        //    {
        //        createJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        //    }
        //    catch
        //    {
        //        Debug.LogError("Relay create join code request failed");
        //        throw;
        //    }

        //    var dtlsEndpoint = allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");
        //    return (dtlsEndpoint.Host, (ushort)dtlsEndpoint.Port, allocation.AllocationIdBytes, allocation.ConnectionData, allocation.Key, createJoinCode);
        //}


        ////CONFIGURE THE TRANSPORT AND START NGO
        //IEnumerator ConfigureTransportAndStartNgoAsHost()
        //{
        //    var serverRelayUtilityTask = AllocateRelayServerAndGetJoinCode(m_MaxConnections);
        //    while (!serverRelayUtilityTask.IsCompleted)
        //    {
        //        yield return null;
        //    }
        //    if ( serverRelayUtilityTask.IsFaulted)
        //    {
        //        Logger.Instance.LogError("Erro ao tentar criar partida: "+ serverRelayUtilityTask.Exception.Message);
        //        yield break;
        //    }

        //    var (ipv4address, port, allocationIdBytes, connectionData, key, joinCode) = serverRelayUtilityTask.Result;

        //    // The .GetComponent method returns a UTP NetworkDriver (or a proxy to it)
        //    NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(ipv4address, port, allocationIdBytes, key, connectionData, true);
        //    if (NetworkManager.Singleton.StartHost())
        //    {            
        //        relayJoinCode = joinCode.ToString();
        //        TextJoinCode.SetText(relayJoinCode);
        //    }
        //    yield return null;
        //}


        ////JOIN AN ALLOCATION
        //public static async Task<(string ipv4address, ushort port, byte[] allocationIdBytes, byte[] connectionData, byte[] hostConnectionData, byte[] key)>
        //    JoinRelayServerFromJoinCode(string joinCode)
        //{
        //    JoinAllocation allocation;
        //    try
        //    {
        //        allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        //    }
        //    catch
        //    {
        //        Debug.LogError("Relay join request failed");
        //        throw;
        //    }

        //    Debug.Log($"client connection data: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        //    Debug.Log($"host connection data: {allocation.HostConnectionData[0]} {allocation.HostConnectionData[1]}");
        //    Debug.Log($"client allocation ID: {allocation.AllocationId}");

        //    var dtlsEndpoint = allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");
        //    return (dtlsEndpoint.Host, (ushort)dtlsEndpoint.Port, allocation.AllocationIdBytes, allocation.ConnectionData, allocation.HostConnectionData, allocation.Key);
        //}

        ////CONFIGURE THE TRNASPORT AND START NGO AS A JOINING PLAYER
        //IEnumerator ConfigureTransportAndStartNgoAsClient()
        //{
        //    Logger.Instance.LogInfo("joincode usado: " + inputFieldJoinCode.text);
        //    // Populate RelayJoinCode beforehand through the UI
        //    var clientRelayUtilityTask = JoinRelayServerFromJoinCode(inputFieldJoinCode.text);

        //    while (!clientRelayUtilityTask.IsCompleted)
        //    {
        //        yield return null;
        //    }

        //    if (clientRelayUtilityTask.IsFaulted)
        //    {
        //        Debug.LogError("Exception thrown when attempting to connect to Relay Server. Exception: " + clientRelayUtilityTask.Exception.Message);
        //        yield break;
        //    }

        //    var (ipv4address, port, allocationIdBytes, connectionData, hostConnectionData, key) = clientRelayUtilityTask.Result;

        //    NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(ipv4address, port, allocationIdBytes, key, connectionData, hostConnectionData, true);

        //    if (NetworkManager.Singleton.StartClient())
        //    {
        //        var result = new StringBuilder();
        //        foreach (var item in hostConnectionData)
        //        {
        //            result.Append(item);

        //        }
        //    }
        //    yield return null;
        //}




        ////Host button click
        //public void HostGame()
        //{
        //    StartCoroutine(ConfigureTransportAndStartNgoAsHost());
        //}

        ////Join button click
        //public void JoinGame()
        //{
        //    StartCoroutine(ConfigureTransportAndStartNgoAsClient());
        //}


        //private void OnGUI()
        //{
        //    if (GUI.Button(new Rect(10, 10, 100, 20), "Start CLIENT")) NetworkManager.Singleton.StartClient();
        //    if (GUI.Button(new Rect(10, 30, 100, 20), "Start SERVER")) NetworkManager.Singleton.StartServer();
        //    if (GUI.Button(new Rect(10, 50, 100, 20), "Start HOST")) NetworkManager.Singleton.StartHost();
        //}

        #endregion
}

}
