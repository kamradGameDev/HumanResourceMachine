using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ConveerDown : ConveerBase, ITargetMovable
{
    protected override void OnEnable()
    {
        base.OnEnable();
        LevelEvents.GetTargetMovableEvent += GetTargetMovable;
        LevelEvents.GetItemsDeliveredEvent += GetItemsDelivered;
        LevelEvents.NewStateLevelEvent += CheckStateLevel;
    }

    private ITargetMovable GetTargetMovable(object _object)
    {
        if (_object.GetType().Name == "ConveerDown")
            return this;

        return null;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LevelEvents.GetTargetMovableEvent -= GetTargetMovable;
        LevelEvents.GetItemsDeliveredEvent -= GetItemsDelivered;
        LevelEvents.NewStateLevelEvent -= CheckStateLevel;
    }

    private int GetItemsDelivered()
    {
        int itemCount = 0;

        foreach (var item in ItemParents)
            if (item.childCount > 0)
                itemCount++;

        return itemCount;
    }

    protected override void CheckStateConveer(ConveerBase conveer, ItemBase item, bool type)
    {
        if(conveer.GetType().Name == "ConveerDown")
        {
            if (item == null)
                return;

            items.Add(item);
            item.transform.parent = CheckChildNull();
            item.gameObject.SetActive(true);
            item.transform.localPosition = Vector3.zero;

            LevelEvents.GetCountItemsInConveerDown(item);

            if (!LevelManager.instance.LevelRules.CheckRules().GetAwaiter().GetResult())
                return;
        }
    }

    private Transform CheckChildNull()
    {
        foreach (var par in ItemParents)
            if (par.gameObject.transform.childCount == 0)
                return par;

        return null;
    }

    protected override void CheckStateLevel(StateLevel newStateLevel)
    {
        //здесь прописывать движение конвеера вниз когда выполнены все условия уровня
        if (newStateLevel == StateLevel.restart || newStateLevel == StateLevel.menuLevels || newStateLevel == StateLevel.comply)
        {
            foreach (var item in items)
                if (item)
                    Destroy(item.gameObject);

            items.Clear();
        }
    }
}
