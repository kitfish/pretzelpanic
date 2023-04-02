using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : MonoBehaviour
{
    private RecipeSO patronDesiredRecipeSO;

    private enum TypeOfPatron
    {
        PasserBy,
        Customer,
    }

    private TypeOfPatron typeOfPatron;


    private void Update()
    {
        switch (typeOfPatron)
        {
            case TypeOfPatron.PasserBy:
                HandlePasserBy();
                break;
            case TypeOfPatron.Customer:
                HandleCustomer();
                break;
        }
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, DeliveryManager.OnRecipeCompletedEventArgs e)
    {
        if (e.recipeSO == patronDesiredRecipeSO)
        {
            HandleCustomerLeave();
        }
    }

    private void HandleCustomer()
    {

    }

    private void HandlePasserBy()
    {

    }

    private void HandleCustomerLeave()
    {

    }

    public bool IsCutomer()
    {
        return typeOfPatron == TypeOfPatron.Customer;
    }

    public bool IsPasserBy()
    {
        return typeOfPatron == TypeOfPatron.PasserBy;
    }
}
