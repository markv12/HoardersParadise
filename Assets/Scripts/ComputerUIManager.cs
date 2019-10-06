using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerUIManager : MonoBehaviour
{
    public static ComputerUIManager instance;

    public GameObject mainObject;
    public ItemPanelManager[] itemPanels;
    public ItemCollection itemCollection;
    public Transform instantiationLocation;

    void Awake()
    {
        mainObject.SetActive(false);
        instance = this;
        for (int i = 0; i < itemPanels.Length; i++) {
            itemPanels[i].itemPanelClickedEvent += ItemPanelClickedHandler;
        }
    }

    private void ItemPanelClickedHandler(MoveableItem itemType) {
        mainObject.SetActive(false);
        Time.timeScale = 1;
        InstantiateItemType(itemType);
    }

    public void ShowComputerUI() {
        MoveableItem[] items = itemCollection.items;
        mainObject.SetActive(true);
        Time.timeScale = 0;
        for (int i = 0; i < itemPanels.Length; i++) {
            itemPanels[i].Item = items[Random.Range(0, items.Length)];
        }
    }

    public void InstantiateItemType(MoveableItem item) {
        GameObject newItem = Instantiate(item.gameObject);
        newItem.transform.position = instantiationLocation.position;
        StatUIManager.instance.RegisterItem(newItem.GetComponent<MoveableItem>());
    }
}
