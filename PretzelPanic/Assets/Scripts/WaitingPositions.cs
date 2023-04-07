using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingPositions : MonoBehaviour
{
    [SerializeField] private WaitingPosition[] customerWaitingPositions;

    public static WaitingPositions Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ShuffleCustomersForward();
        }
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, DeliveryManager.OnRecipeCompletedEventArgs e)
    {
        //Transform customerThatRequestedRecipe = MatchRecipeToCustomer(e.recipeSO);
        //MatchRecipeToCustomer(e.recipeSO);
        ShuffleCustomersForward();
    }

    private void MatchRecipeToCustomer(RecipeSO recipeCompleted)
    {
        foreach (WaitingPosition customerPosition in customerWaitingPositions)
        {
            Customer customerAtWaitingPosition = customerPosition.GetWaitingPositionChild().GetComponent<Customer>();
            RecipeSO requestedRecipe = customerAtWaitingPosition.GetCustomerRequestedRecipe();

            if (requestedRecipe == recipeCompleted)
            {
                customerAtWaitingPosition.SetOrderCompleteState();
                customerPosition.ClearWaitingPositionChild();
                return;
            }
        }
    }
    
    private void ShuffleCustomersForward()
    {
        WaitingPosition previousWaitingPosition = customerWaitingPositions[0];
        foreach(WaitingPosition customerPosition in customerWaitingPositions)
        {
            Transform currentCustomerPosition = customerPosition.transform;

            if (currentCustomerPosition.childCount > 1)
            {
                Debug.Log(currentCustomerPosition + ":-> " + currentCustomerPosition.GetChild(1));

                //if ()

            }
            
            //Debug.Log(customerPosition);
            if (previousWaitingPosition == null && customerPosition.GetWaitingPositionChild() != null)
            {
                previousWaitingPosition.SetWaitingPositionChild(customerPosition.GetWaitingPositionChild());
            }

            if(customerPosition.GetWaitingPositionChild() == null)
            {
                previousWaitingPosition = customerPosition;
            }
        }
    }
}
