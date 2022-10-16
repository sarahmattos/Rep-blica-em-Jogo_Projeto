using Unity.Networking.Transport;
using UnityEngine;

public class NetMessage 
{
    public OpCode Code { set; get; }


    public virtual void Serialize(ref DataStreamWriter writer)
    {

    }
    public virtual void Deserialize()
    {

    }
}
