using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlienBehavior : MonoBehaviour {

	public float ForwardThrust = 10f;
	public float Torque = 2f;
	public float ReverseThrustRatio = .5f;
	public float DeadlyForce = 3f;
	public AudioClip WinSound;

	private Rigidbody2D m_body;
	private BeamBehavior m_beam;
	private int m_score;
	private Text m_scoreText;
	private GameObject m_engine;
	private bool m_dead;
	private bool m_winning;
	private GameObject[] m_humans;

	public void AddScore(int amount)
	{
		m_score += amount;
		m_scoreText.text = "Score: " + m_score;

		Debug.Log(m_score);
		Debug.Log(amount);
		if (m_score > 0 && m_score >= m_humans.Length * amount) // TODO this assumes that amount is constant
		{
			StartCoroutine(Win());
		}
	}

	public IEnumerator Win()
	{
		m_winning = true;
		Debug.Log("Winning");
		AudioSource.PlayClipAtPoint(WinSound, Vector3.zero);
		yield return new WaitForSeconds(2f);
		Application.LoadLevel("Victory");
	}

	// Use this for initialization
	void Start () {
		m_body = GetComponent<Rigidbody2D>();
		m_beam = transform.Find("Beam").GetComponent<BeamBehavior>();

		//m_engineFlame = transform.Find("EngineFlame").GetComponent<ParticleSystem>();
		m_engine = transform.FindChild("EngineFlame").gameObject;
		if (m_engine == null)
			Debug.LogError("No EngineFlame");

		if (WinSound == null)
			Debug.LogError("WinSound null");

		m_humans = GameObject.FindGameObjectsWithTag("Human");
		Debug.Log(string.Format("{0} humans, game will end at {1} score", m_humans.Length, m_humans.Length * 100));
		
		
		m_scoreText = GameObject.Find("Score").GetComponent<Text>();
		if (m_scoreText == null)
			Debug.LogError("No game object named Score found");
		AddScore(0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (m_dead)
			return;

		ReadInput();
	}

	void ReadInput()
	{
		if (IsUpKey)
		{
			m_body.AddForce(m_body.transform.up * ForwardThrust);
			m_engine.SetActive(true);
		}
		else
		{
			m_engine.SetActive(false);
		}
		
		if (IsReversKey)
			m_body.AddForce(m_body.transform.up * ForwardThrust * ReverseThrustRatio * -1);
		
		if (IsRotateLeftKey)
			m_body.AddTorque(Torque);
		if (IsRotateRightKey)
			m_body.AddTorque(-Torque);
		
		if (IsRightKey)
			m_body.AddForce(m_body.transform.right * ForwardThrust * ReverseThrustRatio);
		if (IsLeftKey)
			m_body.AddForce(m_body.transform.right * -1 * ForwardThrust * ReverseThrustRatio);

		m_beam.IsBeaming = IsBeamKey;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log(string.Format("Collided with {0} force: {1}", other.gameObject.name, other.relativeVelocity.magnitude));
		if (other.gameObject.name == "Ground")
		{
			if (other.relativeVelocity.magnitude > DeadlyForce)
			{
				Explode();
			}
		}
	}

	public void Explode()
	{
		if (m_winning)
			return;

		m_beam.IsBeaming = false;
		m_engine.SetActive(false);
		transform.FindChild("Explosion").gameObject.SetActive(true);
		m_dead = true;
		StartCoroutine(WaitAndDestroy(3f));
	}
	
	IEnumerator WaitAndDestroy(float waitTime) {
		float interval = waitTime * .1f;
		for (float i = 0; i < waitTime; i += interval)
		{
			yield return new WaitForSeconds(interval);
			// TODO fade alpha
		}
		Destroy(gameObject);

		Application.LoadLevel("GameOver");
	}
	
	bool IsUpKey
	{
		get { return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Keypad8); }
	}
	
	bool IsReversKey
	{
		get { return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.Keypad5); }
	}
	
	bool IsRotateLeftKey
	{
		get { return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Keypad4); }
	}
	
	bool IsRotateRightKey
	{
		get { return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.Keypad6); }
	}

	bool IsLeftKey
	{
		get { return Input.GetKey(KeyCode.Keypad7); }
	}

	bool IsRightKey
	{
		get { return Input.GetKey(KeyCode.Keypad9); }
	}

	bool IsBeamKey
	{
		get { return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Alpha0); }
	}
}
