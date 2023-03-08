using Unity.Collections;
using Unity.Netcode;

public struct PlayerNetworkData : INetworkSerializable
{
    public FixedString128Bytes Name;
    public FixedString128Bytes AvatarUrl;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Name);
        serializer.SerializeValue(ref AvatarUrl);
    }
}
