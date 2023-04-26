using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

internal static class LevelEvents
{
    internal static event Func<TypeCharacter, ITargetMovable, UniTask> CodeActionEvent;
    internal static event Action<StateLevel> NewStateLevelEvent;

    internal static event Action<ConveerBase, ItemBase, bool> ConveerEvent;
    internal static event Func<object, ITargetMovable> GetTargetMovableEvent;
    internal static event Func<ItemBase> GetPlayerCollectItemEvent;
    internal static event Action<ItemBase> SetPlayerCollectItemEvent;

    internal static event Action<ItemBase> GetCountItemsInConveerDownEvent;
    internal static Action<int> SetNumberItemEvent;

    internal static Func<int> GetItemsDeliveredEvent;

    internal static UniTask CallCodeAction(TypeCharacter typeCharacter, ITargetMovable target) =>
        (UniTask)(CodeActionEvent?.Invoke(typeCharacter, target));

    internal static void NewStateLevel(StateLevel newStateLevel) =>
        NewStateLevelEvent?.Invoke(newStateLevel);

    internal static void NewCallConveerState(ConveerBase conveer, ItemBase item, bool type) =>
        ConveerEvent?.Invoke(conveer, item, type);

    internal static ITargetMovable GetTargetMovable(object _object) =>
        GetTargetMovableEvent?.Invoke(_object);

    internal static ItemBase GetPlayerCollectItem() =>
        GetPlayerCollectItemEvent?.Invoke();

    internal static void SetPlayerCollectItem(ItemBase newItem) =>
        SetPlayerCollectItemEvent?.Invoke(newItem);

    internal static void SetNumberItem(int number) =>
        SetNumberItemEvent?.Invoke(number);

    internal static void GetCountItemsInConveerDown(ItemBase lastItem) =>
        GetCountItemsInConveerDownEvent?.Invoke(lastItem);

    internal static int GetItemsDelivered() =>
        (int)(GetItemsDeliveredEvent?.Invoke());
}
