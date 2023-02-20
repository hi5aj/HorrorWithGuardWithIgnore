using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //public GameObject Hatchet;
    public bool CanAttack = true;
    public float AttackCooldown= 1.0f;
    //public AudioClip hatchetAttackSound;
    Animator anim;
    AudioSource ac;
    public BoxCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //ac = GetComponent<AudioSource>();
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(CanAttack)
            {
                 HatchetAttack();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
        }
    }
    
    public void HatchetAttack()
    {
        collider.enabled = true;
        CanAttack = false;
        anim.SetBool("isAttacking", true);
        //ac.PlayOneShot(hatchetAttackSound);
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        collider.enabled = false;
        anim.SetBool("isAttacking", false);
        CanAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Breakable")
        {
            Debug.Log("Breaking object");
            Destroy(other.gameObject);
        }
    }

}

