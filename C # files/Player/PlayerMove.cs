using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	private float speed;
	private Vector3 moveDirection = Vector3.zero;
	public Player pscript;
	public CharacterController controller;
	public int smoothing = 10;
	private Vector3 forward = Vector3.zero;
	private int currDirFacing = 0;

	void Start() {
		pscript = GameObject.FindWithTag("Player").GetComponent<Player>();
		controller = GetComponent<CharacterController>();
	}

	void Update() {
		if(!Battle.inBattle && !Menu.GAME_PAUSED) {
			if(controller.isGrounded) {
				speed = (float)pscript.player.job.Speed/4;
				forward = Vector3.forward*Input.GetAxis("Vertical") + Vector3.right*Input.GetAxis("Horizontal");

				Quaternion target = Quaternion.Euler(0, MeshRotate(), 0);
				transform.rotation = Quaternion.Slerp(transform.rotation,target,Time.deltaTime * smoothing);

				Animate();
			}
			controller.SimpleMove(forward * speed * Run ());
		}
	}

	void Animate() {
		if(Input.GetAxis("Vertical") > 0.05) {
		}
		else if (Input.GetAxis("Horizontal") > 0.01) {
		}
	}

	public int MeshRotate () {
		if(Input.GetAxis("Vertical") > 0.1 && Input.GetAxis ("Horizontal") == 0) {
			currDirFacing = 0;
			return 0;
		}
		if(Input.GetAxis("Vertical") > 0.1 && Input.GetAxis("Horizontal") > 0.1) {
			currDirFacing = 45;
			return 45;
		}
		if(Input.GetAxis("Horizontal") > 0.1 && Input.GetAxis ("Vertical") == 0) {
			currDirFacing = 90;
			return 90;
		}
		else if(Input.GetAxis("Vertical") < -0.1 && Input.GetAxis("Horizontal") > 0.1) {
			currDirFacing = 135;
			return 135;
		}
		else if(Input.GetAxis("Vertical") < -0.1 && Input.GetAxis("Horizontal") == 0) {
			currDirFacing = 180;
			return 180;
		}
		else if(Input.GetAxis("Vertical") < -0.1 && Input.GetAxis("Horizontal") < -0.1) {
			currDirFacing = 225;
			return 225;
		}
		else if(Input.GetAxis("Horizontal") < -0.1 && Input.GetAxis("Vertical") == 0) {
			currDirFacing = 270;
			return 270;
		}
		else if(Input.GetAxis("Vertical") > 0.1 && Input.GetAxis("Horizontal") < -0.1) {
			currDirFacing = 315;
			return 315;
		}
		else {
			return currDirFacing;
		}
	}

	float Run() {
		if (Input.GetButton("Run")) {
			return 1.5f;
		}
		else {
			return 1.0f;
		}
	}
}