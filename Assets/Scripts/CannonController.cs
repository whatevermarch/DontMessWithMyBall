using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CannonController : NetworkBehaviour {

	public GameObject shot;
	public float detectRadius = 5f;
	public float LifeTimeInSec = 180f;

	[SyncVar]
	[HideInInspector]
	public int cannonTeamNumber = 0;

	float timer;
	float fireInterval = 1.4f;

	// Use this for initialization
	void Start () {
		if (cannonTeamNumber == 1) 
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
			if (cannonTeamNumber == 0)
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
		if (cannonTeamNumber == 1) {
			foreach (Collider hit in colliders) {
				if (hit.gameObject.tag == "Player") {
					if (hit.gameObject.GetComponent<MyPlayerController> ().teamNumber == 2) {
						Vector3 enemyDir = hit.transform.position - transform.position;
						enemyDir.y = 0f;
						GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation (enemyDir));
						bullet.gameObject.GetComponent<BombController> ().bomb_team = 1;
						NetworkServer.Spawn (bullet);

						//transform.rotation = Quaternion.LookRotation (enemyDir);
						transform.GetChild(1).rotation = Quaternion.LookRotation (enemyDir);

						break;
					}
				}
				else if (hit.gameObject.layer == LayerMask.NameToLayer("BlueAsset")) {
					Vector3 enemyDir = hit.transform.position - transform.position;
					enemyDir.y = 0f;
					GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation (enemyDir));
					bullet.gameObject.GetComponent<BombController> ().bomb_team = 1;
					NetworkServer.Spawn (bullet);

					//transform.rotation = Quaternion.LookRotation (enemyDir);
					transform.GetChild(1).rotation = Quaternion.LookRotation (enemyDir);

					break;
				} 
			}
		}
		else if (cannonTeamNumber == 2) {
			foreach (Collider hit in colliders) {
				if (hit.gameObject.tag == "Player") {
					if (hit.gameObject.GetComponent<MyPlayerController> ().teamNumber == 1) {
						Vector3 enemyDir = hit.transform.position - transform.position;
						enemyDir.y = 0f;
						GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation (enemyDir));
						bullet.gameObject.GetComponent<BombController> ().bomb_team = 2;
						NetworkServer.Spawn (bullet);

						//transform.rotation = Quaternion.LookRotation (enemyDir);
						transform.GetChild(1).rotation = Quaternion.LookRotation (enemyDir);

						break;
					}
				}
				else if (hit.gameObject.layer == LayerMask.NameToLayer("RedAsset")) {
					Vector3 enemyDir = hit.transform.position - transform.position;
					enemyDir.y = 0f;
					GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation (enemyDir));
					bullet.gameObject.GetComponent<BombController> ().bomb_team = 2;
					NetworkServer.Spawn (bullet);

					//transform.rotation = Quaternion.LookRotation (enemyDir);
					transform.GetChild(1).rotation = Quaternion.LookRotation (enemyDir);

					break;
				} 
			}
		}
	}

}