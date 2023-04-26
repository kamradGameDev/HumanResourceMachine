using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract class PlayerActionBase : ScriptableObject
{
    internal abstract void Init<T>(ref T t);
    internal abstract void Process();
    protected abstract void End();
}
