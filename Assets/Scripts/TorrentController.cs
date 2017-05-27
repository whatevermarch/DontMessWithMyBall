using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TorrentController : NetworkBehaviour {

	public GameObject shot;
	public float detectRadius = 5f;
	public float LifeTimeInSec = 180f;

	[SyncVar]
	[HideInInspector]
	public int teamNumber = 0;

	float timer;
	float fireInterval = 0.8f;

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
			if (teamNumber == 0)
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
		if (teamNumber == 1) {
			foreach (Collider hit in colliders) {
				if (hit.gameObject.tag == "Player") {
					if (hit.gameObject.GetComponent<MyPlayerController> ().teamNumber == 2) {
						Vector3 enemyDir = hit.transform.position - transform.position;

						GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation (enemyDir));
						NetworkServer.Spawn (bullet);

						enemyDir.y = 0f;
						transform.rotation = Quaternion.LookRotation (enemyDir);

						break;
					}
				}
				else if (hit.gameObject.layer == LayerMask.NameToLayer("BlueAsset")) {
					Vector3 enemyDir = hit.transform.position - transform.position;

					GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation (enemyDir));
					NetworkServer.Spawn (bullet);

					enemyDir.y = 0f;
					transform.rotation = Quaternion.LookRotation (enemyDir);

					break;
				} 
			}
		}
		else if (teamNumber == 2) {
			foreach (Collider hit in colliders) {
				if (hit.gameObject.tag == "Player") {
					if (hit.gameObject.GetComponent<MyPlayerController> ().teamNumber == 1) {
						Vector3 enemyDir = hit.transform.position - transform.position;

						GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation (enemyDir));
						NetworkServer.Spawn (bullet);

						enemyDir.y = 0f;
						transform.rotation = Quaternion.LookRotation (enemyDir);

						break;
					}
				}
				else if (hit.gameObject.layer == LayerMask.NameToLayer("RedAsset")) {
					Vector3 enemyDir = hit.transform.position - transform.position;

					GameObject bullet = Instantiate (shot, transform.position, Quaternion.LookRotation (enemyDir));
					NetworkServer.Spawn (bullet);

					enemyDir.y = 0f;
					transform.rotation = Quaternion.LookRotation (enemyDir);

					break;
				} 
			}
		}
	}
		
}
