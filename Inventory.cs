using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /* TODO:
     * 1. Make a script known as "Backpack", this will be the inventory (open)
     *  - AddItem() and RemoveItem() should accept the GameObject the inItem and outItems are to come to and from, respectively.
     *
     * 2. Edit the rocket launcher's length
     *
     * 3. Setup a second hand that will appear when the player is dual wielding (open)
     *  - Also, put in dual wielding
     *       1. Uses an off-hand system
     *       
     * 4. Have the player scroll through their inventory (open)
     *  - The player should also be able to hotkey a couple items (open)
     *  
     * 5. Add objects to backpack array instead of having spaces
    */

    // Components and Assignables

    // Backpack
    public GameObject backPack;
    Backpack backPackScript;

    // Hands
    // Main hand
    public GameObject hand;
    Hand handScript;

    // Off hand
    //public Transform offHand;

    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        backPackScript = backPack.GetComponent<Backpack>();
        handScript = hand.GetComponent<Hand>();
    }

    // Valid objects to go in inventory - defined by tags
    List<string> validTags = new List<string>() { "Weapon", "Item", "Consumable", "Munition" };

    // Misc vars
    float kickOutForce = 20.0f;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(hand.transform.position, handScript.pickUpRadius);

        foreach (Collider collider in colliders)
        {
            if (validTags.Contains(collider.tag))
            {
                if (Input.GetKey(KeyCode.F))
                {
                    if (AddItemToPack(collider.gameObject, SearchPack("empty"))) break;
                }
            }
        }

        // Stops Unity from throwing an out of bounds error
        try
        {
            if (Input.GetKeyDown(KeyCode.V) && !handScript.full)
            {
                EquipToHand(backPackScript.transform.GetChild(0).gameObject, SearchPack("full"));
            }
            // Returns current item to inventory
            else if (Input.GetKeyDown(KeyCode.F) && handScript.full)
            {
                if (AddItemToPack(hand.transform.GetChild(0).gameObject, SearchPack("empty")))
                {
                    handScript.full = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Period) && handScript.full)
            {
                if (AddItemToPack(hand.transform.GetChild(0).gameObject, SearchPack("empty")))
                {
                    handScript.full = false;
                    EquipToHand(backPack.transform.GetChild(0).gameObject, SearchPack("full"));
                }
            }
            // Removes item from hand
            else if (Input.GetKeyDown(KeyCode.V) && handScript.full)
            {
                RemoveItemFromHand(hand.transform.GetChild(0).gameObject);
            }
        }
        catch (UnityException) { /* purposefully empty */ };
    }

    private bool AddItemToPack(GameObject inItem, int index)
    {
        if (!backPackScript.full[index])
        {
            inItem.transform.SetParent(backPack.transform);
            inItem.gameObject.SetActive(false);
            inItem.GetComponent<Collider>().enabled = false;

            inItem.GetComponent<Rigidbody>().isKinematic = true;
            inItem.transform.position = backPack.transform.position;
            inItem.transform.rotation = backPack.transform.rotation;

            backPackScript.full[index] = true;
            return true;
        }

        return false;
    }

    private void EquipToHand(GameObject inItem, int index)
    {
        inItem.transform.SetParent(hand.transform);

        inItem.transform.position = hand.transform.position;
        inItem.transform.rotation = hand.transform.rotation;

        backPackScript.full[index] = false;

        inItem.gameObject.SetActive(true);

        handScript.full = true;
    }

    public void RemoveItemFromHand(GameObject outItem)
    {
        outItem.GetComponent<Rigidbody>().isKinematic = false;
        outItem.GetComponent<Rigidbody>().AddForce(hand.transform.right * kickOutForce, ForceMode.Impulse);

        handScript.full = false;

        outItem.GetComponent<Collider>().enabled = true;
        outItem.transform.SetParent(null);
    }

    private int SearchPack(string type)
    {
        for (int s = 0; s < backPackScript.full.Length; s++)
        {
            if (type == "empty" && !backPackScript.full[s])
            {
                return s;
            }
            else if (type == "full" && backPackScript.full[s])
            {
                return s;
            }
        }

        return 0;
    }
}
