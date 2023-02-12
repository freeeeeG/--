using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : AbstractGood
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Floor")
        {
            ReturnFactory();
        }
        else if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().photonView.RPC("TakeDamage", RpcTarget.All, 10);
            ReturnFactory();
        }

    }
}
