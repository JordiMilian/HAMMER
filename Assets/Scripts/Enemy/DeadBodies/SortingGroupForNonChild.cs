using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SortingGroupForNonChild : MonoBehaviour
{
    [SerializeField] SortingGroup ParentSortingGroup;
    [SerializeField] SortingGroup OwnSortingGroup;

    private void Update()
    {
        MatchSorting();   
    }
    void MatchSorting()
    {
        OwnSortingGroup.sortingLayerID = ParentSortingGroup.sortingLayerID;
        OwnSortingGroup.sortingLayerName = ParentSortingGroup.sortingLayerName;
        OwnSortingGroup.sortingOrder = ParentSortingGroup.sortingOrder;
    }
}
