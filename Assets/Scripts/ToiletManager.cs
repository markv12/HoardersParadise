using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletManager : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null && StatUIManager.instance.Toilet > 50) {
            Time.timeScale = 0;
            Action confirmAction = delegate {
                StatUIManager.instance.Toilet = 0;
                Time.timeScale = 1;
            };
            Action cancelAction = delegate {
                Time.timeScale = 1;

            };
            CollisionEventManager.instance.ShowEvent("Use the Bathroom?", confirmAction, cancelAction);
        }
    }
}
