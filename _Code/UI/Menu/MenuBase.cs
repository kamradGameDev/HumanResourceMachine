using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class MenuBase : MonoBehaviour
{
    public virtual void Open()
    {
        transform.DOScale(Vector3.one, 0.5f);
    }

    public virtual void Close()
    {
        transform.DOScale(Vector3.zero, 0.5f);
    }
}
