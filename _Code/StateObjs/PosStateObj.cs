using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PosStateObj : StateObjBase
{
    private Vector3 startPos;
    private bool start = false;

    protected override void RestartState()
    {
        if(start)
            transform.localPosition = startPos;
    }


    protected override void SaveStartState()
    {
        if(!start)
            startPos = transform.localPosition;
        start = true;
    }
}
