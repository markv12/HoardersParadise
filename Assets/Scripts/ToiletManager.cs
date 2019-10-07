using System;
using UnityEngine;

public class ToiletManager : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null) {
            if (StatUIManager.instance.Toilet > 50) {
                StatUIManager.instance.Toilet = 0;
                AlertCanvasManager.instance.ShowAlert("You made it to the bathroom in time?");
            } else {
                AlertCanvasManager.instance.ShowAlert("You don't have to go to the bathroom yet.");
            }
        }
    }
}
