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
            itemDescText.text = item.websiteDescription + GenerateStatText(item);
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

    private static string GenerateStatText(MoveableItem item) {
        string result = " ";
        if(item.purchaseHealth != 0) {
            result += "Health " + GetNumberString(item.purchaseHealth) + "  ";
        }
        if (item.purchaseExcretion != 0) {
            result += "Excretion " + GetNumberString(item.purchaseExcretion) + "  ";
        }
        if (item.purchaseHunger != 0) {
            result += "Hunger " + GetNumberString(item.purchaseHunger) + "  ";
        }
        if (item.purchaseFilth != 0) {
            result += "Filth " + GetNumberString(item.purchaseFilth) + "  ";
        }
        if (item.purchaseSatisfaction != 0) {
            result += "Satisfaction " + GetNumberString(item.purchaseSatisfaction) + "  ";
        }
        if (item.satisfactionPerSecond != 0) {
            result += "Satisfaction per Second " + GetNumberString(item.satisfactionPerSecond) + "  ";
        }

        return result;
    }

    private static string GetNumberString(float number) {
        return number >= 0 ? "+" + number.ToString() : number.ToString();
    }
}
