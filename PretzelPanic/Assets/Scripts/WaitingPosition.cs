using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingPosition : MonoBehaviour
{
    [SerializeField] private Transform waitingPositionChild;

    public void SetWaitingPositionChild(Transform child)
    {
        child.parent = this.transform;
        waitingPositionChild = child;
    }

    public void ClearWaitingPositionChild()
    {
        waitingPositionChild.parent = null;
        waitingPositionChild = null;
    }

    public Transform GetWaitingPositionChild()
    {
        return waitingPositionChild;
    }

}
