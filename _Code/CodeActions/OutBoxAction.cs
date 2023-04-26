using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

internal class OutBoxAction : CodeTaskBase
{
    internal async override UniTask StartTaskCode(TypeMainCodeTask typeMainCodeTask)
    {
        if (LevelManager.instance.StateLevel == StateLevel.fail || LevelManager.instance.StateLevel == StateLevel.comply)
            return;

        Debug.Log("Out In Box Action");

        //плюс €щики должны прийти по числовому пор€дку, то есть к примеру 7,2, если сразу принес 2 то уже ошибка будет
        //будут две проверки, одна на количество предметов доставленных, втора€ на номер €щика, если хоть одна ошибка то прерывание уровн€
        ConveerDown conveerDown = new ConveerDown();
        ITargetMovable targetMovable = LevelEvents.GetTargetMovable(conveerDown);

        await LevelEvents.CallCodeAction(TypeCharacter.player, targetMovable);

        if(LevelEvents.GetPlayerCollectItem())
        {
            Debug.Log("положили предмет на конвеер");
            
            LevelManager.instance.complyTasks.Add(this);
            LevelManager.instance.endTasks.Add(this);

            LevelEvents.SetPlayerCollectItem(null);

            if (LevelManager.instance.complyTasks.Count < LevelManager.instance.activeTasks.Count && 
                LevelManager.instance.activeTasks[LevelManager.instance.complyTasks.Count].GetType().Name != "OutBoxAction")
            {
                Debug.Log("старт следующей задачи");
                await LevelManager.instance.StartNextTask(typeMainCodeTask);
            }
            else
            {
                LevelManager.instance.CheckComplyAllTasks(typeMainCodeTask);
                await LevelManager.instance.LevelRules.CheckRules();
            }
            return;
        }

        if (LevelManager.instance.StateLevel == StateLevel.comply || LevelManager.instance.StateLevel == StateLevel.fail)
            return;

        Debug.Log("ѕустое значение! ¬ы не можете делать операцию OUTBOX с пустыми руками!.");
        

        LevelManager.instance.complyTasks.Add(this);

        if (!LevelEvents.GetPlayerCollectItem())
        {
            LevelManager.instance.endTasks.Add(this);
            MessageScreen.instance.NewMessageScreen("ѕустое значение! ¬ы не можете делать операцию OUTBOX с пустыми руками!.", Color.red);
            LevelEvents.NewStateLevel(StateLevel.fail);
            return;
        }

        //конец задачи

        if (LevelManager.instance.CheckComplyAllTasks(typeMainCodeTask))
            return;

        if (!LevelManager.instance.LevelRules.CheckRules().GetAwaiter().GetResult())
            return;
        Debug.Log("end task out box");

        await LevelManager.instance.StartNextTask(typeMainCodeTask);
        
    }
}
