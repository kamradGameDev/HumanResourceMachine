using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ButtonsToChooseFrom : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;

    private void OnEnable() => 
        LevelEvents.NewStateLevelEvent += NewLevelState;

    private void OnDisable() => 
        LevelEvents.NewStateLevelEvent -= NewLevelState;

    private void NewLevelState(StateLevel stateLevel)
    {
        if (stateLevel == StateLevel.start)
            ActiveButtons();
        else if(stateLevel != StateLevel.start && stateLevel != StateLevel.restart)
            foreach (var item in buttons)
                item.SetActive(false);
    }

    private void ActiveButtons()
    {
        for (var i = 0; i < LevelManager.instance.TypeCodeActions.Length; i++)
            if(LevelManager.instance.TypeCodeActions[i].ToString().Contains(buttons[i].name))
                buttons[i].SetActive(true);
    }
}
