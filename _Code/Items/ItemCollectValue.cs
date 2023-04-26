using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ItemCollectValue : ItemBase, ITargetMovable
{
    [SerializeField] private bool randomType;
    public bool RandomType => randomType;

    [SerializeField] private int numberValueItem;
    public int NumberValueItem => numberValueItem;

    [SerializeField] private TypeCharItemCollect typeCharItemCollect;
    public TypeCharItemCollect _TypeCharItemCollect => typeCharItemCollect;

    private void OnMouseDown() =>
        LevelEvents.SetNumberItem(numberValueItem);

    protected override void CheckStateLevel(StateLevel newStateLevel)
    {
        if (newStateLevel == StateLevel.start && randomType)
            typeCharItemCollect = (TypeCharItemCollect)Enum.Parse(typeof(TypeCharItemCollect), numberValueItem.ToString());
    }

    public string GetCharsItem
    {
        get
        {
            if (randomType)
                return typeItem.ToString();
            else
                return typeCharItemCollect.ToString();
        }
    }
}
