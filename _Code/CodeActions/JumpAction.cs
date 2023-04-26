using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

internal class JumpAction : CodeTaskBase, IJumpAction
{
    internal override void InitializeTask(int index = 0)
    {
        base.InitializeTask(index);
    }

    internal async override UniTask StartTaskCode(TypeMainCodeTask typeMainCodeTask)
    {
        /*получаем индекс этого действия
         * затем запускаем индекс начального прыжка
         * и прогоняем цикл пока не будут выполены все условия уровня
         * или не будет нарушен порядок действий
         * к примеру два раза подняли предмет
         * или два раза выкинули предмет
         */

        await UniTask.Delay(100);

        if (LevelManager.instance.StateLevel == StateLevel.process)
        {
            Debug.Log("start loop jump: " + name);

            IJumpAction iJump = this;

            iJump.LoopJump(IndexTask, LevelManager.instance.activeTasks);
            //LevelEvents.NewStateLevel(StateLevel.jump);
        }

        //выставляем на одну позицию выше кнопки
    }
}

internal interface IJumpAction
{
    internal async void LoopJump(int thisCodeIndex, List<CodeTaskBase> activeTasks)
    {
        //LevelEvents.NewStateLevel(StateLevel.jump);

        int startLoopIndex = ++thisCodeIndex;

        Debug.Log($"start loop index: " + thisCodeIndex);

        LevelEvents.NewStateLevel(StateLevel.jump);

        while (LevelManager.instance.StateLevel == StateLevel.jump)
        {
            await UniTask.Yield(PlayerLoopTiming.LastUpdate);

            for (var i = startLoopIndex; i < activeTasks.Count; i++)
                await activeTasks[i].StartTaskCode(TypeMainCodeTask.Jump);
        }
    }

    //сперва находим эту задачу
    //затем начинаем выполнение следующей задачи
    //пока не найдет вторую задачу прыжка, начинаем с этой задачи и так постоянно
    //может зациклиться если рядом стоят две задачи
    //или выполнить уровень
    //или вывести ошибку выполнения уровня
    //добавить простую проверку состояния уровня
}
