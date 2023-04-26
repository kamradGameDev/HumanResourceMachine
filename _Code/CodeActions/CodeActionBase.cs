using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public enum TypeMainCodeTask
{
    Default, Jump
}

internal abstract class CodeTaskBase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indexTaskText;
    [SerializeField] protected int indexTask;
    internal int IndexTask => indexTask;

    internal abstract UniTask StartTaskCode(TypeMainCodeTask typeMainCodeTask);

    internal virtual void InitializeTask(int index = 0)
    {
        indexTask = index;
        indexTaskText.text = (indexTask + 1).ToString();
    }

    internal void ChangeIndexTask(int newIndex)
    {
        indexTask = newIndex;
        indexTaskText.text = (indexTask + 1).ToString();
    }
}
