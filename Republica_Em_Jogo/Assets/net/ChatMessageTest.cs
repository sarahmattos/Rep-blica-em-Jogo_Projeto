using TMPro;
using UnityEngine;

public class ChatMessageTest : MonoBehaviour
{
    [SerializeField] private TMP_InputField chatInput;


    public void OnSubmitClick()
    {
        Net_ChatMessage msg = new Net_ChatMessage();
        FindObjectOfType<BaseClient>().SendToServer(msg);
    }
}
