using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rgdBody;
    public SpriteRenderer spriteRenderer;
    public Transform spriteTransform;

    public Sprite rightSprite;
    public Sprite upSprite;
    public Sprite downSprite;

    public static Player instance;

    private const float WALKING_SPEED = 5f;
    // Start is called before the first frame update
    public Bounds PlayerBounds {
        get {
            return spriteRenderer.bounds;
        }
    }

    private void Awake() {
        instance = this;
    }

    [NonSerialized]
    public float inputX = 0;
    [NonSerialized]
    public float inputY = 0;
    void FixedUpdate()
    {
        if (!CollisionEventManager.instance.Showing) {
            inputX = 0;
            inputY = 0;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                inputX -= 1;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                inputX += 1;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                inputY -= 1;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                inputY += 1;
            }
            Vector2 movement = new Vector2(inputX, inputY) * Time.fixedDeltaTime * WALKING_SPEED;
            rgdBody.MovePosition(rgdBody.position + movement);
            //rgdBody.position += movement;

            if (inputX != 0) {
                spriteRenderer.sprite = rightSprite;
                if (inputX < 0) {
                    spriteTransform.localScale = new Vector3(-1, 1, 1);
                } else if (inputX > 0) {
                    spriteTransform.localScale = new Vector3(1, 1, 1);
                }
            } else {
                if (inputY < 0) {
                    spriteRenderer.sprite = downSprite;
                } else if (inputY > 0) {
                    spriteRenderer.sprite = upSprite;
                }
            }
        }
    }
}
