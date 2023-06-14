using UnityEngine;

namespace ReadyPlayerMe.Multiplayer
{
    public class Loading : MonoBehaviour
    {
        [SerializeField] private GameObject loadingPanel;

        public void SetActive(bool enable)
        {
            loadingPanel.SetActive(enable);
        }
    }
}
