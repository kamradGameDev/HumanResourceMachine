using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

internal class Level_3_Rules : LevelRulesBase
{
    [SerializeField] private TypeCharItemCollect[] typeCharItemCollects;
    public TypeCharItemCollect[] TypeCharItemCollect => typeCharItemCollects;

    private object _randomType = null;

    private ItemBase lastItemInConveerDown = null;

    private void OnEnable() => 
        LevelEvents.GetCountItemsInConveerDownEvent += GetCountItemsInConveerDown;

    private void OnDisable() => 
        LevelEvents.GetCountItemsInConveerDownEvent -= GetCountItemsInConveerDown;

    private void GetCountItemsInConveerDown(ItemBase lastItem) => 
        lastItemInConveerDown = lastItem;

    internal override UniTask<bool> CheckRules()
    {
        _randomType = lastItemInConveerDown.GetType().GetProperty("GetCharsItem").GetValue(lastItemInConveerDown);

        //минус 1 так как подразумеваетс€ что раз доставили предмет к конвееру то уже 
        //значит что проверка идет об€зательно задачи -1
        //так как последн€€ задача это доставка к конвееру или куда-то еще

        int countDelivereditems = LevelEvents.GetItemsDelivered();
        Debug.Log("ќжидалась буква: " + $" <color=white>{typeCharItemCollects[countDelivereditems - 1]}</color>" +
                ", а получили " + $"<color=white>{_randomType}</color>");

        //if (LevelManager.instance.complyTasks.Count - 1 / 2 < 0 || LevelManager.instance.complyTasks.Count - 1 / 2 > typeCharItemCollects.Length - 1)
        //    return UniTask.FromResult(false);

        if (_randomType.ToString() != typeCharItemCollects[countDelivereditems - 1].ToString())
        {
            Debug.Log("fail level");
            MessageScreen.instance.NewMessageScreen("ќжидалась буква: " + $" <color=white>{typeCharItemCollects[countDelivereditems - 1]}</color>" +
               ", а получили " + $"<color=white>{_randomType}</color>", Color.red);
            LevelEvents.NewStateLevel(StateLevel.fail);
            SendMessage();
            LevelManager.instance.complyTasks.Clear();
            return UniTask.FromResult(false);
        }

        else
        {
            if (countDelivereditems == LevelManager.instance.CountActionsToComplyLevel)
                LevelEvents.NewStateLevel(StateLevel.comply);
        }

        return UniTask.FromResult(true);
    }

    public override void SendMessage()
    {
       
    }
}
