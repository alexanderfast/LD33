using UnityEngine;
using System.Collections;

public class PlayerKiller : MonoBehaviour {
	
	void OnCollisionEnter2D(Collision2D other)
	{
		//Debug.Log(string.Format("Collided with {0} force: {1}", other.gameObject.name, other.relativeVelocity.magnitude));
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<AlienBehavior>().Explode();
		}
		else if (other.gameObject.name.Contains("Human"))
		{
			// TODO
		}
	}
}
