using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Networking;

public class MineralController : NetworkBehaviour
{

    public float viewRadius;
    public int amount;
	public int getMineral;
    private int remain;

    public LayerMask targetMask;
    private TextMesh displayNumber;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    private int playerTeam;
    private TeamMoneyDisplay moneyValue;
    private Text moneyText;
	private int totalTarget;
    void Start()
    {
        displayNumber = transform.GetChild(0).GetComponent<TextMesh>();
        remain = amount;
        displayNumber.text = remain + " / " + amount;
        moneyValue = GameObject.Find("MineralDisplay").GetComponent<TeamMoneyDisplay>();
        moneyText = GameObject.Find("MineralDisplay").transform.GetChild(0).GetComponent<Text>();
        StartCoroutine("RenderTargetsWithDelay", .2f);
    }

    IEnumerator RenderTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            RenderTargets();
        }
    }

    void RenderTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        int team1 = 0;
        int team2 = 0;
		totalTarget = 0;
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {

            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            float dstToTarget = Vector3.Distance(transform.position, target.position);

            visibleTargets.Add(target);
            int temp = target.GetComponent<MyPlayerController>().teamNumber;
            if(temp == 1)
                team1 += 1;
            else
                team2 += 1;
        }

		totalTarget = team1 + team2;
		RpcMineralDown();
		if (remain == 0) {
			Destroy (this.gameObject);
		} else {
			moneyValue.teamRedMoney += team1 * getMineral;  
			moneyValue.teamBlueMoney += team2 * getMineral;
		}
    }

    [ClientRpc]
    void RpcMineralDown(){
		remain -= totalTarget * getMineral;
        displayNumber.text = remain + " / " + amount;
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
