using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //public GameObject Hatchet;
    public bool CanAttack = true;
    public float AttackCooldown = 0.01f;
    //public AudioClip hatchetAttackSound;
    Animator anim;
    AudioSource ac;
    public BoxCollider collider;
    public GameObject reloadText;
    public AudioSource audiosource;
    public AudioClip popSound;
    public AudioClip reloadSound;

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
        if (CanAttack == false)
        {
            reloadText.SetActive(true);
            if(Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
            }
        }
    }
    
    public void HatchetAttack()
    {
        collider.enabled = true;
        StartCoroutine(ResetAttackCooldown());
        CanAttack = false;
        anim.SetTrigger("attacking");
        //ac.PlayOneShot(hatchetAttackSound);
        //StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator Reload()
    {
        anim.SetTrigger("reloading");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        CanAttack = true;
        reloadText.SetActive(false);
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        collider.enabled = false;
    }

    void playPop()
    {
        audiosource.PlayOneShot(popSound);

    }
    void playReload()
    {
        audiosource.PlayOneShot(reloadSound);
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

