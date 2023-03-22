using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }


    public event EventHandler OnPickedSomething;
    public event EventHandler OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7.0f;

    private bool isMoving;

    private void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        //GameInput.Instance.
    }


    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2.0f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);
    
        if (!canMove)
        {
            // Attempt movement in x direction
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f).normalized;
            canMove = (moveDirection.x < -0.5f || moveDirection.x > 0.5f) && 
                !Physics.CapsuleCast
                (
                    transform.position, 
                    transform.position + Vector3.up * playerHeight, 
                    playerRadius, 
                    moveDirectionX, 
                    moveDistance
                );
            
            if (canMove)
            {
                moveDirection = moveDirectionX;
            } else
            {
                Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z).normalized;
                canMove = (moveDirection.z < -0.5f || moveDirection.z > 0.5f) &&
                    !Physics.CapsuleCast
                    (
                        transform.position,
                        transform.position + Vector3.up * playerHeight,
                        playerRadius,
                        moveDirectionZ,
                        moveDistance
                    );
            
                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                } else
                {
                    // no available direction to move in
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }

        isMoving = moveDirection != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
}
