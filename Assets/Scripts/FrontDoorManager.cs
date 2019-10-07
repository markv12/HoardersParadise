using UnityEngine;

public class FrontDoorManager : MonoBehaviour
{
    private bool hasShownMessage = false;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (!hasShownMessage) {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null) {
                AlertCanvasManager.instance.ShowAlert("You could go outside...but then you'd have to deal with people...");
                hasShownMessage = true;
            }
        }
    }
}
