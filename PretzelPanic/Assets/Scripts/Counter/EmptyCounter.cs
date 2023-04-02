using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        // do something when we interact with the counter


        if (!HasKitchenObject())
        {
            // No kitchen object present on counter
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
                }else
                {
                    // Player is not carrying a Bucket but something else
                    if (GetKitchenObject().TryGetBucket(out bucketKitchenObject))
                    {
                        // Counter is holding a Bucket
                        if (bucketKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            } else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
