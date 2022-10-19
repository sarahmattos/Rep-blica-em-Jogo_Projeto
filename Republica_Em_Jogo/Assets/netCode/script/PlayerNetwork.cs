using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;


public class PlayerNetwork : NetworkBehaviour
{
    
    public string resposta;
    private NetworkManagerUI netManager;

    public void Start()
    {
        this.netManager = GameObject.FindObjectOfType<NetworkManagerUI>();
    }
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int =56,
            _bool=false,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<FixedString128Bytes> testfloat = new NetworkVariable<FixedString128Bytes>("oi", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData :INetworkSerializable
    {
        public int _int;
        public bool _bool;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T: IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; randomNumber: " + newValue._int+"; "+newValue._bool);
            netManager.chatView.text = newValue._int.ToString();
            
        };
        testfloat.OnValueChanged += (FixedString128Bytes previousValue, FixedString128Bytes newValue) =>
        {
            //string aux = "Cliente "+OwnerClientId +" disse: " +newValue.ToString();
            Debug.Log(OwnerClientId + "; testfloat: " + newValue );
            netManager.chatView.text = "Cliente " + OwnerClientId + " disse: " + newValue.ToString();

        };
    }

    private void Update()
    {
        
        //chatInput = GameObject.FindGameObjectsWithTag("inputField");
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            resposta = netManager.chatInput.text;
            testfloat.Value = resposta;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)){
            resposta = netManager.chatInput.text;
            //Debug.Log(resposta);
            randomNumber.Value = new MyCustomData{
                _int = int.Parse(resposta),
                 _bool = true,
            };
            
        }
        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed=3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
