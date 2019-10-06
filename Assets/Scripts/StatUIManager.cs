using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUIManager : MonoBehaviour
{
    public static StatUIManager instance;

    public Image healthBar;
    public Image toiletBar;
    public Image hungerBar;
    public Image filthBar;

    private float health = 100;
    private float Health {
        get {
            return health;
        }
        set {
            health = Mathf.Max(0, value);
            healthBar.fillAmount = health / 100f;
        }
    }

    private float toilet = 0;
    public float Toilet {
        get {
            return toilet;
        }
        set {
            toilet = Mathf.Max(0, value);
            toiletBar.fillAmount = toilet / 100f;
        }
    }

    private float hunger = 0;
    private float Hunger {
        get {
            return hunger;
        }
        set {
            hunger = Mathf.Max(0, value);
            hungerBar.fillAmount = hunger / 100f;
        }
    }

    private float filth = 0;
    private float Filth {
        get {
            return filth;
        }
        set {
            filth = Mathf.Max(0, value);
            filthBar.fillAmount = filth / 100f;
        }
    }
    void Awake()
    {
        instance = this;
        Health = 100f;
        Toilet = 0f;
        Hunger = 0f;
        Filth = 0f;
    }

    private const float HUNGER_PER_SECOND = 0.666f;
    private const float TOILET_PER_SECOND = 0.666f;
    void Update()
    {
        Filth += (currentFilthPerMinute / 60f) * Time.deltaTime;
        Hunger += HUNGER_PER_SECOND * Time.deltaTime; 
        Toilet += TOILET_PER_SECOND * Time.deltaTime; 
    }

    private List<MoveableItem> activeItems = new List<MoveableItem>();
    private float currentFilthPerMinute = 0;
    public void RegisterItem(MoveableItem newItem) {
        activeItems.Add(newItem);
        Health += newItem.purchaseHealth;
        Toilet += newItem.purchaseExcretion;
        Hunger += newItem.purchaseHunger;
        Filth += newItem.purchaseFilth;
        currentFilthPerMinute += newItem.filthPerMinute;
    }
}
