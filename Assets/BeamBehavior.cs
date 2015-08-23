using UnityEngine;
using System.Collections;

public class BeamBehavior : MonoBehaviour {

	private bool m_isBeaming;
	private MeshRenderer m_meshRenderer;
	private AudioSource m_audioSource;

	// Use this for initialization
	void Start () {
		m_meshRenderer = GetComponent<MeshRenderer>();
		m_meshRenderer.enabled = m_isBeaming;
		m_audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool IsBeaming
	{
		get { return m_isBeaming; }
		set
		{
			// we are still playing the sound/beaming, no way to stop
			if (m_audioSource.isPlaying)
				return;

			if (m_isBeaming == value)
				return;
			m_isBeaming = value;

			m_meshRenderer.enabled = value;

			if (value)
				m_audioSource.Play();
			else
				m_audioSource.Stop();
		}
	}

	void OnTriggerEnter2D(Collider2D other){
	} 

	void OnTriggerExit2D(Collider2D other) {

	}
}
