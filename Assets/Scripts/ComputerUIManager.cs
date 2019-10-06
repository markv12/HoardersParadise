using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerUIManager : MonoBehaviour {
    public static ComputerUIManager instance;

    public GameObject mainObject;
    public ItemPanelManager[] itemPanels;
    public ItemCollection itemCollection;
    public Transform instantiationLocation;

    void Awake() {
        mainObject.SetActive(false);
        instance = this;
        for (int i = 0; i < itemPanels.Length; i++) {
            itemPanels[i].itemPanelClickedEvent += ItemPanelClickedHandler;
        }
    }

    private void ItemPanelClickedHandler(MoveableItem itemType) {
        mainObject.SetActive(false);
        Time.timeScale = 1;
        InstantiateItemType(itemType, instantiationLocation.position);
    }

    private List<MoveableItem> tempItems = new List<MoveableItem>();
    public void ShowComputerUI() {
        mainObject.SetActive(true);
        Time.timeScale = 0;
        tempItems.AddRange(itemCollection.items);
        for (int i = 0; i < itemPanels.Length; i++) {
            int randomInt = Random.Range(0, tempItems.Count);
            itemPanels[i].Item = tempItems[randomInt];
            tempItems.RemoveAt(randomInt);
        }
        tempItems.Clear();
    }

    public void InstantiateItemType(MoveableItem item, Vector3 position) {
        GameObject newItem = Instantiate(item.gameObject);
        newItem.transform.position = position;
        StatUIManager.instance.RegisterItem(newItem.GetComponent<MoveableItem>());
    }
}
