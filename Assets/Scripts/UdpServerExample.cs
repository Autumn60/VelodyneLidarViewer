using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpServerExample : MonoBehaviour
{
    [SerializeField]
    private int _port = 2368;

    private UdpClient _client;

    private void Awake()
    {
        _client = new UdpClient(_port);
    }

    private void OnEnable()
    {
        _client.BeginReceive(OnReceived, _client);
    }

    private void OnReceived(System.IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);

        Debug.Log(Encoding.UTF8.GetString(getByte));

        getUdp.BeginReceive(OnReceived, getUdp);
    }

    private void OnDisable()
    {
        _client.Close();
    }
}
