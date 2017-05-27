using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Networking;

public class MyPlayerController : NetworkBehaviour {

	public int maxHP = 2;
	public int maxSpeed = 5;
	public float acc = 20f;
	public float fallVelocity = -30f;
	public GameObject Torrent;
	public GameObject Cannon;
	public GameObject ExplosionFX;
	public GameObject MiniExplosionFX;

	[SyncVar]
	[HideInInspector]
	public string playerName;
	[SyncVar]
	[HideInInspector]
	public int teamNumber;
	[SyncVar]
	[HideInInspector]
	public Color playerColor;
	[SyncVar]
	[HideInInspector]
	public int lifePoint;

	private Rigidbody rb;
	private MeshRenderer mr;
	private Collider cl;
	private List<Vector3> availableSP;
	private float teamTRFireInterval = 0.8f;

	Vector3 movement;
	bool isJumpable;
	float sqrMaxSpeed;
	float timer;
	float clickInterval = 0.2f;
	float explosionYield = 3f;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.color = playerColor;

		if (!isLocalPlayer)
			return;

		rb = GetComponent<Rigidbody> ();
		//mr = GetComponent<MeshRenderer> ();
		//cl = GetComponent<Collider> ();
		isJumpable = false;
		sqrMaxSpeed = maxSpeed * maxSpeed;

		availableSP = new List<Vector3> ();
		NetworkStartPosition[] spawnPointPool = FindObjectsOfType<NetworkStartPosition>();
		if (teamNumber == 1) {
			foreach (NetworkStartPosition sp in spawnPointPool) {
				if (sp.tag == "RedSpawnPoint")
					availableSP.Add (sp.transform.position);
			}
		}
		else {
			foreach(NetworkStartPosition sp in spawnPointPool){
				if(sp.tag == "BlueSpawnPoint")
					availableSP.Add (sp.transform.position);
			}		
		}

		respawnFunc ();

	}

	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
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
		GameObject torrent = Instantiate(Torrent, transform.position + new Vector3(0f,0f,1f) ,Quaternion.identity);
		torrent.GetComponent<TorrentController> ().teamNumber = teamNumber;
		NetworkServer.Spawn (torrent);
	}

	[Command]
	void CmdSetCannon(){
		GameObject cannon = Instantiate(Cannon,transform.position + new Vector3(0f,0f,1f) ,Quaternion.identity);
		NetworkServer.Spawn (cannon);
	}

	[Command]
	void CmdUpgrade(int choice){
		switch (choice) {
		case 1:
			RpcUpgradeMaxHP (teamNumber);
			break;
		case 2:
			RpcUpgradeSpeed (teamNumber);
			break;
		case 3:
			RpcUpgradeTorrentShotRate (teamNumber);
			break;
		}
	}

	[Command]
	void CmdKamikaze(){
		
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionYield);
		if (teamNumber == 1) {
			foreach (Collider hit in colliders) {
				if (hit.gameObject.tag == "Player") {
					if (hit.gameObject.GetComponent<MyPlayerController> ().teamNumber == 2) {
						// Damage them

					}
				}
				else if (hit.gameObject.layer == LayerMask.NameToLayer("BlueAsset")) {
					// Damage them

				} 
			}
		}
		else if (teamNumber == 2) {
			foreach (Collider hit in colliders) {
				if (hit.gameObject.tag == "Player") {
					if (hit.gameObject.GetComponent<MyPlayerController> ().teamNumber == 1) {
						// Damage them

					}
				}
				else if (hit.gameObject.layer == LayerMask.NameToLayer("RedAsset")) {
					// Damage them

				} 
			}
		}

		GameObject fx = Instantiate(ExplosionFX,transform.position,Quaternion.identity);
		NetworkServer.Spawn (fx);

		lifePoint = maxHP;
		RpcRespawn ();

		// Detect enemy and damage them
	}

	[ClientRpc]
	void RpcUpgradeMaxHP(int team){
		if (team != teamNumber)
			return;

		int tmp = maxHP + 1;
		if (tmp <= 10)
			maxHP = tmp;
		
	}

	[ClientRpc]
	void RpcUpgradeSpeed(int team){
		if (team != teamNumber)
			return;

		int tmp = maxSpeed + 1;
		if (tmp <= 10)
			maxSpeed = tmp;
		
	}

	[ClientRpc]
	void RpcUpgradeTorrentShotRate(int team){
		if (team != teamNumber)
			return;

		float tmp = teamTRFireInterval - 0.1f;
		if (tmp >= 0.4f)
			teamTRFireInterval = tmp;
	}

	[ClientRpc]
	void RpcRespawn()
	{
		if (!isLocalPlayer)
			return;
		
		respawnFunc ();

	}

	void respawnFunc(){
		// Set the spawn point to origin as a default value
		Vector3 spawnPoint = Vector3.zero + new Vector3(0f,3.82f,0f);

		// If there is a spawn point array and the array is not empty, pick one at random
		if (availableSP.Count > 0)
		{
			spawnPoint = availableSP [Random.Range (0, availableSP.Count)];//.transform.position;
		}

		// Set the player’s position to the chosen spawn point
		transform.position = spawnPoint;

	}

	public void takeDamage(int amount){
		if (!isServer)
			return;

		lifePoint -= amount;

		if (lifePoint <= 0) {
			GameObject fx = Instantiate(MiniExplosionFX,transform.position,Quaternion.identity);
			NetworkServer.Spawn (fx);
			lifePoint = maxHP;
			RpcRespawn ();
		}
	}

	void OnCollisionEnter(Collision obj){
		if (obj.collider.gameObject.layer == LayerMask.NameToLayer("Floor")) {
			isJumpable = true;
			//Debug.Log ("touch");
		}
	}

	void FixedUpdate(){

		if (!isLocalPlayer)
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

}

