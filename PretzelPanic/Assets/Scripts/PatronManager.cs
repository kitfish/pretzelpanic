using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronManager : MonoBehaviour
{
    [SerializeField] private int patronListSizeMax;
    private List<GameObject> patronListArray;

    public static PatronManager Instance { get; private set; }

    

    private void Awake()
    {
        Instance = this;
        patronListArray = new List<GameObject>(patronListSizeMax);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
    }

    // I dont think we need to use a list and can just put this event into the patron itself
    private void DeliveryManager_OnRecipeCompleted(object sender, DeliveryManager.OnRecipeCompletedEventArgs e)
    {
        if (e.recipeSO == this)
        {

        }

        // send recipe from the onrecipe completed event in delivery manager
        // assign recipe to patrons that are customers
        // scan list for the patron with the first instance of the recipe
        // remove them from list
        // handle making that patron leave/vanish or something
    }
}
