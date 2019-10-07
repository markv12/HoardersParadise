using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class CollisionEventManager : MonoBehaviour
{
    public static CollisionEventManager instance;

    public CanvasGroup mainGroup;
    public TMP_Text eventText;
    public Button positiveButton;
    public Button negativeButton;

    private bool showing;
    public bool Showing {
        get {
            return showing;
        }
        private set {
            showing = value;
            mainGroup.gameObject.SetActive(showing);
        }
    }

    private void Awake() {
        instance = this;
        Showing = false;
        positiveButton.onClick.AddListener(delegate { DoConfirm(); });
        negativeButton.onClick.AddListener(delegate { DoCancel(); });
    }

    private Action confirmAction = null;
    private Action cancelAction = null;
    public void ShowEvent(string eventTitle, Action _confirmAction, Action _cancelAction) {
        confirmAction = _confirmAction;
        cancelAction = _cancelAction;
        eventText.text = eventTitle;

        Showing = true;
    }

    private void Update() {
        if (showing) {
            if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape)) {
                DoCancel();
            } else if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) {
                DoConfirm();
            }
        }
    }

    private void DoCancel() {
        Showing = false;
        cancelAction?.Invoke();
    }

    private void DoConfirm() {
        Showing = false;
        confirmAction?.Invoke();
    }
}
