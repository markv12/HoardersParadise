using UnityEngine;

public class FrontDoorManager : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null) {
            AlertCanvasManager.instance.ShowAlert("You could go outside...but then you'd have to deal with people...");
        }
    }
}
