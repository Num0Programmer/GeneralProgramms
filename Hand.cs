using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    /* TODO:
     * 
     * 1. Put this on the off hand
     * 
     * 2. The state of the main hand may be affected by the changes made to the off hand when operations are carried out
     * 
     */

    // Hand inventory
    public bool full = false;

    // Pickup
    public float pickUpRadius = 1.62f;
    public float rightOffset = 1.03f;
    public float forwardOffset = 1.03f;

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.forward * forwardOffset + transform.right * rightOffset + transform.position, pickUpRadius);
    }
    */
}
