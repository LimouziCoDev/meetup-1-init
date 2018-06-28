using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    private GameObject parent;

	void Start () {
        parent = transform.parent.gameObject;	
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject != parent)
        {
            var player = collision.gameObject.GetComponent<PlayerMove>();
            if(player)
            {
                player.hit();
            }
        }
    }
}
