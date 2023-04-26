using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

internal class Level_1_Rules : LevelRulesBase
{
    public override void SendMessage()
    {
        
    }

    internal override UniTask<bool> CheckRules()
    {
        bool check = base.CheckRules().GetAwaiter().GetResult();

        return UniTask.FromResult(check);
    }
}
