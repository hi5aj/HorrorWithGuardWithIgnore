using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KevinAI : MonoBehaviour
{
    public GameObject kevinDest;
    NavMeshAgent kevinAgent;
    public GameObject kevinEnemy;
    public bool isStalking;
    public float cooldown = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Temporarily sets stalking mode to true so Kevin follows player
        //isStalking = true;
        kevinAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // Follows player if isStalking is true
        if (isStalking == false)
        {
            //kevinEnemy.GetComponent<Animator>().Play("Idle");
        }
        else
        {
            kevinAgent.SetDestination(kevinDest.transform.position);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Hatchet")
        {
            Debug.Log("Trying to stun enemy");
            isStalking = false;
            StartCoroutine(StunCooldown());
        }
    }

    IEnumerator StunCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        isStalking = true;
    }
}
