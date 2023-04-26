using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

internal class UIManager : MonoBehaviour
{
    internal static UIManager instance { get; private set; }

    internal static int countActions;

    [SerializeField] private Canvas mainCanvas;
    internal Canvas MainCanvas => mainCanvas;

    [SerializeField] private TextMeshProUGUI nameText, descText;

    [SerializeField] private Transform actionsCodeContent;
    internal Transform ActionsCodeContent => actionsCodeContent;

    [SerializeField] private GameObject actionCodePrefab;
    internal GameObject ActionCodePrefab => actionCodePrefab;

    [SerializeField] private Button startActionsCode;
    internal Button StartActionsCode => startActionsCode;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    private void OnEnable() =>
        LevelEvents.NewStateLevelEvent += NewStateLevel;

    private void OnDisable() =>
        LevelEvents.NewStateLevelEvent -= NewStateLevel;

    private void NewStateLevel(StateLevel newStateLevel)
    {
        if(newStateLevel == StateLevel.loading)
        {
            nameText.text = LevelManager.instance.LevelData.NameLevel;
            descText.text = LevelManager.instance.LevelData.DescLevel;
        }
        else if(newStateLevel == StateLevel.comply || newStateLevel == StateLevel.fail || newStateLevel == StateLevel.preLoading)
        {
            nameText.text = "";
            descText.text = "";
        }
    }

    internal void RecalculationIndexes()
    {
        for(var i = 0; i < LevelManager.instance.activeTasks.Count; i++)
            LevelManager.instance.activeTasks[i].InitializeTask(i);
    }

    internal void SwapActiveCodeActionButtons() =>
        LevelManager.instance.activeTasks = LevelManager.instance.activeTasks.OrderBy(s => s.IndexTask).ToList();
}
