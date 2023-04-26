using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class StateObjBase : MonoBehaviour
{
    protected abstract void SaveStartState();
    protected abstract void RestartState();

    private void OnEnable() => 
        LevelEvents.NewStateLevelEvent += NewStateLevel;

    private void OnDisable() =>
        LevelEvents.NewStateLevelEvent -= NewStateLevel;

    private void NewStateLevel(StateLevel newStateLevel)
    {
        if (newStateLevel == StateLevel.loading)
            SaveStartState();

        if (newStateLevel == StateLevel.restart || newStateLevel == StateLevel.preLoading)
            RestartState();

    }
}
