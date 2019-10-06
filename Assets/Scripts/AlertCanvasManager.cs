using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertCanvasManager : MonoBehaviour
{
    public static AlertCanvasManager instance;

    public GameObject mainObject;
    public TMP_Text alertText;
    public Button okButton;

    private bool showing;
    public bool Showing {
        get {
            return showing;
        }
        private set {
            showing = value;
            mainObject.SetActive(showing);
            Time.timeScale = showing ? 0 : 1;
        }
    }

    private void Awake() {
        instance = this;
        Showing = false;
        okButton.onClick.AddListener(delegate { DoOK(); });
    }

    public void ShowAlert(string _alertText) {
        alertText.text = _alertText;
        Showing = true;
    }

    private void DoOK() {
        Showing = false;
    }
}
