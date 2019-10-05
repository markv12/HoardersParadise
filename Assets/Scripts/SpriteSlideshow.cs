using UnityEngine;

public class SpriteSlideshow : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float timePerImage;
    public Sprite[] images;

    private float elapsedTime = 0;
    private int currentIndex = 0;
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= timePerImage) {
            currentIndex = (currentIndex + 1) % images.Length;
            spriteRenderer.sprite = images[currentIndex];
            elapsedTime = 0;
        }
    }
}
