using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurretController : NetworkBehaviour {

	public GameObject shot;
	public float detectRadius = 5f;
	public float LifeTimeInSec = 180f;

	[SyncVar]
	[HideInInspector]
	public int turretTeamNumber = 0;

	float timer;
	float fireInterval = 0.8f;

	// Use this for initialization
	void Start () {
		if (turretTeamNumber == 1) 
			transform.GetChild(0).gameObject.GetComponent<Renderer> ().material.color = Color.red;
		else
			transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.blue;

		timer = 0f;
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		LifeTimeInSec -= Time.deltaTime;

		if (LifeTimeInSec < 0)
			Destroy (this.gameObject);
		else if (timer > fireInterval){
			if (turretTeamNumber == 0)
				return;
			CmdShootEnemy ();
			timer = 0f;
		}
	}

	public void setFireInterval(float fi){
		fireInterval = fi;
	}

	[Command]
	void CmdShootEnemy(){
		Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius);
		if (turretTeamNumber == 1) {
			foreach (Collider hit in colliders) {
				if (hit.gameObject.tag == "Player") {
					if (hit.gameObject.GetComponent<MyPlayerController> ().teamNumber == 2) {
						Vector3 enemyDir = hit.transform.position - transform.position;

						GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation (enemyDir));
						bullet.gameObject.GetComponent<BulletController> ().bullet_color = 1;
						NetworkServer.Spawn (bullet);

						enemyDir.y = 0f;
						//transform.rotation = Quaternion.LookRotation (enemyDir);
						transform.GetChild(1).rotation = Quaternion.LookRotation (enemyDir);

						break;
					}
				}
				else if (hit.gameObject.layer == LayerMask.NameToLayer("BlueAsset")) {
					Vector3 enemyDir = hit.transform.position - transform.position;

					GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation (enemyDir));
					bullet.gameObject.GetComponent<BulletController> ().bullet_color = 1;
					NetworkServer.Spawn (bullet);

					enemyDir.y = 0f;
					//transform.rotation = Quaternion.LookRotation (enemyDir);
					transform.GetChild(1).rotation = Quaternion.LookRotation (enemyDir);

					break;
				} 
			}
		}
		else if (turretTeamNumber == 2) {
			foreach (Collider hit in colliders) {
				if (hit.gameObject.tag == "Player") {
					if (hit.gameObject.GetComponent<MyPlayerController> ().teamNumber == 1) {
						Vector3 enemyDir = hit.transform.position - transform.position;

						GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation (enemyDir));
						bullet.gameObject.GetComponent<BulletController> ().bullet_color = 2;
						NetworkServer.Spawn (bullet);

						enemyDir.y = 0f;
						//transform.rotation = Quaternion.LookRotation (enemyDir);
						transform.GetChild(1).rotation = Quaternion.LookRotation (enemyDir);

						break;
					}
				}
				else if (hit.gameObject.layer == LayerMask.NameToLayer("RedAsset")) {
					Vector3 enemyDir = hit.transform.position - transform.position;

					GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation (enemyDir));
					bullet.gameObject.GetComponent<BulletController> ().bullet_color = 2;
					NetworkServer.Spawn (bullet);

					enemyDir.y = 0f;
					//transform.rotation = Quaternion.LookRotation (enemyDir);
					transform.GetChild(1).rotation = Quaternion.LookRotation (enemyDir);

					break;
				} 
			}
		}
	}

}