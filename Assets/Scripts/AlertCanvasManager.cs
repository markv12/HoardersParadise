using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertCanvasManager : MonoBehaviour
{
    public static AlertCanvasManager instance;

    public GameObject mainObject;
    public Transform t;
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
        StartCoroutine(PopAnimation());
    }

    private void DoOK() {
        Showing = false;
        onComplete?.Invoke();
        onComplete = null;
    }

    private const float POP_IN_TIME = 0.1f;
    private const float POP_OUT_TIME = 0.22f;
    private static readonly Vector3 normScale = Vector3.one;
    private static readonly Vector3 popScale = new Vector3(1.06f, 1.06f, 1f);
    private IEnumerator PopAnimation() {
        float elapsedTime = 0;
        float progress = 0;
        while (progress <= 1) {
            elapsedTime += Time.unscaledDeltaTime;
            progress = elapsedTime / POP_IN_TIME;
            t.localScale = Vector3.Lerp(normScale, popScale, progress);
            yield return null;
        }
        t.localScale = popScale;
        elapsedTime = 0;
        progress = 0;
        while (progress <= 1) {
            elapsedTime += Time.unscaledDeltaTime;
            progress = elapsedTime / POP_OUT_TIME;
            t.localScale = Vector3.Lerp(popScale, normScale, progress);
            yield return null;
        }
        t.localScale = normScale;
    }
}
