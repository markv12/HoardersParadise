using System.Collections.Generic;
using UnityEngine;

public class InfoCirclePool : MonoBehaviour
{
    public static InfoCirclePool instance;
    public GameObject infoCirclePrefab;

    private void Awake() {
        instance = this;
    }

    private List<InfoCircle> freeInfoCircles = new List<InfoCircle>(16);

    public void ShowInfoCircle(string theText, Vector3 thePosition) {
        ShowInfoCircle(theText, thePosition, Color.black);
    }
    public void ShowInfoCircle(string theText, Vector3 thePosition, Color theColor) {
        InfoCircle newCircle = GetInfoCircle();
        newCircle.PlayAnimation(theText, thePosition, theColor);
    }

    public InfoCircle GetInfoCircle() {
        InfoCircle result;
        if (freeInfoCircles.Count > 0) {
            result = freeInfoCircles[freeInfoCircles.Count - 1];
            freeInfoCircles.RemoveAt(freeInfoCircles.Count - 1);
            result.gameObject.SetActive(true);
            result.t.localScale = new Vector3(1, 1, 1);
        } else {
            result = GetNewInfoCircle();
        }
        return result;
    }

    private InfoCircle GetNewInfoCircle() {
        return Instantiate(infoCirclePrefab).GetComponent<InfoCircle>();
    }

    public void DisposeInfoCircle(InfoCircle infoCircle) {
        infoCircle.t.SetParent(null, false);
        infoCircle.gameObject.SetActive(false);
        freeInfoCircles.Add(infoCircle);
    }
}
