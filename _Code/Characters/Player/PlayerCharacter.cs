using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Cysharp.Threading.Tasks;

internal class PlayerCharacter : CharacterBase
{
    [SerializeField] private ItemBase collectItem;

    protected async override UniTask CheckCall(TypeCharacter typeCharacter, ITargetMovable targetMovable)
    {
        if (typeCharacter == TypeCharacter.player && targetMovable != null)
        {
            moveCharacterState.Init(targetMovable);
            actualCharacterState = moveCharacterState;
            while (actualCharacterState != null)
                await UniTask.Delay(100);
        }
        await UniTask.CompletedTask;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ItemCollect") && GetTargetMove() == collision.transform)
        {
            collectItem = collision.transform.parent.gameObject.GetComponent<ItemBase>();
            collision.transform.parent.gameObject.SetActive(false);
            collision.transform.parent.parent = transform;

            ConveerUp conveerUp = new ConveerUp();

            LevelEvents.NewCallConveerState(conveerUp, collision.transform.parent.gameObject.GetComponent<ItemBase>(), false);

            LevelManager.instance.itemsConveerUp.Remove(collectItem);

            actualCharacterState = null;
        }

        else if(collision.CompareTag("TakeItems") && GetTargetMove() == collision.transform)
        {
            ConveerDown conveerDown = new ConveerDown();
            LevelEvents.NewCallConveerState(conveerDown, collectItem, true);

            //collectItem = null;

            actualCharacterState = null;
        }

        else if (collision.CompareTag("ItemCollectValue") && GetTargetMove().parent == collision.transform)
        {
            collectItem = collision.gameObject.GetComponent<ItemBase>();
            collision.transform.gameObject.SetActive(false);
            collision.transform.parent = transform;

            actualCharacterState = null;
        }
    }

    private Transform GetTargetMove()
    {
        object targetProperty = null;

        //срабатывает лишний раз, не могу понять почему
        if(actualCharacterState)
            targetProperty = actualCharacterState.GetType().GetProperty("TargetMovable").GetValue(actualCharacterState);

        return (Transform)targetProperty;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        LevelEvents.GetPlayerCollectItemEvent += GetPlayerCollectItem;
        LevelEvents.SetPlayerCollectItemEvent += SetPlayerCollectItem;
        LevelEvents.NewStateLevelEvent += CheckLevelState;
    }

    private void CheckLevelState(StateLevel stateLevel)
    {
        if(stateLevel == StateLevel.fail || stateLevel == StateLevel.restart || stateLevel == StateLevel.menuLevels)
        {
            Debug.Log("stateLevel: " + stateLevel);

            if (collectItem == null)
                return;

            Destroy(collectItem.gameObject);
            collectItem = null;
        }
    }

    //можно сюда переместить сборку предметов также, присылать через событие
    //а функции прикрепить к типам предметов колизии
    private void SetPlayerCollectItem(ItemBase newItem)
    {
        if(newItem == null)
        {
            //сюда можно добавить анимацию выкидывания предмета
            
        }
        collectItem = newItem;
    }

    private ItemBase GetPlayerCollectItem() => collectItem;

    protected override void OnDisable()
    {
        base.OnDisable();
        LevelEvents.GetPlayerCollectItemEvent -= GetPlayerCollectItem;
    }

}
