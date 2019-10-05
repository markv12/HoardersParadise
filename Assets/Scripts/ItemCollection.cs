using UnityEngine;
[CreateAssetMenu(fileName = "NewItemCollection", menuName = "Item Collection", order = 101)]
public class ItemCollection : ScriptableObject {
    public MoveableItem[] items;
}
