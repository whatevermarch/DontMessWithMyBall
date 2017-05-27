using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

	public float speed;
	public float shotYield;
	public GameObject effect;
	private Rigidbody rb;

	float timer = 0f;
	float lifeTime = 5f;
	Vector3 direction = Vector3.zero;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		direction = transform.forward + new Vector3 (0f, 1f, 0f);
		rb.AddForce(direction * speed);
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
		Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
		foreach (Collider hit in colliders)
		{
			if (hit.tag == "Enemy") {
				Rigidbody rb = hit.GetComponent<Rigidbody> ();
				if (rb != null)
					rb.AddExplosionForce (650f, transform.position, 20f, 2f);
				//EnemyController ec = hit.GetComponent<EnemyController> () as EnemyController;
				//ec.startDestroy ();
			} else if (hit.tag == "Trap") {
				Rigidbody rb = hit.GetComponent<Rigidbody> ();
				if (rb != null)
					rb.AddExplosionForce (650f, transform.position, 20f, 2f);
				//TrapController ec = hit.GetComponent<TrapController> () as TrapController;
				//ec.startDestroy ();
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
