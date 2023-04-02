using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoldCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyKnead;

    new public static void ResetStaticData()
    {
        OnAnyKnead = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnKnead;

    [SerializeField] private MoldRecipeSO[] moldRecipeSOArray;

    private int moldingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no Kitchen Object here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player is carrying something that can be Molded
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    moldingProgress = 0;

                    MoldRecipeSO moldRecipeSO = GetMoldRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)moldingProgress / moldRecipeSO.moldProgressMax
                    });
                }
            }
            else
            {
                // Player is not carrying anything
            }
        }
        else
        {
            // There is a Kitchen Object here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetBucket(out BucketKitchenObject bucketKitchenObject))
                {
                    // Player is holding a Bucket
                    if (bucketKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // There is a Kitchen Object here AND it can be molded
            moldingProgress++;
            MoldRecipeSO moldRecipeSO = GetMoldRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnKnead?.Invoke(this, EventArgs.Empty);
            OnAnyKnead?.Invoke(this, EventArgs.Empty);

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)moldingProgress / moldRecipeSO.moldProgressMax
            });

            if (moldingProgress >= moldRecipeSO.moldProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);

            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        MoldRecipeSO moldRecipeSO = GetMoldRecipeSOWithInput(inputKitchenObjectSO);
        return moldRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        MoldRecipeSO moldRecipeSO = GetMoldRecipeSOWithInput(inputKitchenObjectSO);
        if(moldRecipeSO != null)
        {
            return moldRecipeSO.output;
        } else
        {
            return null;
        }
    }

    private MoldRecipeSO GetMoldRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (MoldRecipeSO moldRecipeSO in moldRecipeSOArray)
        {
            if (moldRecipeSO.input == inputKitchenObjectSO)
            {
                return moldRecipeSO;
            }
        }
        return null;
    }
}
