using System;
using System.Collections.Generic;
using UnityEngine;

public class IsoSpriteSorting : MonoBehaviour
{
    public bool renderBelowAll;

    public MoveableItem moveableItem;

    [NonSerialized]
    public bool forceSort;

    [NonSerialized]
    public List<IsoSpriteSorting> staticDependencies = new List<IsoSpriteSorting>(16);
    [NonSerialized]
    public List<IsoSpriteSorting> inverseStaticDependencies = new List<IsoSpriteSorting>(16);
    public List<IsoSpriteSorting> movingDependencies = new List<IsoSpriteSorting>(8);

    private readonly List<IsoSpriteSorting> visibleStaticDependencies = new List<IsoSpriteSorting>(16);
    private List<IsoSpriteSorting> activeDependencies = new List<IsoSpriteSorting>(16);
    public List<IsoSpriteSorting> ActiveDependencies
    {
        get
        {
            activeDependencies.Clear();
            IsoSpriteSortingManager.FilterListByVisibility(staticDependencies, visibleStaticDependencies);
            activeDependencies.AddRange(visibleStaticDependencies);
            activeDependencies.AddRange(movingDependencies);
            return activeDependencies;
        }
    }

    public Vector3 SorterPositionOffset = new Vector3();

    private Transform t;

    private Vector3 SortingPoint1
    {
        get
        {
            return SorterPositionOffset + t.position;
        }
    }

    public bool IsNear(Vector3 point, float distance)
    {
        return Vector2.Distance(point, SortingPoint1) <= distance;
    }

    public Renderer[] renderersToSort;

#if UNITY_EDITOR
    public void SortScene()
    {
        IsoSpriteSorting[] isoSorters = FindObjectsOfType(typeof(IsoSpriteSorting)) as IsoSpriteSorting[];
        for (int i = 0; i < isoSorters.Length; i++)
        {
            isoSorters[i].Setup();
        }
        IsoSpriteSortingManager.UpdateSorting();
        for (int i = 0; i < isoSorters.Length; i++)
        {
            isoSorters[i].Unregister();
        }
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
#endif

    void Awake()
    {
        if (Application.isPlaying)
        {
            IsoSpriteSortingManager temp = IsoSpriteSortingManager.Instance; //bring the instance into existence so the Update function will run;
            Setup();
        }
    }

    private void Setup()
    {
        t = transform;
        if (renderersToSort == null || renderersToSort.Length == 0)
        {
            renderersToSort = new Renderer[] { GetComponent<Renderer>() };
        }
        Register();
        System.Array.Sort(renderersToSort, (a, b) => a.sortingOrder.CompareTo(b.sortingOrder));
    }

    public static int CompairIsoSortersBasic(IsoSpriteSorting sprite1, IsoSpriteSorting sprite2)
    {
        float y1 = sprite1.SortingPoint1.y;
        float y2 = sprite2.SortingPoint1.y;
        return y2.CompareTo(y1);
    }

    public int RendererSortingOrder
    {
        get
        {
            if (renderersToSort.Length > 0)
            {
                return renderersToSort[0].sortingOrder;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            for (int j = 0; j < renderersToSort.Length; ++j)
            {
                renderersToSort[j].sortingOrder = value;
            }
            if (moveableItem != null) {
                int itemOnTopCount = moveableItem.itemsOnTop.Count;
                for (int i = 0; i < itemOnTopCount; i++) {
                    moveableItem.itemsOnTop[i].isoSorter.RendererSortingOrder = value + (i + 1);
                }
                if(moveableItem.infoCircle != null) {
                    moveableItem.infoCircle.backgroundImage.sortingOrder = (value + itemOnTopCount + 1);
                    moveableItem.infoCircle.infoText.sortingOrder = (value + itemOnTopCount + 2);
                }
            }
        }
    }

    void OnDestroy()
    {
        if (Application.isPlaying)
        {
            Unregister();
        }
    }

    public void Register() {
        IsoSpriteSortingManager.RegisterSprite(this);
    }

    public void Unregister()
    {
        IsoSpriteSortingManager.UnregisterSprite(this);
    }
}
