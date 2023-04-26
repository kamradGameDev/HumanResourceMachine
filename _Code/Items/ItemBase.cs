using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


internal abstract class ItemBase : MonoBehaviour
{
    [SerializeField] internal SpriteRenderer sprite;
    [SerializeField] internal int typeItem;

    protected virtual void OnEnable() => LevelEvents.NewStateLevelEvent += CheckStateLevel;

    protected abstract void CheckStateLevel(StateLevel newStateLevel);

    protected virtual void OnDisable() => LevelEvents.NewStateLevelEvent -= CheckStateLevel;
}
