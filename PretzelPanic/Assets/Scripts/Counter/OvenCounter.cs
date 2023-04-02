using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Baking,
        Baked,
        Burned
    }

    [SerializeField] private BakingRecipeSO[] bakingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;


    private State state;
    private float bakingTimer;
    private BakingRecipeSO bakingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;

                case State.Baking:

                    bakingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = bakingTimer / bakingRecipeSO.bakingTimerMax
                    });

                    if (bakingTimer > bakingRecipeSO.bakingTimerMax)
                    {
                        // Baked
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(bakingRecipeSO.output, this);

                        state = State.Baked;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;

                case State.Baked:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        // Burned
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;

                case State.Burned:
                    break;

            }
        }
    }

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
                    // Player carrying something that can be Baked
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    bakingRecipeSO = GetBakingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Baking;
                    bakingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = bakingTimer / bakingRecipeSO.bakingTimerMax
                    });
                }
            }
            else
            {
                // Player not carrying anything
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

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                }); ;
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        BakingRecipeSO bakingRecipeSO = GetBakingRecipeSOWithInput(inputKitchenObjectSO);
        return bakingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        BakingRecipeSO bakingRecipeSO = GetBakingRecipeSOWithInput(inputKitchenObjectSO);
        if (bakingRecipeSO != null)
        {
            return bakingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private BakingRecipeSO GetBakingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BakingRecipeSO bakingRecipeSO in bakingRecipeSOArray)
        {
            if (bakingRecipeSO.input == inputKitchenObjectSO)
            {
                return bakingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputkitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputkitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsBaked()
    {
        return state == State.Baked;
    }
}
