using System;
using UnityEngine;

namespace ReadyPlayerMe.NetcodeSupport
{
    public class PlayerData
    {
        public string Name;
        public bool IsPlayer1;
        public float Health;
        
        public Transform Transform;

        public Action<float> HealthChanged;

        public void Damage(float newValue)
        {
            Health = newValue;
            HealthChanged?.Invoke(Health);
        }
    }
}
