using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableItem : MonoBehaviour
{
    public string title;
    [TextArea]
    public string websiteDescription;
    [TextArea]
    public string purchaseEventText;

    public Sprite sprite;
    public bool stackable = true;
    public bool canBeOnTopOfOtherThings = true;

    public Rigidbody2D rigidBody;
    public Collider2D theCollider;
    public Transform t;
    public IsoSpriteSorting isoSorter;

    public Transform stackingLocation;

    public float purchaseHunger;
    public float purchaseExcretion;
    public float purchaseHealth;
    public float purchaseFilth;
    public float filthPerMinute;
    public int purchaseSatisfaction;
    public int satisfactionPerSecond;

    public List<MoveableItem> itemsOnTop = new List<MoveableItem>();

    [NonSerialized]
    public bool partOfStack = false;
    private int lastStackStatusChangeFrame = 0;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (Time.frameCount - lastStackStatusChangeFrame > 10) {
            MoveableItem otherItem = collision.gameObject.GetComponent<MoveableItem>();
            if (otherItem != null &&
                !partOfStack &&
                stackable &&
                otherItem.stackable &&
                Time.frameCount - otherItem.lastStackStatusChangeFrame > 10) {
                if (itemsOnTop.Count == 0 && otherItem.itemsOnTop.Count == 0) {
                    if (!canBeOnTopOfOtherThings && otherItem.canBeOnTopOfOtherThings) {
                        PutItemOnTop(otherItem);
                    }
                    if (canBeOnTopOfOtherThings && otherItem.canBeOnTopOfOtherThings) {
                        Transform pT = Player.instance.transform;
                        float selfToPlayer = Vector3.Distance(t.position, pT.position);
                        float otherToPlayer = Vector3.Distance(otherItem.t.position, pT.position);
                        if (selfToPlayer > otherToPlayer) {
                            PutItemOnTop(otherItem);
                        }
                    }
                } else if (itemsOnTop.Count == 0 || otherItem.itemsOnTop.Count == 0) {
                    if (itemsOnTop.Count > 0 && otherItem.canBeOnTopOfOtherThings) {
                        PutItemOnTop(otherItem);
                    }
                }
            }
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && itemsOnTop.Count > 0) {
                CollapseStack(player.footTransform.position);
            }
        }
    }

    private void PutItemOnTop(MoveableItem otherItem) {
        lastStackStatusChangeFrame = Time.frameCount;
        otherItem.lastStackStatusChangeFrame = Time.frameCount;
        otherItem.DestroyPhysics();
        otherItem.isoSorter.Unregister();
        otherItem.t.SetParent(t);
        float y = (itemsOnTop.Count + 1) * 0.2f;
        Vector3 randomJitter = new Vector3(UnityEngine.Random.Range(-0.15f, 0.15f), UnityEngine.Random.Range(0, 0.15f), 0);
        Vector3 stackPos = stackingLocation == null ? Vector3.zero : stackingLocation.localPosition;
        otherItem.t.localPosition = stackPos + new Vector3(0, y, 0) + randomJitter;
        itemsOnTop.Add(otherItem);
        otherItem.partOfStack = true;
    }

    private void CollapseStack(Vector3 collapseSource) {
        lastStackStatusChangeFrame = Time.frameCount;
        Vector3 collapseDirection = (collapseSource - t.position).normalized;
        for (int i = 0; i < itemsOnTop.Count; i++) {
            MoveableItem item = itemsOnTop[i];
            item.RestorePhysics();
            item.t.localPosition += (collapseDirection*1.2f) * (i + 1);
            item.isoSorter.Register();
            item.t.SetParent(null);
            item.partOfStack = false;
            item.lastStackStatusChangeFrame = Time.frameCount;
        }
        itemsOnTop.Clear();
    }

    private void DestroyPhysics() {
        rgdBodyLinearDrag = rigidBody.drag;
        if (theCollider is BoxCollider2D) {
            BoxCollider2D theBox = theCollider as BoxCollider2D;
            boxColliderOffset = theBox.offset;
            boxColliderSize = theBox.size;
        }
        if (theCollider is PolygonCollider2D) {
            PolygonCollider2D thePolygon = theCollider as PolygonCollider2D;
            polygonColliderPath = thePolygon.GetPath(0);
        }
        Destroy(theCollider);
        Destroy(rigidBody);
    }

    private void RestorePhysics() {
        rigidBody = gameObject.AddComponent<Rigidbody2D>();
        rigidBody.mass = rgdBodyMass;
        rigidBody.drag = rgdBodyLinearDrag;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidBody.gravityScale = 0;
        if (boxColliderOffset != Vector2.zero) {
            theCollider = gameObject.AddComponent<BoxCollider2D>();
            theCollider.offset = boxColliderOffset;
            (theCollider as BoxCollider2D).size = boxColliderSize;
        }
        if(polygonColliderPath != null) {
            theCollider = gameObject.AddComponent<PolygonCollider2D>();
            (theCollider as PolygonCollider2D).SetPath(0, polygonColliderPath);
        }
    }

    private float rgdBodyMass;
    private float rgdBodyLinearDrag;
    private Vector2 boxColliderOffset = Vector2.zero;
    private Vector2 boxColliderSize = Vector2.zero;
    private Vector2[] polygonColliderPath = null;
}
