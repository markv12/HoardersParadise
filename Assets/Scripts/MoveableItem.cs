using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableItem : MonoBehaviour
{
    public string title;
    [TextArea]
    public string description;

    public Sprite sprite;
    public bool canStackOnTop = true;

    public Rigidbody2D rigidBody;
    public Collider2D theCollider;
    public Transform t;
    public IsoSpriteSorting isoSorter;

    public List<MoveableItem> itemsOnTop = new List<MoveableItem>();

    [NonSerialized]
    public bool partOfStack = false;

    private void OnCollisionEnter2D(Collision2D collision) {
        MoveableItem otherItem = collision.gameObject.GetComponent<MoveableItem>();
        if (otherItem != null && !partOfStack) {
            if (itemsOnTop.Count == 0 && otherItem.itemsOnTop.Count == 0) {
                Transform pT = Player.instance.transform;
                float selfToPlayer = Vector3.Distance(t.position, pT.position);
                float otherToPlayer = Vector3.Distance(otherItem.t.position, pT.position);
                if (selfToPlayer > otherToPlayer) {
                    PutItemOnTop(otherItem);
                }
            } else if (itemsOnTop.Count == 0 || otherItem.itemsOnTop.Count == 0) {
                if(itemsOnTop.Count > 0) {
                    PutItemOnTop(otherItem);
                }
            }
        }
    }

    private void PutItemOnTop(MoveableItem otherItem) {
        Destroy(otherItem.theCollider);
        Destroy(otherItem.rigidBody);
        otherItem.isoSorter.Unregister();
        otherItem.t.SetParent(t);
        float y = (itemsOnTop.Count + 1) * 0.2f;
        Vector3 randomJitter = new Vector3(UnityEngine.Random.Range(-0.15f, 0.15f), UnityEngine.Random.Range(0, 0.15f), 0);
        otherItem.t.localPosition = new Vector3(0, y, 0) + randomJitter;
        itemsOnTop.Add(otherItem);
        otherItem.partOfStack = true;
    }
}
