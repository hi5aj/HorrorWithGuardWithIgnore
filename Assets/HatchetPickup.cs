using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchetPickup : MonoBehaviour
{

    public GameObject weaponPickup;
    public GameObject weaponHold;
    public BoxCollider coll;
    public GameObject pc;




    public float pickUpRange;

    private void PickUp()
    {

        Vector3 distanceToPlayer = player.postition - transform.position;
        if(Input.GetKeyDown("E") && distanceToPlayer.magnitude <= pickUpRange)
        {
            Destroy.GameObject("weaponPickup");
            SetActive.GameObject("WeaponHold");
        }
    }
}
