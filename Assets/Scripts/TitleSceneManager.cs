using UnityEngine;
using UnityEngine.UI;
public class TitleSceneManager : MonoBehaviour
{
    public Button startButton;
    void Awake()
    {
        startButton.onClick.AddListener(delegate {
            UnityEngine.SceneManagement.SceneManager.LoadScene("HouseScene");
        });     
    }
}
