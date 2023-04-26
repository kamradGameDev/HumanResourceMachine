using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Levels/NewLevelData", fileName = "NewLevelData", order = 56)]
internal class LevelData : ScriptableObject
{
    [Multiline(10)]
    [SerializeField] private string descLevel;
    [SerializeField] private string nameLevel;
    
    internal string DescLevel => descLevel;
    internal string NameLevel => nameLevel;
}
