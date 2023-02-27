using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreak : MonoBehaviour
{
    CoreAI ai;
    // Start is called before the first frame update
    void Start()
    {
        ai = GameObject.Find("Enemy").GetComponent<CoreAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnColliderEnter(Collider other)
    {
        if (other.tag == "Hatchet")
        {
            ai._AIState = CoreAI.AIState.Angry;
            Destroy(this.gameObject);
        }
    }

}
