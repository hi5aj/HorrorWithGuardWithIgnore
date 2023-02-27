using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //public GameObject Hatchet;
    public bool CanAttack;
    public float AttackCooldown = 0.05f;
    public float attackCooldownNormal = 3f;
    //public AudioClip hatchetAttackSound;
    Animator anim;
    AudioSource ac;
    public BoxCollider collider;
    public GameObject reloadText;
    public AudioSource audiosource;
    public AudioClip popSound;
    public AudioClip reloadSound;
    public AudioClip swingSound;
    public AudioClip breakSound;
    CoreAI ai;

    // Start is called before the first frame update
    void Start()
    {
        CanAttack = true;
        anim = GetComponent<Animator>();
        //ac = GetComponent<AudioSource>();
        collider.enabled = false;
        ai = GameObject.Find("Enemy").GetComponent<CoreAI>();
    }

    // Update is called once per frame
    void  FixedUpdate()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            if (CanAttack == true)
            {
                HatchetAttack();
            }
            
        }
        if (CanAttack == false)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
            }
            /*if (Input.GetMouseButton(1))
            {
                StartCoroutine(ReturnToIdle());
            }*/
        }
        /*if (CanAttack == false)
        {
            //reloadText.SetActive(true);
            StartCoroutine(ReturnToIdle());
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
            }

        }*/
    }
    
    public void HatchetAttack()
    {
        CanAttack = false;
        collider.enabled = true;
        StartCoroutine(ResetAttackCooldown());
        anim.SetTrigger("attacking");
        //ac.PlayOneShot(hatchetAttackSound);
        //StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator Reload()
    {
        anim.SetTrigger("reloading");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        CanAttack = true;
        //reloadText.SetActive(false);
    }

    IEnumerator ResetAttackCooldown()
    {

        yield return new WaitForSeconds(AttackCooldown);
        collider.enabled = false;
    }

    IEnumerator ReturnToIdle()
    {
        anim.SetTrigger("returnToIdle");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        CanAttack = true;
    }

    void playPop()
    {
        audiosource.PlayOneShot(popSound);

    }
    void playReload()
    {
        audiosource.PlayOneShot(reloadSound);
    }

    void playSwing()
    {
        audiosource.PlayOneShot(swingSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Breakable")
        {
            Debug.Log("Breaking object");
            audiosource.PlayOneShot(breakSound);
            ai._AIState = CoreAI.AIState.Angry;
            Destroy(other.gameObject);
        }
    }

}

