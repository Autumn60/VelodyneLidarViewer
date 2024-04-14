using Unity.Collections;

namespace VelodyneLidarViewer.Data
{
    public class VelodyneScan
    {
        public VelodynePacket[] packets;

        public VelodyneScan(int npackets)
        {
            packets = new VelodynePacket[npackets];
        }
    }
}