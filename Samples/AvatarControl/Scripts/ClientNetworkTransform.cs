using Unity.Netcode.Components;

namespace ReadyPlayerMe.NetcodeSupport
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative() => false;
    }
}
