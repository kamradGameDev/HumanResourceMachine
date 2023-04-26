using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


class InBoxAction : CodeTaskBase
{
    internal async override UniTask StartTaskCode(TypeMainCodeTask typeMainCodeTask)
    {
        //можно взять все предметы по очередности, игрок будет выкидывать сразу и брать новый ящик, в конце списка будет ошибка
        //если выкидываются ящики при подборе то выводится сообщения о недостаточности предметов для вывода

        //если в руках уже есть предмет то ошибка, можно добавить анимацию выбрасывания предмета по желанию

        if (LevelManager.instance.StateLevel == StateLevel.fail || LevelManager.instance.StateLevel == StateLevel.comply)
            return;

        Debug.Log("Start In Box Action");

        if(LevelEvents.GetPlayerCollectItem() != null)
        {
            Debug.Log("Выкидываем предмет");

            LevelEvents.SetPlayerCollectItem(null);

            await MoveToNextTarget();

            LevelManager.instance.endTasks.Add(this);

            LevelManager.instance.CheckComplyAllTasks(typeMainCodeTask);

            int countItemsDelivered = LevelEvents.GetItemsDelivered();

            MessageScreen.instance.NewMessageScreen("Недостаточно вещей на ВЫВОДЕ!" + $" Руководство ожидало <color=white>{ LevelManager.instance.CountActionsToComplyLevel / 2}</color> "
                + $"предметов, а не <color=white> { countItemsDelivered }</color>" + "!", Color.red);

            LevelEvents.NewStateLevel(StateLevel.fail);

            return;
        }

        await MoveToNextTarget();

        LevelManager.instance.complyTasks.Add(this);
        LevelManager.instance.endTasks.Add(this);

        if (LevelManager.instance.complyTasks.Count == LevelManager.instance.activeTasks.Count)
        {
            LevelManager.instance.CheckComplyAllTasks(typeMainCodeTask);
            return;
        }

        //конец задачи

        Debug.Log("конец задачи");

        await LevelManager.instance.StartNextTask(typeMainCodeTask);       
    }

    private async UniTask MoveToNextTarget()
    {
        if (LevelManager.instance.itemsConveerUp.Count > 0)
        {
            ITargetMovable targetMovable = LevelManager.instance.itemsConveerUp[0] as ITargetMovable;
            await LevelEvents.CallCodeAction(TypeCharacter.player, targetMovable);
        }
    }
}
