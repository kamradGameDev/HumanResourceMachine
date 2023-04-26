using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

internal abstract class LevelRulesBase : MonoBehaviour
{
    internal virtual UniTask<bool> CheckRules()
    {
        //можно проверять по доставленным предметам, что самое логичное
        Debug.Log("LevelManager.instance.complyTasks: " + LevelManager.instance.complyTasks.Count);
        if (LevelManager.instance.complyTasks.Count == LevelManager.instance.CountActionsToComplyLevel)
        {
            Debug.Log($"Уровень завершен!");
            LevelEvents.NewStateLevel(StateLevel.comply);
            gameObject.SetActive(false);

            return UniTask.FromResult(true);

        }

        return UniTask.FromResult(false);
    }

    public abstract void SendMessage();
}
