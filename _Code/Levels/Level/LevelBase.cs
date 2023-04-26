using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract class LevelBase : ScriptableObject
{
    [SerializeField] private LevelData levelData;
    internal LevelData LevelData => levelData;

   
}
