using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;
    private bool isMoving = false;
    private float sphereCastRadius = 0.75f;
    private float sphereCastOffsetY = 0.25f;

    private void Update()
    {
 

        if (isMoving)
        {
            kitchenObjectParent = null;
            //if(Physics.CapsuleCast(transform.position, transform.position + Vector3.up, sphereCastRadius, Vector3.down, out RaycastHit hitInfo))
            if (Physics.SphereCast(transform.position, sphereCastRadius, new Vector3(transform.position.x, transform.position.y - sphereCastOffsetY, transform.position.z), out RaycastHit hitInfo))
            {
                
                //Debug.Log(hitInfo.transform.name);
                isMoving = false;

                if (hitInfo.transform.TryGetComponent(out BaseCounter baseCounter))
                {
                    if (baseCounter.TryGetComponent(out TrashCounter trashCounter) || baseCounter.TryGetComponent(out ContainerCounter containerCounter))
                    {
                        Destroy(gameObject);
                        return;
                    }


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

        if (kitchenObjectParent != null)
        {
            transform.position = kitchenObjectParent.GetKitchenObjectFollowTransform().position;
        }

    }

    private void OnDrawGizmos()
    {
        if (isMoving)
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - sphereCastOffsetY, transform.position.z), sphereCastRadius);
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
