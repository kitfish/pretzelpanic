using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }


    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private BaseCounter selectedCounter;
    private bool isMoving;
    private bool canDash;
    private float dashTimer;
    private Vector3 lastInteractDirection;
    private KitchenObject kitchenObject;
    private Rigidbody playerRigidBody;


    private void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        GameInput.Instance.OnDashAction += GameInput_OnDashAction;

        playerRigidBody = GetComponent<Rigidbody>();
    }

    private void GameInput_OnDashAction(object sender, EventArgs e)
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        if (IsMoving() && canDash)
        {
            canDash = false;
            dashTimer = 0.25f;

            float dashDistance = 8f;
            Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
            moveDir.x *= dashDistance;
            moveDir.z *= dashDistance;
            playerRigidBody.AddForce(moveDir,ForceMode.Impulse);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        //if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
        else
        {
            // if player is attempting to throw the held Kitchen Object
            if (kitchenObject != null)
            {
                Rigidbody kitchenObjectRigidBody = kitchenObject.GetComponent<Rigidbody>();

                Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

                float throwDistance = 10f;
                float throwArcHeight = 5f;

                Vector3 throwDirection = new Vector3(inputVector.x, 0f, inputVector.y).normalized;

                kitchenObjectRigidBody.AddForce(throwDirection * throwDistance + Vector3.up * throwArcHeight, ForceMode.Impulse);
                
                kitchenObjectRigidBody.useGravity = true;
                kitchenObject.GetComponent<CapsuleCollider>().enabled = true;
                kitchenObject.SetIsMoving(true);

                kitchenObject.transform.parent = null;
                //kitchenObject.SetKitchenObjectParent(null);
                ClearKitchenObject();
            }
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        //if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        } 
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
        
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDirection = moveDir;
        }

        float interactDistance = 2.0f;
        Debug.DrawRay(transform.position, lastInteractDirection, Color.green);
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has ClearCounter  
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }

    }

    private void HandleMovement()
    {
        if (!canDash)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0)
            {
                canDash = true;
                playerRigidBody.velocity = Vector3.zero;
            }
        }

        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Cannot move towards moveDir

            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // can move only on the z
                    moveDir = moveDirZ;
                }
                else
                {
                    // cannot move in any direction
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        isMoving = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null )
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
