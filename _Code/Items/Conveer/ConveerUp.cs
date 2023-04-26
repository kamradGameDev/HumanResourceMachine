using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class ConveerUp : ConveerBase
{    
    [SerializeField] private Transform targetMove;
    [SerializeField] private float speedMove;

    protected override void CheckStateConveer(ConveerBase conveer, ItemBase item, bool type)
    {
        if(type && conveer.GetType().Name == "ConveerUp")
            items.Remove(item);

        else if(type && conveer.GetType().Name == "ConveerDown")
            items.Add(item);
    }

    protected override void CheckStateLevel(StateLevel newStateLevel)
    {
        if (newStateLevel == StateLevel.loading)
            StartCoroutine(StartMoveItems());

        else if (newStateLevel == StateLevel.fail)
        {
            foreach (var item in LevelManager.instance.itemsConveerUp)
                item.transform.parent = transform;

            //items.Clear();
        }
    }

    private IEnumerator DestroyItems()
    {
        foreach(var item in LevelManager.instance.itemsConveerUp)
        {
            if (!item.gameObject)
                continue;

            Destroy(item.gameObject);

            yield return new WaitForEndOfFrame();
        }
        items.Clear();
        LevelManager.instance.itemsConveerUp.Clear();
    }

    private IEnumerator InstanceNewItems()
    {
        yield return StartCoroutine(DestroyItems());

        TypeCharItem[] typeCharItems = ItemValuesManager.instance.GetRandomTypesCharItems();

        for(var i = 0; i < ItemParents.Length; i++)
        {
            ItemBase newItem = Instantiate(typeCharItems[i].prefab, ItemParents[i]).GetComponent<ItemBase>();

            newItem.sprite.sprite = typeCharItems[i].sprite;

            newItem.typeItem = (int)typeCharItems[i].typeCharItemCollect;

            items.Add(newItem);

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator StartMoveItems()
    {
        yield return StartCoroutine(InstanceNewItems());

        Transform parentItems = items[0].transform.parent.parent;

        while (Vector2.Distance(parentItems.position, targetMove.position) > 0.1f)
        {
            parentItems.Translate(Vector2.up * Time.deltaTime * speedMove);
            yield return new WaitForEndOfFrame();
        }

        LevelManager.instance.itemsConveerUp = items.ToList();

        LevelEvents.NewStateLevel(StateLevel.start);
    }
}
