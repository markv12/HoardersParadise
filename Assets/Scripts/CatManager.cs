using UnityEngine;

public class CatManager : MonoBehaviour
{
    private Vector2 currentDirection;
    public Rigidbody2D rgdBody;

    private const float MOVEMENT_SPEED = 1.5f;

    private void Awake() {
        currentDirection = GetRandomDirection();
    }

    private static Vector2 GetRandomDirection() {
        return new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * MOVEMENT_SPEED;
    }

    void Update()
    {
        rgdBody.MovePosition(rgdBody.position + (currentDirection * Time.deltaTime));
    }

    private int lastCollisionFrame = 0;
    private void OnCollisionStay2D(Collision2D collision) {
        if (Time.frameCount - lastCollisionFrame > 20) {
            currentDirection = GetRandomDirection();
            lastCollisionFrame = Time.frameCount;
        }
    }
}
