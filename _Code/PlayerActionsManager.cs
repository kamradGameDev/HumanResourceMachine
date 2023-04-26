using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PlayerActionsManager : CharacterActionsManagerBase
{
    internal static PlayerActionsManager instance { get; private set; }

    [SerializeField] private PlayerActionBase[] playerActions;
    internal PlayerActionBase[] PlayerActions => playerActions;

    private void Awake() => instance = this;
}
