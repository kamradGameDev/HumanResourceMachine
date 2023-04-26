using System.Collections;
using System.Collections.Generic;
using UnityEngine;


internal abstract class StateCharacterBase : ScriptableObject
{
    internal abstract void Init<T>(T target);
    internal abstract bool Process(CharacterBase character);
}
