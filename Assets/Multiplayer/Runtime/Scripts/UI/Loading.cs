using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;

    public void SetActive(bool enable)
    {
        loadingPanel.SetActive(enable);
    }
}
