using System;
using System.Collections.Generic;
using UnityEngine;

public class ComputerUIManager : MonoBehaviour {
    public static ComputerUIManager instance;

    public GameObject mainObject;
    public ItemPanelManager[] itemPanels;
    public ItemCollection itemCollection;
    public Transform instantiationLocation;

    private List<MoveableItem> tempItems = new List<MoveableItem>();

    void Awake() {
        mainObject.SetActive(false);
        instance = this;
        for (int i = 0; i < itemPanels.Length; i++) {
            itemPanels[i].itemPanelClickedEvent += ItemPanelClickedHandler;
        }
        FillWithRandomItems(tempItems);
    }

    private HashSet<string> seenItemNames = new HashSet<string>();
    private void ItemPanelClickedHandler(MoveableItem item) {
        mainObject.SetActive(false);
        Time.timeScale = 1;
        if (!seenItemNames.Contains(item.title)) {
            AlertCanvasManager.instance.ShowAlert(item.purchaseEventText);
            seenItemNames.Add(item.title);
        }
        InstantiateItem(item, instantiationLocation.position);
        if (item.purchaseSatisfaction != 0) {
            InfoCirclePool.instance.ShowInfoCircle("Satisfaction " + ItemPanelManager.GetNumberString(item.purchaseSatisfaction), instantiationLocation.position, Color.black, 0.4f);
        }
    }

    private int tempItemsIndex = 0;
    public void ShowComputerUI() {
        mainObject.SetActive(true);
        Time.timeScale = 0;
        for (int i = 0; i < itemPanels.Length; i++) {
            itemPanels[i].Item = tempItems[tempItemsIndex];
            tempItemsIndex++;
            if(tempItemsIndex >= tempItems.Count) {
                tempItems.Clear();
                FillWithRandomItems(tempItems);
                tempItemsIndex = 0;
            }
        }
    }

    private static System.Random rng = new System.Random();
    private void FillWithRandomItems(List<MoveableItem> tempItems) {
        tempItems.AddRange(itemCollection.items);
        int n = tempItems.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            MoveableItem value = tempItems[k];
            tempItems[k] = tempItems[n];
            tempItems[n] = value;
        }
    }

    public void InstantiateItem(MoveableItem item, Vector3 position) {
        GameObject newItem = Instantiate(item.gameObject);
        newItem.transform.position = position;
        StatUIManager.instance.RegisterItem(newItem.GetComponent<MoveableItem>());
    }
}
