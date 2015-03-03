using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public CharacterController controller;
	public Transform target;
	private Vector3 moveDirection;
	private float Distance;
	public float gravity;
	public EnemyStats esscript;
	public Enemies escript;

	public float LookDistance;
	public float ChaseDistance;

	private bool chasing;
	private bool chaseUpdated;
	
	void Start () {
		controller = GetComponent<CharacterController>();
		esscript = GetComponent<EnemyStats>();
		escript = GetComponent<Enemies>();
	}

	void Update () {
		if(target == null)
			target = GameObject.FindWithTag("Player").transform;

		if(!Battle.inBattle && !escript.thisEnemy.battled && !Menu.GAME_PAUSED) {
			Distance = Vector3.SqrMagnitude(target.position - transform.position);

			if (Distance < ChaseDistance * ChaseDistance) {
				controller.SimpleMove(transform.forward * esscript.Speed / 10);
				if(chasing == false) {
					chasing = true;
					chaseUpdated = false;
					ChaseUpdate(chaseUpdated);
				}
			}
			if(Distance < LookDistance * LookDistance) {
				transform.LookAt(new Vector3(target.position.x,transform.position.y,target.position.z));
			}

			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move(moveDirection * Time.deltaTime);
		}
	}

	void ChaseUpdate(bool updated) {
		if(!chasing) {
			updated = true;
		}
		if (chasing && !updated) {
			ChaseDistance = ChaseDistance * 2;
			updated = true;
		}
	}
}
