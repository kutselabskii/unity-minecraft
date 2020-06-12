using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Finish") {
			Destroy(collider.gameObject);
		}

		if (collider.gameObject.tag == "Player") {
			collider.gameObject.GetComponent<PlayerController>().LoseLife();
		}
	}
}
