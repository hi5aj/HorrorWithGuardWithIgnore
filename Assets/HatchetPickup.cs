using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchetPickup : MonoBehaviour
{

    public static bool armed = false;
    public GameObject weaponHold;
    public GameObject weaponPickup;

    void OnTriggerEnter(Collider other)
    {
        if(armed == false)
        {
        PlayerMovement controller = other.GetComponent<PlayerMovement>();
        weaponHold.SetActive(true);
        weaponPickup.SetActive(false);
        armed = true;
        }


    }
}
