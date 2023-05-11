using System;
using UnityEngine;

namespace ReadyPlayerMe.Multiplayer
{
    public class PlayerData : MonoBehaviour
    {
        public string Name;
        public string AvatarUrl;
        public bool IsPlayer1;
        public float Health;

        public Action<float> HealthChanged;

        public void Damage(float newValue)
        {
            Health = newValue;
            HealthChanged?.Invoke(Health);
        }
    }
}
