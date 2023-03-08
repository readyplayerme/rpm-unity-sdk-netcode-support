using UnityEngine;
using UnityEngine.UI;

public class UILogger : MonoBehaviour
{
    [SerializeField] private Text text;

    private void OnEnable()
    {
        Application.logMessageReceived += OnLogReceived;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= OnLogReceived;
    }

    private void OnLogReceived(string condition, string stacktrace, LogType type)
    {
        text.text += condition + "\n";
    }
}
