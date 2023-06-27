using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.NetcodeSupport
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private Text player1NameField;
        [SerializeField] private Text player2NameField;
        [SerializeField] private Slider player1Health;
        [SerializeField] private Slider player2Health;

        public void SetPlayerName(bool isPlayer1, string playerName)
        {
            if (isPlayer1)
            {
                player1NameField.text = playerName;
            }
            else
            {
                player2NameField.text = playerName;
            }
        }

        public void SetHealth(bool isPlayer1, float health)
        {
            if (isPlayer1)
            {
                player1Health.value = health;

            }
            else
            {
                player2Health.value = health;
            }
        }
    }
}
