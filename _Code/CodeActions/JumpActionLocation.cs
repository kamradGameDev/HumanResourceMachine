using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

internal class JumpActionLocation : CodeTaskBase, IJumpAction
{
    [SerializeField] private JumpAction mainJumpAction;

    internal override void InitializeTask(int index = 0)
    {
        //сперва узнаём индекс главной кнопки прыжка
        int indexMainJump = mainJumpAction.IndexTask;
        //затем опускаем эту кнопку по умолчанию на 1 индекс вниз
        indexTask = indexMainJump + 1;
        transform.parent = mainJumpAction.transform.parent;
        gameObject.SetActive(true);

        LevelManager.instance.activeTasks.Add(this);
        
        base.InitializeTask(indexTask);

        UIManager.instance.SwapActiveCodeActionButtons();

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

        await UniTask.Delay(300);

        if (LevelManager.instance.StateLevel == StateLevel.start)
        {
            Debug.Log("start loop jump: " + name);

            IJumpAction iJump = this;

            iJump.LoopJump(IndexTask, LevelManager.instance.activeTasks);
            //LevelEvents.NewStateLevel(StateLevel.jump);
        }

    }
}
