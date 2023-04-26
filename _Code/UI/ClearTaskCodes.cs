using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTaskCodes : MonoBehaviour
{
    private void OnEnable() => 
        LevelEvents.NewStateLevelEvent += CheckStateLevel;

    private void CheckStateLevel(StateLevel newState)
    {
        if (newState == StateLevel.comply || newState == StateLevel.fail || newState == StateLevel.restart)
            Clear();
    }

    private void OnDisable() =>
        LevelEvents.NewStateLevelEvent -= CheckStateLevel;

    public void Clear()
    {
        foreach (var item in LevelManager.instance.activeTasks)
            Destroy(item.gameObject);

        UIManager.countActions = 0;
        LevelManager.instance.activeTasks.Clear();
    }
}
