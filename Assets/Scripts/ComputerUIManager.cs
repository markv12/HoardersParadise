using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerUIManager : MonoBehaviour {
    public static ComputerUIManager instance;

    public GameObject mainObject;
    public ItemPanelManager[] itemPanels;
    public ItemCollection itemCollection;
    public Transform instantiationLocation;
    public Transform infoTextSpawnLocation;

    void Awake() {
        mainObject.SetActive(false);
        instance = this;
        for (int i = 0; i < itemPanels.Length; i++) {
            itemPanels[i].itemPanelClickedEvent += ItemPanelClickedHandler;
        }
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
            InfoCirclePool.instance.ShowInfoCircle("Satisfaction " + ItemPanelManager.GetNumberString(item.purchaseSatisfaction), infoTextSpawnLocation.position);
        }
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

    public void InstantiateItem(MoveableItem item, Vector3 position) {
        GameObject newItem = Instantiate(item.gameObject);
        newItem.transform.position = position;
        StatUIManager.instance.RegisterItem(newItem.GetComponent<MoveableItem>());
    }
}
