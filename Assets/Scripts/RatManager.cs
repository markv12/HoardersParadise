using UnityEngine;

public class RatManager : MonoBehaviour
{
    private Vector2 currentDirection;
    public Rigidbody2D rgdBody;
    public Transform spriteTransform;

    private const float MOVEMENT_SPEED = 3f;

    private void Awake() {
        currentDirection = GetRandomDirection();
    }

    private static Vector2 GetRandomDirection() {
        return new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * MOVEMENT_SPEED;
    }

    void Update()
    {
        rgdBody.MovePosition(rgdBody.position + (currentDirection * Time.deltaTime));
        spriteTransform.localScale = currentDirection.x > 1 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    private int lastCollisionFrame = 0;
    private void OnCollisionStay2D(Collision2D collision) {
        if (Time.frameCount - lastCollisionFrame > 20) {
            currentDirection = GetRandomDirection();
            lastCollisionFrame = Time.frameCount;
        }
    }
}
