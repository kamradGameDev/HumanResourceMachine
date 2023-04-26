using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

internal class CopyFormCodeAction : CodeTaskBase
{
    [SerializeField] private TextMeshProUGUI numberCollectItemText;

    [SerializeField] private int numberCollectItem;

    internal override void InitializeTask(int index = 0)
    {
        base.InitializeTask(index);
        numberCollectItemText.gameObject.SetActive(true);

        numberCollectItem = 1;
        numberCollectItemText.text = numberCollectItem.ToString();
    }

    internal async override UniTask StartTaskCode(TypeMainCodeTask typeMainCodeTask)
    {
        if (LevelManager.instance.StateLevel == StateLevel.fail || LevelManager.instance.StateLevel == StateLevel.comply)
            return;

        int currentTask = LevelManager.instance.NumberTaskCode;

        if (LevelManager.instance.complyTasks.Count > 0 && LevelManager.instance.complyTasks[currentTask - 1].GetType().Name == "CopyFormCodeAction")
        {
            LevelEvents.NewStateLevel(StateLevel.fail);
            Debug.Log("Выкидываем предмет и ошибка");
            await UniTask.CompletedTask;
        }

        if(numberCollectItem != 0)
        {
            foreach(var item in ItemsCollectValue.instance.Items)
                if(item.NumberValueItem == numberCollectItem - 1)
                {
                    ITargetMovable targetMovable = item;
                    await LevelEvents.CallCodeAction(TypeCharacter.player, targetMovable);
                }
        }

        //конец задачи
        LevelManager.instance.complyTasks.Add(this);
        await LevelManager.instance.StartNextTask(typeMainCodeTask);

        if (!LevelManager.instance.LevelRules.CheckRules().GetAwaiter().GetResult())
            return;
    }

    public void ChangeNumberCollectitem()
    {
        //подписались на событие и ждем пока оно вызовется
        //чтобы получить нужное значние и сразу отписать от события
        LevelEvents.SetNumberItemEvent += Change;
    }

    private void Change(int number)
    {
        numberCollectItem = ++number;

        numberCollectItemText.text = numberCollectItem.ToString();
        LevelEvents.SetNumberItemEvent -= Change;
    }
}
