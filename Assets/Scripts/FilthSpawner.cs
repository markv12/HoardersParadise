using System.Collections;
using UnityEngine;

public class FilthSpawner : MonoBehaviour
{
    public StatUIManager statUIManager;
    public MoveableItem ratPrefab;
    public Transform filthCreatureSpawnPosition;

    public GameObject[] theRats;

    private static readonly WaitForSeconds theWait = new WaitForSeconds(1.5f);
    IEnumerator Start()
    {
        theRats = new GameObject[5];
        for (int i = 0; i < theRats.Length; i++) {
            GameObject newRat = SpawnFilthCreature();
            newRat.SetActive(false);
            theRats[i] = newRat;
        }
        while (true) {
            yield return theWait;
            float filth = statUIManager.Filth;
            int ratCount = Mathf.FloorToInt(filth / 20f);
            for (int i = 0; i < ratCount; i++) {
                GameObject rat = theRats[i];
                if (!rat.activeSelf) {
                    rat.SetActive(true);
                }
            }
            for (int i = ratCount; i < theRats.Length; i++) {
                GameObject rat = theRats[i];
                if (rat.activeSelf) {
                    rat.SetActive(false);
                }
            }
        }
    }

    private GameObject SpawnFilthCreature() {
        GameObject newRat = Instantiate(ratPrefab.gameObject);
        newRat.transform.position = filthCreatureSpawnPosition.position;
        StatUIManager.instance.RegisterItem(newRat.GetComponent<MoveableItem>());
        return newRat;
    }
}
