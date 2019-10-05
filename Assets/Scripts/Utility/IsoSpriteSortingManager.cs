using System.Collections.Generic;

public class IsoSpriteSortingManager : Singleton<IsoSpriteSortingManager>
{
    private static List<IsoSpriteSorting> floorSpriteList = new List<IsoSpriteSorting>(64);
    private static List<IsoSpriteSorting> moveableSpriteList = new List<IsoSpriteSorting>(64);
    private static List<IsoSpriteSorting> currentlyVisibleMoveableSpriteList = new List<IsoSpriteSorting>(64);
    public static void RegisterSprite(IsoSpriteSorting newSprite)
    {
        if (newSprite.renderBelowAll)
        {
            floorSpriteList.Add(newSprite);
            SortListSimple(floorSpriteList);
            SetSortOrderNegative(floorSpriteList);
        }
        else
        {
            moveableSpriteList.Add(newSprite);
        }
    }

    public static void UnregisterSprite(IsoSpriteSorting spriteToRemove)
    {
        if (spriteToRemove.renderBelowAll)
        {
            floorSpriteList.Remove(spriteToRemove);
        }
        else
        {
            moveableSpriteList.Remove(spriteToRemove);
        }
    }

    void Update()
    {
        UpdateSorting();
    }

    private static List<IsoSpriteSorting> sortedSprites = new List<IsoSpriteSorting>(64);
    public static void UpdateSorting()
    {
        FilterListByVisibility(moveableSpriteList, currentlyVisibleMoveableSpriteList);

        SortListSimple(currentlyVisibleMoveableSpriteList);
        SetSortOrderBasedOnListOrder(currentlyVisibleMoveableSpriteList);
    }

    private static void SetSortOrderBasedOnListOrder(List<IsoSpriteSorting> spriteList)
    {
        int orderCurrent = 0;
        for (int i = 0; i < spriteList.Count; i++)
        {
            spriteList[i].RendererSortingOrder = orderCurrent;
            MoveableItem item = spriteList[i].moveableItem;
            orderCurrent += (1 + (item == null ? 0 : item.itemsOnTop.Count));
        }
    }

    private static void SetSortOrderNegative(List<IsoSpriteSorting> spriteList)
    {
        int startOrder = -spriteList.Count - 1;
        for (int i = 0; i < spriteList.Count; ++i)
        {
            spriteList[i].RendererSortingOrder = startOrder + i;
        }
    }

    public static void FilterListByVisibility(List<IsoSpriteSorting> fullList, List<IsoSpriteSorting> destinationList)
    {
        destinationList.Clear();
        for (int i = 0; i < fullList.Count; i++)
        {
            IsoSpriteSorting sprite = fullList[i];
            if (sprite.forceSort)
            {
                destinationList.Add(sprite);
                sprite.forceSort = false;
            }
            else
            {
                for (int j = 0; j < sprite.renderersToSort.Length; j++)
                {
                    if (sprite.renderersToSort[j].isVisible)
                    {
                        destinationList.Add(sprite);
                        break;
                    }
                }
            }
        }
    }

    private static void SortListSimple(List<IsoSpriteSorting> list)
    {
        list.Sort((a, b) =>
        {
            if (!a || !b)
            {
                return 0;
            }
            else
            {
                return IsoSpriteSorting.CompairIsoSortersBasic(a, b);
            }
        });
    }
}
