using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour {

	public float speed;
	public float shotYield;
	private Rigidbody rb;

	float timer;
	float lifeTime;
	[SyncVar]
	[HideInInspector]
	public int bullet_color;

	// Use this for initialization
	void Start () {
		//Debug.Log("Bullet" + bullet_color);
		if(bullet_color == 1)
			GetComponent<Renderer>().material.color = Color.red;
		else
			GetComponent<Renderer>().material.color = Color.blue;
		rb = GetComponent<Rigidbody> ();
		lifeTime = shotYield / speed;
		rb.velocity = transform.forward * speed;
		//Debug.Log ("hey");
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > lifeTime)
			Destroy (this.gameObject);

		//if (direction != Vector3.zero && !isDirAssigned) {
			//rb.velocity = direction.normalized * speed;
			//isDirAssigned = true;
		//}

	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.layer == LayerMask.NameToLayer("Floor")){
			Destroy (this.gameObject);
		}
		else if (col.tag == "Player") {
			if ((bullet_color == 1 && col.GetComponent<MyPlayerController> ().teamNumber == 2) ||
				(bullet_color == 2 && col.GetComponent<MyPlayerController> ().teamNumber == 1)) {

				col.GetComponent<MyPlayerController> ().takeDamage (1);
				Destroy (this.gameObject);
			}
		}
	}

	/*public void setDir(Vector3 dir){
		if (dir == Vector3.zero)
			dir = Vector3.forward;
		direction = dir;
	}*/
}

