using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class InteractActionButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform dragObj;

    [SerializeField] private CodeTaskBase childTaskCode;
    private CodeTaskBase ChildTaskCode => childTaskCode;

    private string startNameParent;

    [SerializeField] GraphicRaycaster m_Raycaster;
    [SerializeField] EventSystem m_EventSystem;

    private PointerEventData m_PointerEventData;

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (LevelManager.instance.StateLevel != StateLevel.start)
            return;

        startNameParent = transform.parent.name;
        //прописано через простое условие но можно использовать и состояние-поведение
        if (startNameParent == "ButtonsToChooseFrom")
            dragObj = Instantiate(gameObject, transform.position, Quaternion.identity, UIManager.instance.MainCanvas.transform).transform;
        else
            transform.parent = UIManager.instance.MainCanvas.transform;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (LevelManager.instance.StateLevel != StateLevel.start)
            return;

        if (startNameParent == "ButtonsToChooseFrom")
            dragObj.transform.position = Input.mousePosition;

        else if (startNameParent == "ActionsCodeContent")
            transform.position = Input.mousePosition;
    }

    private List<RaycastResult> GetRaycastResults()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        m_Raycaster.Raycast(m_PointerEventData, results);

        return results;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (LevelManager.instance.StateLevel != StateLevel.start)
            return;

        foreach (var item in GetRaycastResults())
        {
            if (item.gameObject.name == "ScrollCodeActions" && startNameParent == "ButtonsToChooseFrom")
            {
                GameObject obj = Instantiate(gameObject, UIManager.instance.ActionsCodeContent);

                CodeTaskBase codeTask = obj.GetComponent<CodeTaskBase>();

                LevelManager.instance.activeTasks.Add(codeTask);

                int newCount = UIManager.countActions++;

                codeTask.InitializeTask(newCount);

                if (childTaskCode)
                {
                    InteractActionButton interactActionButton = obj.GetComponent<InteractActionButton>();

                    CodeTaskBase childCodeTask = interactActionButton.ChildTaskCode;

                    childCodeTask.gameObject.AddComponent<ChangeSetSiblingCodeTaskUI>();

                    childCodeTask.GetComponent<Image>().enabled = true;

                    childCodeTask.transform.parent = UIManager.instance.ActionsCodeContent;

                    Destroy(childCodeTask.GetComponent<InteractActionButton>());

                    LevelManager.instance.activeTasks.Add(childCodeTask);

                    newCount = UIManager.countActions++;

                    childCodeTask.InitializeTask(newCount);

                }

                obj.AddComponent<ChangeSetSiblingCodeTaskUI>();
                Destroy(obj.GetComponent<InteractActionButton>());
            }     
        }

        if(dragObj)
            Destroy(dragObj.gameObject);
    }
}
