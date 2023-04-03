using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private RecipeSO customerRequestedRecipeSO;

    private enum State
    {
        GettingInLine,
        WaitingToOrder,
        OrderingFood,
        WaitingForFood,
        Angry,
        OrderComplete
    }

    private State state;

    private void Update()
    {
        switch (state)
        {
            case State.GettingInLine:
                break;

                case State.OrderComplete:
                Destroy(gameObject);
                break;
        }
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        state = State.GettingInLine;
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, DeliveryManager.OnRecipeCompletedEventArgs e)
    {
        //if (e.recipeSO) { }
    }

    public RecipeSO GetCustomerRequestedRecipe()
    {
        return customerRequestedRecipeSO;
    }

    public void SetOrderCompleteState()
    {
        state = State.OrderComplete;
    }
}
