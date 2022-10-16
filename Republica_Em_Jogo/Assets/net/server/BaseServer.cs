using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

public class BaseServer : MonoBehaviour
{
    public NetworkDriver driver;
    protected NativeList<NetworkConnection> connections;

#if UNITY_EDITOR
    private void Start() { Init(); }
    private void Update() { UpdateServer(); }
    private void OnDestroy() { Shutdown(); }
#endif
    public virtual void Init()
    {
        driver = NetworkDriver.Create();
        NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4; //quuam pode conectar na gnt
        endpoint.Port = 5522;
        if (driver.Bind(endpoint) != 0)
        {
            Debug.Log("erro ao conectar a porta " + endpoint.Port);
        }
        else
        {
            driver.Listen();
        }
        //inicio lista conexão
        connections = new NativeList<NetworkConnection>(4, Allocator.Persistent);//max

    }
    public virtual void Shutdown()
    {
        driver.Dispose();
        connections.Dispose();
    }
    public virtual void UpdateServer()
    {
        driver.ScheduleUpdate().Complete();
        CleanupConnections();
        AcceptNewConnections();
        UpdateMessagePump();
    }
    private void CleanupConnections()
    {
        for (int i = 0; i < connections.Length; i++)
        {
            connections.RemoveAtSwapBack(i);
            --i;
        }
    }
    private void AcceptNewConnections()
    {
        NetworkConnection c;
        while((c=driver.Accept())!= default(NetworkConnection))
         {
            connections.Add(c);
            Debug.Log("Accept connection");
         }
    }
    protected virtual void UpdateMessagePump()
    {
        DataStreamReader stream;
        for (int i = 0; i < connections.Length; i++)
        {
            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connections[i],out stream)) !=NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    uint number = stream.ReadByte();
                    Debug.Log("Got " + number + " from the Client");
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Cliente disconnect from the server");
                    connections[i] = default(NetworkConnection);
                }
            }
        }
    }
}
