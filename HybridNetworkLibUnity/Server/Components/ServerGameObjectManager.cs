using System.Collections.Generic;
using HybridNetworkLibUnity.Common;
using UnityEngine;

namespace HybridNetworkLibUnity.Server.Components
{
    public class ServerGameObjectManager : MonoBehaviour
    {
        private const int IdLowBound = byte.MaxValue; // Send a byte id packet if the (int) id is lower
        private const int IdHighBound = IdLowBound + ushort.MaxValue; // Send a ushort packet otherwise where if ushort = 0, the int id is IdLowBound

        private Queue<int> _availableId;
        private ulong _nextUniqueId;

        public NetworkInstanceId GetNextId()
        {
            if (_availableId.TryPeek(out int result))
            {
                
            }

            return default(NetworkInstanceId);
        }
        
        /// <summary>
        /// Spawn an *existing* and *registered* GameObject on all clients.
        /// This is different from Instantiate(), the passed in object must be in a scene and NOT a prefab
        /// </summary>
        /// <param name="obj"></param>
        public void Spawn(GameObject obj)
        {
            
        }
    }
}