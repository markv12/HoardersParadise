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
    public TMP_Text userNameText;

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
            userNameText.text = "Sold by: " + GetRandomUserName();
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
        string result = Environment.NewLine;
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

    public static string GetNumberString(float number) {
        return number >= 0 ? "+" + number.ToString() : number.ToString();
    }

    private static string GetRandomUserName() {
        currentUsernameIndex = (currentUsernameIndex + UnityEngine.Random.Range(1, 6)) % userNames.Length;
        return userNames[currentUsernameIndex];
    }

    private static int currentUsernameIndex = 0;
    private static string[] userNames = new string[] {
        "comeintomyhouse43",
        "illegalfun_uk",
        "monsterfantasy3",
        "i_am_an_rpgamer",
        "rpvespa2001",
        "logosandpathos",
        "ojdidit1999",
        "xXxDarrrkAngelxXx",
        "bootysmakr",
        "SSJPatrick69",
        "hobbithead2000",
        "lotrloverX",
        "CharMinChiCa^_^",
        "sweetiesurfergurl22",
        "CantTouchDis666",
        "mannkomaster",
        "xUxHaDxMExONcE",
        "oOo[LoUdBrUnEtTe]oOo",
        "catsrule13",
        "passionatescorpio11",
        "nudist_at_home",
        "wiccan_mistress831",
        "master_warlock940",
        "BmyQTplz",
        "o_owoahmano_o",
        "forsaken_princess",
        "shadow_lord",
        "i_love_jesus",
        "christluvsU69",
        "ChristianMommy",
        "NecromancerBobby",
        "wheresthecheese",
        "illkillu",
        "dontcallmeuncle",
        "bytheclovenhoofipray",
        "ilikesports99",
        "CuteChurchLady13",
        "uRkinDaHOTTx18x",
        "sportscenterfan35",
        "sportzdad",
        "ILoveAmerica33",
        "sk8erboi301",
        "agoodman90",
        "wangmeister931043",
        "hail_emperorsapphire6666",
        "rough_and_ready",
        "seductive_gentleman19",
        "xxxSweetestKittyKatxxx",
        "markv13",
        "itsburning_help",
        "1337h4x0r41i43",
        "r0x0rmyb0x0rz",
        "LordOfTheWeed69_96",
        "Robismycousin",
        "x_xdontfriendmex_x",
        "jesusismyhomie",
        "BlueJeanBaby00",
        "KapriciousChaos",
        "delectable_tart",
        "alt_ctrl_del",
        "roflcopterzzz",
        "azn_persuazn717",
        "PrincessMyako",
        "DeathGodxXxXx",
        "sugerbunney8",
        "lizardsrcool",
        "Mastrofpruppets",
        "Elemental_Warlord_LV99",
        "dbzobsessed",
        "LadyOftheUnicorns",
        "The_Buttman15",
        "whosyourdaddy9000",
        "TrixxyDixxy99199",
        "fatz2nasty",
        "2hott4tv",
        "CaliHunny44",
        "GodismyShepard",
        "DoggieLuvr93433123",
        "drugsarekool3",
        "cremdelacrem8008",
        "HoopsMcCann",
        "TimesAreTough64",
        "XxDevilEyesxX",
        "Grandmas4Lyfe",
        "CheezyKrackerKing",
        "BobChikin69314",
        "DrMuscleKilledMyBrother",
        "CoolDude57",
        "iloveclowns69",
        "ConfederateMissy13",
        "CajunPersuasion9",
        "Rockula8888",
        "Lugosihead452",
        "VegasorBustx777x",
        "TheCatzMeooow",
        "iwantadivorce",
        "masseuse_therapist",
        "SkankinPunk4555",
        "nice_girl13",
        "killmenow"
    };
}
