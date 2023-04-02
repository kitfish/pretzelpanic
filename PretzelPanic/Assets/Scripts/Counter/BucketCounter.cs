using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketCounter : BaseCounter
{
    public event EventHandler OnBucketSpawn;
    public event EventHandler OnBucketRemoved;

    [SerializeField] private KitchenObjectSO bucketKitchenObjectSO;

    private float spawnBucketTimer = 0f;
    private float spawnBucketTimerMax = 2.0f;
    private int bucketSpawnAmount = 0;
    private int bucketSpawnAmountMax = 4;

    private void Update()
    {
        spawnBucketTimer += Time.deltaTime;
        if (spawnBucketTimer > spawnBucketTimerMax)
        {
            spawnBucketTimer = 0f;
            if (bucketSpawnAmount < bucketSpawnAmountMax)
            {
                bucketSpawnAmount++;
                OnBucketSpawn?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is empty handed
            if(bucketSpawnAmount > 0)
            {
                // There's at least one bucket here
                bucketSpawnAmount--;

                KitchenObject.SpawnKitchenObject(bucketKitchenObjectSO, player);

                OnBucketRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
