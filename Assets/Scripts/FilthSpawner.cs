using System.Collections;
using UnityEngine;

public class FilthSpawner : MonoBehaviour
{
    public StatUIManager statUIManager;
    public MoveableItem ratPrefab;
    public Transform filthCreatureSpawnPosition;

    private static readonly WaitForSeconds theWait = new WaitForSeconds(25);
    IEnumerator Start()
    {
        while (true) {
            yield return theWait;
            float filth = statUIManager.Filth;
            if(UnityEngine.Random.Range(0.0f, 100.0f) < filth) {
                SpawnFilthCreature();
            }
        }
    }

    private void SpawnFilthCreature() {
        ComputerUIManager.instance.InstantiateItem(ratPrefab, filthCreatureSpawnPosition.position);
    }
}
