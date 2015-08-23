using UnityEngine;
using System.Collections;

public class ShipExplosionBehavior : MonoBehaviour {

	void Start() {
		print("Starting " + Time.time);
		StartCoroutine(WaitAndPrint(3.5f));
		print("Before WaitAndPrint Finishes " + Time.time);
	}

	IEnumerator WaitAndPrint(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		print("WaitAndPrint " + Time.time);
		Destroy(gameObject);
	}
}
