using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class HumanBehavior : MonoBehaviour {

	public float BeamSpeed = 100f;
	public float DeadlyCollisionSpeed = 3f;
	public GameObject BloodPrefab;
	public GameObject ScorePrefab;

	private Rigidbody2D m_body;
	private GameObject m_player;
	private BeamBehavior m_beam;
	private bool m_isBeaming;
	private bool m_onGround;

	public void Crush()
	{
		Instantiate(BloodPrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	public void Abduct()
	{
		m_player.GetComponent<AlienBehavior>().AddScore(100);
		Instantiate(ScorePrefab, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		m_body = GetComponent<Rigidbody2D>();

		m_player = GameObject.FindGameObjectWithTag("Player");
		if (m_player == null)
			Debug.LogError("Player not found");
	}
	
	// Update is called once per frame
	void Update () {
		if (m_beam != null && m_beam.IsBeaming)
		{
			var direction = m_player.transform.position - transform.position;
			direction = direction.normalized * BeamSpeed;
			
			//Debug.Log(direction);
			m_body.AddForce(direction);
		}
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.name == "Beam") {
			m_beam = other.GetComponent<BeamBehavior>();
			//Debug.Log(this + " is beaming");
		}
	} 
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.name == "Beam") {
			m_beam = null;
			//Debug.Log(this + " is not beaming");
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		//Debug.Log(string.Format("Collided with {0} force: {1}", other.gameObject.name, other.relativeVelocity.magnitude));
		if (other.gameObject.tag == "Player")
		{
			if (m_onGround)
				Crush();
			else
				Abduct();
		}
		else if (other.relativeVelocity.magnitude > DeadlyCollisionSpeed)
		{
			Crush();
		}
		else if (other.gameObject.name == "Ground")
		{
			m_onGround = true;
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.name == "Ground")
		{
			m_onGround = false;
		}
	}
}
