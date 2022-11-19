using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction2 : NetworkBehaviour
{
    private TMP_Text text;
    [SerializeField] private Camera cam;
    private NetworkVariable<int> nextInt = new NetworkVariable<int>
    (
        1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner
    );


    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        cam = FindObjectOfType<Camera>();

    }
    private void OnEnable()
    {
        nextInt.OnValueChanged += UpdateText;
    }


    private void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            nextInt.Value += 1;
        }

        //transform.LookAt(transform.position + cam.transform.forward);

    }


    public void UpdateText(int previousValue, int nextValue)
    {
        text.text = nextValue.ToString();
    }


}
