using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /* TODO:
     1. Separate Inventory and HotBar scripts
       - Every picked up item should go in the inventory first
       - Make a script known as "Backpack", this will be the inventory
    
     2. Edit the rocket launcher's length
    
     3. Setup a second hand that will appear when the player is dual wielding
       - Also, put in dual wielding
        - Uses an off-hand system
    */

    // Components and Assignables
    public static Inventory instance;
    public Camera cam;

    private void Awake()
    {
        instance = this;
    }

    // Hand inventory
    public bool full = false;
    public GameObject hand /*, off_hand */;

    // Inventory
    public bool[] iFull;
    public GameObject[] iSlots;

    // Valid objects to go in inventory - defined by tags
    List<string> validTags = new List<string>() { "Weapon", "Item", "Consumable", "Munition" };

    // Pickup
    public float pickUpRadius = 1.62f;

    // Misc vars
    float kickOutForce = 20.0f;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(hand.transform.position, pickUpRadius);

        foreach(Collider collider in colliders)
        {
            // Check gameObject attached to collider for proper tag
            if (validTags.Contains(collider.tag))
            {
                if (Input.GetKey(KeyCode.F))
                {
                    if (!full)
                    {
                        AddItem(collider.gameObject);
                    }
                    // Swap current item and inItem
                    else if(full && (collider.transform.name != hand.transform.GetChild(0).transform.name))
                    {
                        SwitchItems(collider.gameObject, hand.transform.GetChild(0).gameObject);
                    }
                }
            }   
        }

        // Stops Unity from throwing an out of bounds error
        try
        {
            // Removes item from hand
            if (Input.GetKeyDown(KeyCode.V))
            {
                RemoveItem(hand.transform.GetChild(0).gameObject);
            }
        }
        catch (UnityException) { /* purposefully empty */ };
    }

    // Fix duplicate code later
    private void AddItem(GameObject inItem)
    {
        inItem.transform.SetParent(hand.transform);
        inItem.GetComponent<Collider>().enabled = false;

        inItem.GetComponent<Rigidbody>().isKinematic = true;
        inItem.transform.position = hand.transform.position;
        inItem.transform.rotation = hand.transform.rotation;

        full = true;
    }

    public void RemoveItem(GameObject outItem)
    {
        outItem.GetComponent<Rigidbody>().isKinematic = false;
        outItem.GetComponent<Rigidbody>().AddForce(hand.transform.right * kickOutForce, ForceMode.Impulse);

        full = false;

        outItem.GetComponent<Collider>().enabled = true;
        outItem.transform.SetParent(null);
    }

    private void SwitchItems(GameObject inItem, GameObject outItem)
    { 
        RemoveItem(outItem);
        AddItem(inItem);
    }
}
