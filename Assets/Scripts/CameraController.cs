using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraController : MonoBehaviour {

	public Transform playerTransform;

	//private Rigidbody rb;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update()
	{
		transform.position = playerTransform.position + new Vector3 (0.0f, 9.0f, -5.0f);
		transform.LookAt (playerTransform.position);
	}

}
