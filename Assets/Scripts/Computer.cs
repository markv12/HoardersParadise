using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null) {
            ComputerUIManager.instance.ShowComputerUI();

            //Action confirmAction = delegate {
            //    Debug.Log("Confirmed");
            //    ComputerUIManager.instance.ShowComputerUI();
            //};
            //Action cancelAction = delegate {
            //    Debug.Log("Canceled");

            //};
            //CollisionEventManager.instance.ShowEvent("Use Your Computer?", confirmAction, cancelAction);
        }
    }
}
