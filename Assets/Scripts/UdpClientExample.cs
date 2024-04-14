using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class UdpClientExample : MonoBehaviour
{
    [SerializeField]
    private string _host = "127.0.0.1";
    [SerializeField]
    private int _port = 2368;

    private UdpClient _client;

    private void Awake()
    {
        _client = new UdpClient();
    }

    private void OnEnable()
    {
        _client.Connect(_host, _port);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var message = Encoding.UTF8.GetBytes("Hello World!");
            _client.Send(message, message.Length);
        }
    }

    private void OnDisable()
    {
        _client.Close();
    }
}