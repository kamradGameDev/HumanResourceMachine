using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Data/CharacterActions/PlayerActions/DropOutItemPlayerAction", fileName = "NewDropOutItemPlayerAction", order = 59)]

class DropOutItemPlayerAction : PlayerActionBase
{
    private ItemBase dropOutItem;
    protected override void End()
    {
        //LevelManager.instance.
    }

    internal override void Init<T>(ref T t)
    {
        dropOutItem = t as ItemBase;
    }

    internal override void Process()
    {
        /*пока что будет удаляться предмет, потом можно будет делать анимацию предмету и/или игроку, много чего
        логика в том чтобы хранить такие вещи в разных местах и подключать по необходимости разное поведение
        на одинаковые задачи*/
        Destroy(dropOutItem.gameObject);
        End();
    }
}
