using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Transform playerTransform;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update()
	{
		if(playerTransform != null)
		{
			transform.position = playerTransform.position + new Vector3 (0.0f, 9.0f, -5.0f);
		}
	}

	public void setTarget(Transform target)
	{
		playerTransform = target;
	}

}
