using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;


class MessageScreen : MonoBehaviour
{
    public static MessageScreen instance { get; private set; }
    [SerializeField] private TextMeshProUGUI messageText;

    private void Awake() => instance = this;

    public async void NewMessageScreen(string message, Color color)
    {
        messageText.color = color;
        messageText.text = message;

        await UniTask.Delay(20000);

        messageText.text = "";
    }

}
