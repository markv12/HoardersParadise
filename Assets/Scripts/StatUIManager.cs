using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUIManager : MonoBehaviour
{
    public static StatUIManager instance;

    public Image healthBar;
    public Image healthFrame;
    public Image toiletBar;
    public Image toiletFrame;
    public Image hungerBar;
    public Image hungerFrame;
    public Image filthBar;
    public Image filthFrame;
    public TMP_Text satisfactionText;

    public MoveableItem pissPrefab;
    public MoveableItem crapPrefab;

    private float health = 100;
    private float Health {
        get {
            return health;
        }
        set {
            health = Mathf.Min(100, Mathf.Max(0, value));
            healthBar.fillAmount = health / 100f;
            healthFrame.color = health <= 10 ? Color.red : Color.white;
        }
    }

    private float toilet = 0;
    public float Toilet {
        get {
            return toilet;
        }
        set {
            toilet = Mathf.Min(100, Mathf.Max(0, value));
            toiletBar.fillAmount = toilet / 100f;
            toiletFrame.color = toilet >= 90 ? Color.red : Color.white;
        }
    }

    private float hunger = 0;
    private float Hunger {
        get {
            return hunger;
        }
        set {
            hunger = Mathf.Min(100, Mathf.Max(0, value));
            hungerBar.fillAmount = hunger / 100f;
            hungerFrame.color = hunger >= 90 ? Color.red : Color.white;
        }
    }

    private float filth = 0;
    private float Filth {
        get {
            return filth;
        }
        set {
            filth = Mathf.Min(100, Mathf.Max(0, value));
            filthBar.fillAmount = filth / 100f;
            filthFrame.color = filth >= 90 ? Color.red : Color.white;
        }
    }

    private int satisfaction = 0;
    private int Satisfaction {
        get {
            return satisfaction;
        }
        set {
            satisfaction = Mathf.Max(0, value);
            satisfactionText.text = satisfaction.ToString();
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

    private const float HUNGER_PER_SECOND = 1.666f;
    private const float HUNGER_HEALTH_LOSS_PER_SECOND = 1f;
    private const float TOILET_PER_SECOND = 1.666f;

    private float accumulatedSatisfaction = 0;
    void Update()
    {
        accumulatedSatisfaction += ((float)currentSatisfactionPerSecond) * Time.deltaTime;
        int satInt = Mathf.FloorToInt(accumulatedSatisfaction);
        Satisfaction += satInt;
        accumulatedSatisfaction -= satInt;

        Filth += (currentFilthPerMinute / 60f) * Time.deltaTime;
        Hunger += HUNGER_PER_SECOND * Time.deltaTime; 
        Toilet += TOILET_PER_SECOND * Time.deltaTime;
        if(Hunger >= 100) {
            Health -= HUNGER_HEALTH_LOSS_PER_SECOND * Time.deltaTime;
        }
        if(Toilet >= 100) {
            MoveableItem itemToCreate = Random.Range(0.0f, 1.0f) > 0.5f ? pissPrefab : crapPrefab;
            Vector3 instantiateLocation = Player.instance.footTransform.position;
            instantiateLocation.y -= 0.3f;
            ComputerUIManager.instance.InstantiateItemType(itemToCreate, instantiateLocation);
            Toilet = 0;
        }
    }

    private List<MoveableItem> activeItems = new List<MoveableItem>();
    private float currentFilthPerMinute = 0;
    private int currentSatisfactionPerSecond = 0;
    public void RegisterItem(MoveableItem newItem) {
        activeItems.Add(newItem);
        Health += newItem.purchaseHealth;
        Toilet += newItem.purchaseExcretion;
        Hunger += newItem.purchaseHunger;
        Filth += newItem.purchaseFilth;
        Satisfaction += newItem.purchaseSatisfaction;
        currentFilthPerMinute += newItem.filthPerMinute;
        currentSatisfactionPerSecond += newItem.satisfactionPerSecond;
    }
}
