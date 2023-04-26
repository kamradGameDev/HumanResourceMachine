using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ItemBox : ItemBase, ITargetMovable, ICollectItem
{

    public object GetClass() => this;

    protected override void CheckStateLevel(StateLevel newStateLevel)
    {
        
    }
}
