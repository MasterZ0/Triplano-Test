using UnityEngine;
using System.Linq;

namespace TriplanoTest.Gameplay.Level
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private RoomConnection[] connections;

        public RoomConnection GetConnection(string connectionName)
        {
            return connections.First(c => c.ConnectionName == connectionName);
        }
    }
}