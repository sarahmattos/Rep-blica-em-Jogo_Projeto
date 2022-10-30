using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

public class BaseClient : MonoBehaviour
{
    public NetworkDriver driver;
    protected NetworkConnection connection;

#if UNITY_EDITOR
    private void Start() { Init(); }
    private void Update() { UpdateServer(); }
    private void OnDestroy() { Shutdown(); }
#endif
    public virtual void Init()
    {
        driver = NetworkDriver.Create();
        connection = default(NetworkConnection);

        NetworkEndPoint endpoint = NetworkEndPoint.LoopbackIpv4; //quuam pode conectar na gnt
        endpoint.Port = 5522;
        connection = driver.Connect(endpoint);

    }
    public virtual void Shutdown()
    {
        driver.Dispose();
    }
    public virtual void UpdateServer()
    {
        driver.ScheduleUpdate().Complete();
        CheckAlive();
        UpdateMessagePump();
    }
    private void CheckAlive()
    {
        if (!connection.IsCreated)
        {
            Debug.Log("perdeu conexão com server");
        }
    }
    protected virtual void UpdateMessagePump()
    {
        DataStreamReader stream;
        
            NetworkEvent.Type cmd;
            while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Connect)
                {
                    Debug.Log("Você está conectado ao server");
                }
            else if (cmd == NetworkEvent.Type.Data)
            {
                uint value = stream.ReadByte();
                Debug.Log("Got the value " + value + " back from the server");
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Cliente got disconnected from the server");
                    connection = default(NetworkConnection);
                }
            }
        
    }
    public virtual void SendToServer(NetMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
}
