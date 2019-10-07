using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    public float Health {
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
    public float Filth {
        get {
            return filth;
        }
        private set {
            filth = Mathf.Min(100, Mathf.Max(0, value));
            filthBar.fillAmount = filth / 100f;
            filthFrame.color = filth >= 90 ? Color.red : Color.white;
        }
    }

    private int satisfaction = 0;
    public int Satisfaction {
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
        StartCoroutine(StartMessageEnumerator());
    }

    public float hungerPerSecond;
    public float hungerHealthLossPerSecond;
    public float toiletPerSecond;

    private float accumulatedSatisfaction = 0;
    void Update()
    {
        accumulatedSatisfaction += ((float)currentSatisfactionPerSecond) * Time.deltaTime;
        int satInt = Mathf.FloorToInt(accumulatedSatisfaction);
        Satisfaction += satInt;
        accumulatedSatisfaction -= satInt;

        Filth += (currentFilthPerMinute / 60f) * Time.deltaTime;
        Hunger += hungerPerSecond * Time.deltaTime; 
        Toilet += toiletPerSecond * Time.deltaTime;
        if(Hunger >= 100) {
            Health -= hungerHealthLossPerSecond * Time.deltaTime;
        }
        if(Toilet >= 100) {
            MoveableItem itemToCreate = Random.Range(0.0f, 1.0f) > 0.5f ? pissPrefab : crapPrefab;
            Vector3 instantiateLocation = Player.instance.footTransform.position;
            instantiateLocation.y -= 0.3f;
            ComputerUIManager.instance.InstantiateItem(itemToCreate, instantiateLocation);
            Toilet = 0;
            AlertCanvasManager.instance.ShowAlert(itemToCreate.purchaseEventText);
        }
        if(Health <= 0) {
            Sprite deadSprite = Player.instance.deadSprite;
            Player.instance.spriteRenderer.sprite = deadSprite;
            Destroy(Player.instance);
            StartCoroutine(DeathMessageEnumerator());
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

    public void UnRegisterItem(MoveableItem itemToRemove) {
        activeItems.Add(itemToRemove);
        currentFilthPerMinute -= itemToRemove.filthPerMinute;
        currentSatisfactionPerSecond -= itemToRemove.satisfactionPerSecond;
    }

    private IEnumerator DeathMessageEnumerator() {
        yield return new WaitForSeconds(1.666f);
        string deathText = deathTexts[UnityEngine.Random.Range(0, deathTexts.Length)];
        deathText += System.Environment.NewLine + System.Environment.NewLine + "Total Satisfaction: " + Satisfaction;
        AlertCanvasManager.instance.ShowAlert(deathText, delegate {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        });
    }

    private IEnumerator StartMessageEnumerator() {
        yield return new WaitForSeconds(1.333f);
        AlertCanvasManager.instance.ShowAlert(startText);
    }

    private static string[] deathTexts = new string[] {
        "Suddenly, you can take no more. Your legs give way, and you fall onto the floor." + System.Environment.NewLine + "Down there, you can see the layers of filth and dirt that cake your house. Like an ancient ruler of the past, this realm was created by your command." + System.Environment.NewLine + "Strangely, you feel relieved. All the urgency immediately evacuates itself from your body. No more struggling, no more worrying. You can now fully embrace the filth you've lived in for so long, returning to dust at long last, like all humans are fated to do." + System.Environment.NewLine + "Closing your eyes, you gently sink into the dirt and fade away.",
        "Suddenly, you trip over a wire and fall. Across the floor, you see some rats chewing on some wires." + System.Environment.NewLine + "Moments later, your house goes up in flames. You want to escape, but your muscles are too withered." + System.Environment.NewLine + "You burn to death midst a horde of flaming objects and vermin.",
        "Suddenly, you hear a loud pop from your spinal column, and you're on the floor." + System.Environment.NewLine + "Intense pain radiates out from your back. You can't get up." + System.Environment.NewLine + "Weeks later, a neighbor complains about a strange stench emanating from your house. The police eventually break in and find your corpse, half-devoured among a horde of rats and cockroaches. Your funeral happens one week later.",
        "Suddenly, you fall to the ground. You feel so hungry...hungry than you've ever been before." + System.Environment.NewLine + "At first, you had your doubts about trying to survive solely on your favorite comfort foods like crackers, soda, and pizza. But you figured it would all turn out alright. You didn't want to worry about it." + System.Environment.NewLine + "Now, eyes wide in horror, you realize you were wrong. As the months' worth of junk food and Ooze soda takes its toll on your body, you feel a horrible pain rising up from your stomach, as if someone was stabbing you with a butcher knife." + System.Environment.NewLine + "You try and hold on, but the pain is too great. In your last moments, you reach out desperately into the darkness, begging for a second chance.",
        "Suddenly, you trip over something and fall to the ground. The next thing you know, you're face to face with a hulking, red-eyed rat." + System.Environment.NewLine + "You try to roll away, but it's too fast. As it rips into your face, a horde of rats charge out from all your room's nooks and crannies, gnashing their teeth as they rip into your flesh. You kick and flail your arms, and although you knock a few away, you simply aren't fast enough." + System.Environment.NewLine + "Soon, horrid squeaks overtake your ears as they ravage you, chewing away every last part of you so that they may go on to survive another day in the filthy playground you've created for them.",
        "Suddenly, you feel a strange sensation underfoot. You look down to see you've accidentally stepped on a big, fat cockroach." + System.Environment.NewLine + "White eggs ooze out from its crushed belly. You can also see that its long antennae have been broken into pieces." + System.Environment.NewLine + "Slowly you back up, only to squish another. This time, you panic and run. But you only manage to run straight into a piece of furniture and fall to the ground." + System.Environment.NewLine + "The next thing you know, they're all over you. Skittering, twitching roaches squirm all over your body, biting you, forcing themselves inside your mouth. You can feel them consuming you, possessing you, eroding your soul with their unbreakable desire to survive. Yes...it all makes sense now. From the very beginning, the cockroaches were working much harder than you. They deserve to go on -- but you've more than run your course." + System.Environment.NewLine + "As the cockroaches rush inside you, you accept them and become their martyr. The cockroaches dig into your insides, slowly devouring every last juicy piece of life. It's okay. You never felt quite right in your skin to begin with.",
        "'Help! I've fallen and I can't get up!'" + System.Environment.NewLine + "Unfortunately, there is no one around to hear you. If only you'd stayed in contact with your friends...or your family, even." + System.Environment.NewLine + "But it was all so stressful and scary. Being alone at home felt so much better than the alternative. In the end, you only wanted to feel safe. You just wanted a place where it was comfortable to be yourself." + System.Environment.NewLine + "Now, you're afraid, because you realize your body has become too enfeebled to move. Was it the filth, the foul stenches, the malnutrition, or the lack of exercise? As you lay still, waiting for the slow death to finally come, you think back on your life, wondering what you did to deserve this...wondering where exactly you went wrong.",
        "Suddenly, midst your mad scramble for acquisition, you stop to take a moment and inhale the scent of your domain." + System.Environment.NewLine + "Silent and still, you hear the skittering of cockroaches and the squeaking of rats. Pungent odors waft out from every angle." + System.Environment.NewLine + "You see all this, and it is good. The time has finally come." + System.Environment.NewLine + "Shedding your Earthly vestments, you nestle down into the filthy cocoon you have created and fuse your alien body with it. Soon, you and the dwelling will become one as you are reborn with ultimate power - as a fully evolved Filthling. Your journey to dominate the Earth has only just begun.",
    };

    private static string startText = "It feels so good to finally have your own house! But it's so empty..." + System.Environment.NewLine + "You should buy yourself some new things to celebrate.You certainly have more than enough money saved up." + System.Environment.NewLine + "You can probably find some good deals on YouBuy.biz!";
}
