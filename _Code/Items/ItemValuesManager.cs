using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public struct TypeCharItem
{
    [SerializeField] internal TypeCharItemCollect typeCharItemCollect;
    [SerializeField] internal Sprite sprite;
    [SerializeField] internal GameObject prefab;
}

internal class ItemValuesManager : MonoBehaviour
{
    internal static ItemValuesManager instance { get; private set; }
    
    [SerializeField] private TypeCharItem[] typeCharItems;

    private void Awake() =>
        instance = this;

    public TypeCharItem[] GetRandomTypesCharItems()
    {
        return TRandom.RandomItems(typeCharItems);
    }
}
