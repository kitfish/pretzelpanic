using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetBucket(out BucketKitchenObject bucketKitchenObject))
            {
                // Currently only accepts buckets. 

                //DeliveryManager.Instance.DeliverRecipe(bucketKitchenObject);
                player.GetKitchenObject().DestroySelf();
            }

            // TODO include a way to accept drinks
        }
    }
}
