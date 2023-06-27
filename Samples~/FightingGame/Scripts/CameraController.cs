using System.Collections.Generic;
using UnityEngine;

namespace ReadyPlayerMe.NetcodeSupport
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] public float offsetY = 1.5f;
        [SerializeField] public float minZ = -10;
        [SerializeField] public float maxZ = -5;

        public List<Transform> players;

        private void LateUpdate()
        {
            if (players.Count < 2)
            {
                return;
            }

            // Calculate the midpoint between the two players
            var midpoint = GetMidpoint();

            // Calculate the distance between the two players
            var distance = GetDistance();

            var posZ = midpoint.z - distance;
            posZ = Mathf.Clamp(posZ, minZ, maxZ);

            // Set the camera's position to the midpoint between the two players, with an offset on the z-axis
            transform.position = new Vector3(midpoint.x, offsetY, posZ);
        }

        private Vector3 GetMidpoint()
        {
            var midpoint = Vector3.zero;
            foreach (var player in players)
            {
                midpoint += player.position;
            }
            midpoint /= players.Count;
            return midpoint;
        }

        private float GetDistance()
        {
            var distance = 0f;
            for (var i = 0; i < players.Count - 1; i++)
            {
                for (var j = i + 1; j < players.Count; j++)
                {
                    var tempDistance = Vector3.Distance(players[i].position, players[j].position);
                    if (tempDistance > distance)
                    {
                        distance = tempDistance;
                    }
                }
            }
            return distance;
        }
    }
}
