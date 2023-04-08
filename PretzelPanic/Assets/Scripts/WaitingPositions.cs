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
                
                

            }
            
            // this code will go into the if statement above since we are checking to see if the waiting
            // position has more than 1 child. the first child is always the plane object
            // if a second child exists, it is the customer
            if (previousWaitingPosition == null && customerPosition.GetWaitingPositionChild() != null)
            {
                // review setwaitingposition and getwaitingposition when not so tired
                // they might need some checks for null or if multiple children exist too
                previousWaitingPosition.SetWaitingPositionChild(customerPosition.GetWaitingPositionChild());
            }

            if(customerPosition.GetWaitingPositionChild() == null)
            {
                previousWaitingPosition = customerPosition;
            }
        }
    }
}
