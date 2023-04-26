using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSpeedGame : MonoBehaviour
{
    public static ScrollSpeedGame instance { get; private set; }
    [SerializeField] private Slider slider;

    [SerializeField] private float multiplySpeed;

    [SerializeField] private float MultiplySpeed => multiplySpeed;

    //на данный момент все объекты синглтона просто замен€ют собой ссылку
    private void Awake() => instance = this;

    public float GetGameSpeed() =>
        (slider.value + 1) * multiplySpeed;
}
