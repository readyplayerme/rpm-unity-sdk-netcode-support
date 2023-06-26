using Unity.Collections;
using Unity.Netcode;

public struct PlayerData : INetworkSerializable
{
    public int Position;
    public string Url;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Position);
        serializer.SerializeValue(ref Url);
    }
}
