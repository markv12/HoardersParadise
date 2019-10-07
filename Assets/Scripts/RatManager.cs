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

    private void OnCollisionEnter2D(Collision2D collision) {
        Player p = collision.gameObject.GetComponent<Player>();
        if (p != null) {
            StatUIManager.instance.Health -= 15;
            AlertCanvasManager.instance.ShowAlert("As the rat sinks its sharp teeth deep into your flesh, you scream and stomp down on its grimy tail. The wound stings, but what hurts more than anything is your pride. They invaded your sanctuary, subsisting off your crumbs and crafting their foul nests in your belongings. You can't let them get the last laugh...bombs, cats, potpourri, you need to throw everything you have at them!" + System.Environment.NewLine + "Health -15");
        }
    }
}
