using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

internal enum StateLevel
{
    menuLevels, preLoading, loading, start, fail, end, process, jump, comply, restart
}
internal enum TypeCharacter
{
    player, helper
}

internal enum TypeCodeAction
{
    nullAction, inBox, outBox, jump, copyform
}

internal class LevelManager : MonoBehaviour
{
    internal static LevelManager instance { get; private set; }

    [SerializeField] private int indexLevel;
    public int IndexLevel => indexLevel;

    [SerializeField] private LevelRulesBase levelRules;
    public LevelRulesBase LevelRules => levelRules;

    [SerializeField] private TypeCodeAction[] typeCodeActions;
    internal TypeCodeAction[] TypeCodeActions => typeCodeActions;

    [SerializeField] private int countActionsToComplyLevel;
    internal int CountActionsToComplyLevel => countActionsToComplyLevel;

    private StateLevel stateLevel;
    internal StateLevel StateLevel => stateLevel;

    internal List<CodeTaskBase> CodeActionsComply { get; set; }

    internal int NumberTaskCode { get; set; }

    [SerializeField] private LevelData levelData;
    internal LevelData LevelData => levelData;

    [SerializeField] internal List<ItemBase> itemsConveerUp, itemsConveerDown;

    [SerializeField] internal List<CodeTaskBase> activeTasks, complyTasks, endTasks;

    private void Awake() => instance = this;

    private void Start()
    {
        activeTasks = new List<CodeTaskBase>();
        complyTasks = new List<CodeTaskBase>();
        endTasks = new List<CodeTaskBase>();

        LevelEvents.NewStateLevelEvent += CheckStateLevel;

        UIManager.instance.StartActionsCode.onClick.RemoveAllListeners();
        UIManager.instance.StartActionsCode.onClick.AddListener(StartActionsCode);
    }
    private void OnDisable() =>
        LevelEvents.NewStateLevelEvent -= CheckStateLevel;

    private void CheckStateLevel(StateLevel stateLevel)
    {
        this.stateLevel = stateLevel;
        Debug.Log("�������� ������� �����: " + stateLevel);
        if (stateLevel == StateLevel.restart || stateLevel == StateLevel.comply || stateLevel == StateLevel.fail ||
            stateLevel == StateLevel.menuLevels)
        {
            complyTasks.Clear();
            endTasks.Clear();
        }
    }

    internal void StartActionsCode()
    {
        if (stateLevel != StateLevel.start)
            return;

        LevelEvents.NewStateLevel(StateLevel.process);
        activeTasks[0].StartTaskCode(TypeMainCodeTask.Default);
        //��������� ������ ������, ��������� ��� ����� ���� ��������� �� ������������ ��������� ����� ������ � ������������������
    }

    internal async void InitLevel()
    {
        //����� ����� ����� ������� �������� ���������, ����� �������� � ������� ��� � ������������ ����
        await UniTask.Delay(1000);
        LevelEvents.NewStateLevel(StateLevel.loading);
    }

    //�������� �� ���������� ���������� �������
    internal bool CheckComplyAllTasks(TypeMainCodeTask typeCheck)
    {
        //���������� ������������ ���������
        int countItemsDelivered = LevelEvents.GetItemsDelivered();

        int countCheckTasks = 0;

        if (typeCheck == TypeMainCodeTask.Default)
            countCheckTasks = endTasks.Count;

        else if (typeCheck == TypeMainCodeTask.Jump)
            countCheckTasks = countItemsDelivered;

        //��������� ��� ��� ��������� ������ ��� ��������� ��������, �� ����������� �������

        if (complyTasks.Count == countCheckTasks && countItemsDelivered < CountActionsToComplyLevel / 2)
        {
            LevelEvents.NewStateLevel(StateLevel.fail);
            MessageScreen.instance.NewMessageScreen("������������ ����� �� ������!" + $" ����������� ������� <color=white>{ CountActionsToComplyLevel / 2}</color> "
                + $"���������, � �� <color=white> { countItemsDelivered }</color>" + "!", Color.red);
            return false;
        }

        return true;
    }

    internal async UniTask StartNextTask(TypeMainCodeTask typeMainCodeTask)
    {
        if(NumberTaskCode < activeTasks.Count - 1 && StateLevel != StateLevel.jump)
        {
            ++NumberTaskCode;
            await activeTasks[NumberTaskCode].StartTaskCode(typeMainCodeTask);
        }
    }

    internal void ChangeSetSiblingItems()
    {
        for(var i = 0; i < activeTasks.Count; i++)
            activeTasks[i].transform.SetSiblingIndex(i);
    }
}
