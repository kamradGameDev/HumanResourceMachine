using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCollectValue : MonoBehaviour
{
    internal static ItemsCollectValue instance { get; private set; }

    [SerializeField] private ItemCollectValue[] items;
    internal ItemCollectValue[] Items => items;

    private void Awake() => instance = this;
}
