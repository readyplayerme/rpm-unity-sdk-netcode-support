using Unity.Netcode.Components;

namespace ReadyPlayerMe.NetcodeSupport
{
    public class ClientNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative() => false;
    }
}
