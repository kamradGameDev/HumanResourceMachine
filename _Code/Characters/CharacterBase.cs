using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


abstract class CharacterBase : MonoBehaviour
{
    //[SerializeField] private Rigidbody2D rb2D;
    //internal Rigidbody2D Rb2D => rb2D;

    [SerializeField] protected StateCharacterBase[] characterStates;

    protected StateCharacterBase actualCharacterState;

    protected MoveCharacterState moveCharacterState;

    protected virtual void OnEnable() =>
        LevelEvents.CodeActionEvent += CheckCall;

    protected virtual void OnDisable() =>
        LevelEvents.CodeActionEvent -= CheckCall;

    protected abstract UniTask CheckCall(TypeCharacter typeCharacter, ITargetMovable iTargetMovable);

   
    private void Start()
    {
        InitStates();
    }
    private void FixedUpdate()
    {
        if (!actualCharacterState)
            return;

        if (actualCharacterState.Process(this))
            actualCharacterState = null;

    }
    private void InitStates()
    {
        for (var i = 0; i < characterStates.Length; i++)
            characterStates[i] = Instantiate(characterStates[i], transform);

        foreach (var item in characterStates)
        {
            if (item is MoveCharacterState)
            {
                moveCharacterState = (MoveCharacterState)item;
                break;
            }
        }
    }
}
