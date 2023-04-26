using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class AssotiatedCodeActions : MonoBehaviour
{
    [SerializeField] private CodeTaskBase[] codeTasks;
    internal CodeTaskBase[] CodeTasks => codeTasks;
}
