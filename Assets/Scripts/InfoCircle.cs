using UnityEngine;
using TMPro;
using System.Collections;

public class InfoCircle : MonoBehaviour
{
    public Transform t;
    public TextMeshPro infoText;

    public void PlayAnimation(string theText, Vector3 thePosition, Color theColor) {
        infoText.text = theText;
        t.position = thePosition;
        infoText.color = theColor;
        StartCoroutine(_animation());
    }
    private const float ANIM_TIME = 1.5f;
    private const float COLOR_FADE_TIME = 0.4f;
    private IEnumerator _animation() {
        float elapsedTime = 0;
        float progress = 0;
        Vector3 startPos = t.position;
        Vector3 endPos = startPos + new Vector3(0, 0.8f, 0);
        while (progress <= 1) {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / ANIM_TIME;
            t.position = Vector3.Lerp(startPos, endPos, Easing.easeOutSine(0, 1, progress));
            yield return null;
        }
        t.localPosition = endPos;
        elapsedTime = 0;
        progress = 0;
        Color startColor = infoText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        while (progress <= 1) {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / COLOR_FADE_TIME;
            infoText.color = Color.Lerp(startColor, endColor, Easing.easeInOutSine(0, 1, progress));
            yield return null;
        }
        infoText.color = endColor;
        InfoCirclePool.instance.DisposeInfoCircle(this);
    }
}
