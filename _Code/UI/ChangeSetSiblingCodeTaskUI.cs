using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeSetSiblingCodeTaskUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //[SerializeField] private CodeTaskBase childJumpTaskCode;
    private CodeTaskBase codeTask;

    private EventSystem m_EventSystem;
    private GraphicRaycaster m_Raycaster;

    //для удаления связанного объекта
    internal CodeTaskBase associatedCodeAction;

    private void Start()
    {
        codeTask = FindObjectOfType<CodeTaskBase>();
        m_EventSystem = FindObjectOfType<EventSystem>();
        m_Raycaster = UIManager.instance.MainCanvas.GetComponent<GraphicRaycaster>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (LevelManager.instance.StateLevel != StateLevel.start)
            return;
        transform.parent = UIManager.instance.MainCanvas.transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (LevelManager.instance.StateLevel != StateLevel.start)
            return;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (LevelManager.instance.StateLevel != StateLevel.start)
            return;

        List<string> rayResults = new List<string>();

        foreach (var item in GetRaycastResults())
            rayResults.Add(item.gameObject.name);

        if (!rayResults.Contains("ScrollCodeActions"))
        {
            CodeTaskBase[] assodiatedItems = null;
                
            if(GetComponent<AssotiatedCodeActions>())
                assodiatedItems = GetComponent<AssotiatedCodeActions>().CodeTasks;

            if (assodiatedItems != null)
            {
                foreach (var item in assodiatedItems)
                {
                    LevelManager.instance.activeTasks.Remove(item);
                    UIManager.countActions--;
                    Destroy(item.gameObject);
                }

                UIManager.instance.RecalculationIndexes();

                return;
            }

            LevelManager.instance.activeTasks.Remove(gameObject.GetComponent<CodeTaskBase>());
            UIManager.countActions--;

            //перерасчет индексов здесь
            UIManager.instance.RecalculationIndexes();

            Destroy(gameObject);
            return;
        }


        transform.parent = UIManager.instance.ActionsCodeContent;

        CodeTaskBase nearestTask = GetNearestCodeTask();

        int newIndex = 0;

        if (transform.localPosition.y > nearestTask.transform.localPosition.y)
        {
            if(nearestTask.IndexTask - 1 > 0)
                newIndex = nearestTask.IndexTask - 1;

            int tempIndex = nearestTask.IndexTask + 1;

            codeTask.ChangeIndexTask(newIndex);
            nearestTask.ChangeIndexTask(tempIndex);
        }
        else
        {
            if (nearestTask.IndexTask < LevelManager.instance.activeTasks.Count)
                newIndex = nearestTask.IndexTask + 1;

            codeTask.ChangeIndexTask(newIndex);
        }
        UIManager.instance.SwapActiveCodeActionButtons();
        LevelManager.instance.ChangeSetSiblingItems();

        SortSublingItems();
    }

    private void SortSublingItems()
    {
        //List<int> indexItems = new List<int>();

        var items = LevelManager.instance.activeTasks;

        //foreach (var item in items)
        //    indexItems.Add(item.transform.GetSiblingIndex());

        for(var i = 0; i < items.Count; i++)
            items[i].ChangeIndexTask(i);
    }

    private CodeTaskBase GetNearestCodeTask()
    {
        CodeTaskBase codeTask = null;

        List<RaycastResult> objs = GetRaycastResults();

        foreach(var item in objs)
        {
            if(item.gameObject.GetComponent<CodeTaskBase>())
            {
                codeTask = item.gameObject.GetComponent<CodeTaskBase>();
                break;
            }
        }

        codeTask = FindNearestItemIndex();

        return codeTask;
    }

    private CodeTaskBase FindNearestItemIndex()
    {
        List<CodeTaskBase> codeTasks = FindObjectsOfType<CodeTaskBase>().ToList();

        Dictionary<CodeTaskBase, float> distances = new Dictionary<CodeTaskBase, float>();

        //исключаем этот элемент из списка, он же добавляет сейчас все элементы а потом находит ближайшего себя же
        codeTasks.Remove(codeTask);

        foreach (var item in codeTasks)
            distances.Add(item, Vector2.Distance(codeTask.transform.position, item.transform.position));

        float minDist = distances.Values.Min();

        return distances.FirstOrDefault(x => x.Value == minDist).Key;
    }

    private List<RaycastResult> GetRaycastResults()
    {
        PointerEventData m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> resultsRaycasts = new List<RaycastResult>();

        m_Raycaster.Raycast(m_PointerEventData, resultsRaycasts);

        return resultsRaycasts;
    }
}
