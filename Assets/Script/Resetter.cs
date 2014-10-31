using UnityEngine;
using System.Collections;

public class Resetter : MonoBehaviour {

	public Rigidbody2D projectile;
	public float resetSpeed = 0.1f;
	private float resetSpeedSqr;
	
	private SpringJoint2D spring;
	
	void Start () {
		resetSpeedSqr = resetSpeed * resetSpeed;
		spring = projectile.GetComponent<SpringJoint2D>();
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			Reset();		
		}
		
		if(spring == null && projectile.velocity.sqrMagnitude < resetSpeedSqr)
		{
			Debug.Log(resetSpeedSqr);
			Reset();
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.rigidbody2D == projectile) {
			Reset ();		
		}
	}
	void Reset()
	{
		Application.LoadLevel (Application.loadedLevel);
		
	}
}
