using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public GameObject target;
	private int distance = 4;
	private int height = 16;
	private int damping = 1000;
	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(target == null) {
			target = GameObject.FindGameObjectWithTag("Player");
			Debug.Log ("Camera found no Player");
		}

		transform.rotation = Quaternion.Euler(75, 0, 0);
		Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y + height,target.transform.position.z - distance);
		transform.position = Vector3.Lerp(transform.position,targetPosition, damping*Time.deltaTime);
	}
}
