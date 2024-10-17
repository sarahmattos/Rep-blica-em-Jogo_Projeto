using Game.Networking;
using Game.Tools;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class IPManager : Singleton<IPManager>
{
    [SerializeField] private TMP_Text text_IP;
    [SerializeField] private List<string> allIpAddress;

    public int portDefault => 8007;

    private void Start()
    {
        text_IP.SetText(myIpAddress());
        allIpAddress = AllIPAddresses();

        OfflineConnection.Instance.conexaoIpEstabelecida += SetIpText;

    }

    private void OnDestroy()
    {
        OfflineConnection.Instance.conexaoIpEstabelecida -= SetIpText;

    }

    private void SetIpText(string ipAddress)
    {
        text_IP.SetText(ipAddress);
    }


    public List<string> AllIPAddresses()
    {
        List<string> ips = new List<string>();
        IPAddress[] ipAddress = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress ip in ipAddress)
        {
            ips.Add(ip.ToString());
        }
        return ips;
    }

    public string myIpAddress()
    {
        IPHostEntry host;
        string localIP = "?";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily.ToString() == "InterNetwork")
            {

                localIP = ip.ToString();
                return localIP;
            }
        }
        return null;
    }

}
