using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MyPlayerController : NetworkBehaviour {

	[SyncVar]
	[HideInInspector]
	public string playerName;
	[SyncVar]
	[HideInInspector]
	public int teamNumber;
	[SyncVar]
	[HideInInspector]
	public Color playerColor;
	public float acc;
	public float maxSpeed;
	public GameObject shot;
	public float fireInterval = 0.2f;
	public GameObject grenade;
	public float fallVelocity = -15f;

	private Rigidbody rb;

	Vector3 movement;
	bool isJumpable;
	float sqrMaxSpeed;
	float timer;
	float maxRayLength = 100f;
	int sceneMask;

	// Use this for initialization
	void Start () {

		if(isLocalPlayer){
			GameObject spawn;
			if(teamNumber == 1)
				spawn = GameObject.Find("Spawn Red");
			else
				spawn = GameObject.Find("Spawn Blue");
			transform.position = spawn.transform.position;
		}
		GetComponent<Renderer>().material.color = playerColor;
		rb = GetComponent<Rigidbody> ();
		isJumpable = false;
		sqrMaxSpeed = maxSpeed * maxSpeed;
		sceneMask = LayerMask.GetMask ("Scene");
	}

	public override void OnStartLocalPlayer()
	{
		Camera.main.GetComponent<CameraController>().setTarget(transform);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!isLocalPlayer)
		{
			return;
		}

		//checkGrounded ();

		timer += Time.deltaTime;
		if (Input.GetButton("Fire1") && timer > fireInterval) {
			CmdShoot ();
			timer = 0f;

		}
		else if (Input.GetButton("Fire2") && timer > fireInterval) {
			CmdThrowBomb();
			timer = 0f;
		}

		//Debug.Log (transform.TransformPoint(new Vector3 (0, 0, 0)));


	}

	void FixedUpdate(){

		if (!isLocalPlayer)
		{
			return;
		}

		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		Move (h, v);
		Jump ();
		/*
		if (isGrounded) {
			Vector3 friction = 3 * Vector3.Normalize (new Vector3 (-rb.velocity.x, 0, -rb.velocity.z));
			rb.AddForce (friction);
		}*/

		//Debug.Log (rb.velocity.y);

		if (rb.velocity.y <= fallVelocity) {
			
		}
	}

	void Move(float h, float v){

		movement = Vector3.Normalize(new Vector3 (h, 0, v)) * acc;

		Vector3 xzVelo = rb.velocity;
		xzVelo.y = 0f;

		if (xzVelo.sqrMagnitude >= sqrMaxSpeed) {
			Vector3 resistForce = Vector3.Normalize (new Vector3 (-rb.velocity.x, 0, -rb.velocity.z)) * maxSpeed;
			rb.AddForce (resistForce);
			//rb.velocity = rb.velocity.normalized * maxSpeed;
		} else
			rb.AddForce (movement);

	}

	void OnCollisionEnter(Collision obj){
		if (obj.transform.tag == "Floor") {
			isJumpable = true;
			//Debug.Log ("touch");
		}
	}

	void Jump(){
		if (Input.GetButton ("Jump") && isJumpable) {
			isJumpable = false;
			rb.AddForce (Vector3.up * 700);
		}
	}


	float get_angle(){
		Vector3 mousePos = Input.mousePosition;
		Debug.Log (mousePos);
		Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;
		float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		return angle;
	}

	[Command]
	void CmdShoot(){
		var bullet = (GameObject) Instantiate(shot,transform.position,Quaternion.Euler(new Vector3(0, -get_angle() + 90, 0)));
		bullet.GetComponent<BulletController>().bullet_color = teamNumber;
		NetworkServer.Spawn (bullet);
	}

	[Command]
	void CmdThrowBomb(){
		var bomb = (GameObject) Instantiate(grenade,transform.position + new Vector3(0,0.89f,0),Quaternion.Euler(new Vector3(0, -get_angle() + 90, 0)));
		NetworkServer.Spawn (bomb);
	}
}
