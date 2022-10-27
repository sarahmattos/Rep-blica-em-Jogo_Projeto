using Unity.Netcode;
using UnityEngine;

namespace Game.Networking.Data
{
    public static class CustomNetworkData
    {

        public struct NetworkColor : INetworkSerializable
        {
            public float r;
            public float g;
            public float b;
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref r);
                serializer.SerializeValue(ref g);
                serializer.SerializeValue(ref b);
            }
        }

    }

}
