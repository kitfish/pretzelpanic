using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : MonoBehaviour
{
    private RecipeSO patronDesiredRecipeSO;

    private enum State
    {
        GettingInLine,
        WaitingToOrder,
        OrderingFood,
        WaitingForFood,
        Angry,
        Leaving
    }

    private State state;

    private void Update()
    {
        switch (state)
        {
            case State.GettingInLine:
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


}
