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

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) {
            DoOK();
        }
    }

    private System.Action onComplete = null;
    public void ShowAlert(string _alertText, System.Action _onComplete = null) {
        alertText.text = _alertText;
        Showing = true;
        onComplete = _onComplete;
    }

    private void DoOK() {
        Showing = false;
        onComplete?.Invoke();
        onComplete = null;
    }
}
