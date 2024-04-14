using System;
using UnityEngine;

namespace VelodyneLidarViewer
{
    [Serializable]
    public struct VelodyneSensorConfig
    {
        public float rpm;
        public bool timestampFirstPacket;
        [HideInInspector]
        public int npackets;
    }
}
