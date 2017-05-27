using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BombController : NetworkBehaviour {

	public float speed = 5f;
	public float explosionYield = 3f;
	public GameObject effect;
	private Rigidbody rb;

	[SyncVar]
	[HideInInspector]
	public int bomb_team;

	float timer = 0f;
	float lifeTime = 5f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.AddForce(transform.forward * speed);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > lifeTime)
			Destroy (this.gameObject);
		/*
		if (direction != Vector3.zero && !isDirAssigned) {
			rb.AddForce(direction * speed);
			isDirAssigned = true;
		}*/
	}

	void OnTriggerEnter(Collider obj){
		Instantiate (effect, transform.position, Quaternion.identity);
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionYield);
		if (bomb_team == 1) {
			foreach (Collider hit in colliders) {
				if (hit.gameObject.tag == "Player") {
					if (hit.GetComponent<MyPlayerController> ().teamNumber == 2) {
						// Damage them
						hit.GetComponent<MyPlayerController>().takeDamage(10);
					}
				}
				else if (hit.gameObject.layer == LayerMask.NameToLayer("BlueAsset")) {
					// Damage them

				} 
			}
		}
		else if (bomb_team == 2) {
			foreach (Collider hit in colliders) {
				if (hit.gameObject.tag == "Player") {
					if (hit.GetComponent<MyPlayerController> ().teamNumber == 1) {
						// Damage them
						hit.GetComponent<MyPlayerController>().takeDamage(10);
					}
				}
				else if (hit.gameObject.layer == LayerMask.NameToLayer("RedAsset")) {
					// Damage them

				} 
			}
		}
		Destroy (this.gameObject);
	}
	/*
	public void setDir(Vector3 dir){
		if (dir == Vector3.zero) {
			dir = new Vector3 (0, 1, 1);
		}
		direction = dir;

		direction.y += 5f;

	}*/
}
