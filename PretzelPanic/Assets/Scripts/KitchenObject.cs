using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;
    private bool isMoving = false;

    private void Update()
    {
        if (isMoving)
        {
            
            if(Physics.SphereCast(transform.position, 1f, new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z), out RaycastHit hitInfo))
            {
                kitchenObjectParent = null;
                Debug.Log(hitInfo.transform.name);
                isMoving = false;

                if (hitInfo.transform.TryGetComponent(out BaseCounter baseCounter))
                {
                    //baseCounter.SetKitchenObject(this);
                    if (!baseCounter.HasKitchenObject())
                    {
                        SpawnKitchenObject(kitchenObjectSO, baseCounter);
                        Destroy(gameObject);
                    }

                    //SetKitchenObjectParent(baseCounter);
                    //DestroySelf();
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (isMoving)
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z), 1.0f);
        }
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IKitchenObjectParent already has a Kitchen Object!");
        }

        kitchenObjectParent.SetKitchenObject(this);

        if (kitchenObjectParent != null)
        {
            transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
            transform.localPosition = Vector3.zero;
        }

    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }


    public bool TryGetBucket(out BucketKitchenObject bucketKitchenObject)
    {
        if (this is BucketKitchenObject)
        {
            bucketKitchenObject = this as BucketKitchenObject;
            return true;
        } else
        {
            bucketKitchenObject = null;
            return false;
        }
    }
    

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void SetIsMoving(bool moving)
    {
        isMoving = moving;

        if (!isMoving) 
        {
            // capsule is temp. we will use a sphere for all kitchen objects for ease of interacting
            CapsuleCollider kitchenObjectCapsuleCollider = GetComponent<CapsuleCollider>();
            kitchenObjectCapsuleCollider.enabled = false;
        }
    }
}
