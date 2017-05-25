using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public float acc = 20f;
	public float maxSpeed = 5f;
	public float fallVelocity = -30f;
	public GameObject Torrent;
	public GameObject Cannon;
	public GameObject ExplosionFX;

	private Rigidbody rb;
	private MeshRenderer mr;
	private Collider cl;
	private NetworkStartPosition[] spawnPointPool;

	[SyncVar]
	public bool isDead = false;

	Vector3 movement;
	bool isJumpable;
	float sqrMaxSpeed;
	float timer;
	float clickInterval = 0.2f;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer)
			return;
		
		rb = GetComponent<Rigidbody> ();
		mr = GetComponent<MeshRenderer> ();
		cl = GetComponent<Collider> ();
		isJumpable = false;
		sqrMaxSpeed = maxSpeed * maxSpeed;

		spawnPointPool = FindObjectsOfType<NetworkStartPosition>();
	}

	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer || isDead)
		{
			return;
		}

		setCamera ();

		//checkGrounded ();
		timer += Time.deltaTime;

		if (Input.GetButton("Fire1") && timer > clickInterval) {
			CmdSetTorrent();
			//CmdSetCannon();
			timer = 0f;
		}
		else if (Input.GetButton("Fire2") && timer > clickInterval) {
			CmdKamikaze();
			timer = 0f;
		}
	}

	void setCamera(){
		Camera.main.transform.position = transform.position + new Vector3 (0.0f, 9.0f, -5.0f);
		Camera.main.transform.LookAt (transform.position);
	}

	[Command]
	void CmdSetTorrent(){
		GameObject torrent = Instantiate(Torrent,transform.position,Quaternion.identity);
		NetworkServer.Spawn (torrent);
	}

	[Command]
	void CmdSetCannon(){
		GameObject cannon = Instantiate(Cannon,transform.position,Quaternion.identity);
		NetworkServer.Spawn (cannon);
	}

	[Command]
	void CmdKamikaze(){
		GameObject fx = Instantiate(ExplosionFX,transform.position,Quaternion.identity);
		NetworkServer.Spawn (fx);

		RpcRespawn ();

		// Detect enemy and damage them
	}

	[ClientRpc]
	void RpcRespawn()
	{
		if (!isLocalPlayer)
			return;
		
		// Set the spawn point to origin as a default value
		Vector3 spawnPoint = Vector3.zero;

		// If there is a spawn point array and the array is not empty, pick one at random
		if (spawnPointPool != null && spawnPointPool.Length > 0)
		{
			spawnPoint = spawnPointPool[Random.Range(0, spawnPointPool.Length)].transform.position;
		}

		// Set the player’s position to the chosen spawn point
		transform.position = spawnPoint;

	}

	void OnCollisionEnter(Collision obj){
		if (obj.collider.gameObject.layer == LayerMask.NameToLayer("Floor")) {
			isJumpable = true;
			//Debug.Log ("touch");
		}
	}

	void FixedUpdate(){

		if (!isLocalPlayer || isDead)
		{
			return;
		}

		//Debug.Log ("heybitch");
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



	void Jump(){
		if (Input.GetButton ("Jump") && isJumpable) {
			isJumpable = false;
			rb.AddForce (Vector3.up * 700);
		}
	}
	/*
	float get_angle(){
		Vector3 mousePos = Input.mousePosition;
		//Debug.DrawRay (mousePos, Vector3.up);

		Vector3 objectPos = myCam.WorldToScreenPoint (transform.position);
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;
		float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		return angle;
	}

	[Command]
	void CmdShoot(){
		var bullet = (GameObject) Instantiate(shot,transform.position,Quaternion.Euler(new Vector3(0, -get_angle() + 90, 0)));
		NetworkServer.Spawn (bullet);
	}

	[Command]
	void CmdThrowBomb(){
		var bomb = (GameObject) Instantiate(grenade,transform.position + new Vector3(0,0.89f,0),Quaternion.Euler(new Vector3(0, -get_angle() + 90, 0)));
		NetworkServer.Spawn (bomb);
	}
	*/

}

