using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TorrentController : NetworkBehaviour {

	public GameObject shot;
	public float fireInterval = 0.5f;
	public float detectRadius = 5f;
	public float LifeTimeInSec = 180f;

	float timer;

	// Use this for initialization
	void Start () {
		timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		LifeTimeInSec -= Time.deltaTime;

		if (LifeTimeInSec < 0)
			Destroy (this.gameObject);
		else if (timer > fireInterval){
			CmdShootEnemy ();
			timer = 0f;
		}
	}

	[Command]
	void CmdShootEnemy(){
		Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius);
		foreach (Collider hit in colliders)
		{
			if (hit.tag == "Player") {
				Vector3 enemyDir = hit.transform.position - transform.position;

				GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation(enemyDir));
				NetworkServer.Spawn (bullet);
				break;
				//EnemyController ec = hit.GetComponent<EnemyController> () as EnemyController;
				//ec.startDestroy ();
			} 
		}
	}
		
}
