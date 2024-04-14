using System.Net;
using System.Net.Sockets;

using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace VelodyneLidarViewer.Driver
{
    using Data;
    using Model;

    public class VelodyneLidarDriver : MonoBehaviour
    {
        [SerializeField]
        private VelodyneSensorModelEnum _model;
        private VelodyneSensorModelData _modelData;

        [SerializeField]
        private int _port = 2368;

        [SerializeField]
        private VelodyneSensorConfig _config = new VelodyneSensorConfig()
        {
            rpm = 600.0f,
            timestampFirstPacket = false
        };

        public delegate void OnReceived();
        public OnReceived onReceived;

        private UdpClient _client;
        private NativeArray<byte> _data;
        private VelodyneScan _scan;

        private int _packetCount;

        private const int VelodyneLidarUdpPacketLength = 1248;
        private const int VelodyneLidarDataPacketLength = 1206;
        private const int VelodyneLidarDataPacketStartIndex = 42;

        public VelodyneScan scan => _scan;

        private void Awake()
        {
            _modelData = VelodyneSensorModelRegistry.GetModel(_model);

            float frequency = _config.rpm / 60.0f;
            _config.npackets = Mathf.CeilToInt(_modelData.packetRate / frequency);

            _client = new UdpClient(_port);
            _data = new NativeArray<byte>(VelodyneLidarDataPacketLength * _config.npackets, Allocator.Persistent);
            
            _scan = new VelodyneScan(_config.npackets);
            for (int i = 0; i < _config.npackets; i++)
            {
                _scan.packets[i].data = new NativeSlice<byte>(_data, i * VelodyneLidarDataPacketLength, VelodyneLidarDataPacketLength);
            }

            _packetCount = 0;
        }

        private void OnEnable()
        {
            _client.BeginReceive(OnUdpReceived, _client);
        }

        private void OnUdpReceived(System.IAsyncResult result)
        {
            UdpClient client = (UdpClient)result.AsyncState;
            IPEndPoint ipEnd = null;

            byte[] newData = client.EndReceive(result, ref ipEnd);

            if(newData.Length != VelodyneLidarUdpPacketLength)
            {
                client.BeginReceive(OnUdpReceived, client);
                return;
            }

            NativeArray<byte> tmpData = new NativeArray<byte>(newData, Allocator.Temp);
            unsafe
            {
                void* dstPtr = NativeArrayUnsafeUtility.GetUnsafePtr(_data);
                void* srcPtr = NativeArrayUnsafeUtility.GetUnsafePtr(tmpData);

                dstPtr = (byte*)dstPtr + VelodyneLidarDataPacketLength * _packetCount;
                srcPtr = (byte*)srcPtr + VelodyneLidarDataPacketStartIndex;

                UnsafeUtility.MemCpy(dstPtr, srcPtr, VelodyneLidarDataPacketLength * UnsafeUtility.SizeOf<byte>());
            }
            
            if (++_packetCount >= _config.npackets)
            {
                _packetCount = 0;
            }

            client.BeginReceive(OnUdpReceived, client);
        }

        private void OnDisable()
        {
            _client.Close();
        }

        private void OnDestroy()
        {
            _data.Dispose();
        }
    }
}
