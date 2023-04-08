using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Passerby : MonoBehaviour
{
    private enum Directions
    {
        Left,
        Right
    }

    private Rigidbody passerbyRigidBody;
    private Directions directionState;
    private Vector3 moveDirection = Vector3.zero;
    private float moveSpeed = 4f;
    private float sceneBoundaryLeft = -30f;
    private float sceneBoundaryRight = 30f;

    private void Start()
    {
        passerbyRigidBody = GetComponent<Rigidbody>();
        passerbyRigidBody.velocity = moveDirection * 10f;
    }

    private void Update()
    {
        HandleMovement();
    }

    public void AssignDirecton(int direction)
    {
        directionState = (Directions)direction;

        if (directionState == Directions.Left)
        {
            moveDirection = new Vector3(-1, 0, 0);
        }else
        {
            moveDirection = new Vector3(1, 0, 0);
        }
        
    }

    private void HandleMovement()
    {
        //float moveDistance = moveSpeed * Time.deltaTime;

        //transform.position += moveDirection * moveDistance;
        



        if (transform.position.x < sceneBoundaryLeft || transform.position.x > sceneBoundaryRight)
        {
            Destroy(gameObject);
        }
    }

}
