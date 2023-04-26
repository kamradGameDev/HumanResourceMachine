using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class ConveerBase : ItemBase
{
    [SerializeField] private Transform[] itemParents;
    public Transform[] ItemParents => itemParents;

    [SerializeField] protected List<ItemBase> items;

    protected abstract void CheckStateConveer(ConveerBase conveer, ItemBase item, bool type);

    protected override void OnEnable()
    {
        base.OnEnable();
        LevelEvents.ConveerEvent += CheckStateConveer;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LevelEvents.ConveerEvent += CheckStateConveer;
    }
}
