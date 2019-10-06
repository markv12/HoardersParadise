using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPanelManager : MonoBehaviour
{
    public Button mainButton;
    public Image mainImage;
    public TMP_Text itemText;
    public TMP_Text itemDescText;

    [NonSerialized]
    private MoveableItem item;
    public MoveableItem Item {
        get {
            return item;
        }
        set {
            item = value;
            itemText.text = item.title;
            itemDescText.text = item.websiteDescription;
            mainImage.sprite = item.sprite;
        }
    }
    public delegate void ItemPanelClickedEvent(MoveableItem itemType);
    public event ItemPanelClickedEvent itemPanelClickedEvent;

    private void Awake() {
        mainButton.onClick.AddListener(delegate { FireItemPanelClicked(); });
    }

    private void FireItemPanelClicked() {
        itemPanelClickedEvent?.Invoke(item);
    }
}
