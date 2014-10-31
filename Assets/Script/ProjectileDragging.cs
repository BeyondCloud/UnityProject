using UnityEngine;
using System.Collections;

public class ProjectileDragging : MonoBehaviour {

	public float maxStretch = 3f;
	public LineRenderer catapultLineFront;
	public LineRenderer catapultLineBack;
	private SpringJoint2D spring;
	private Transform catapult;
	private Ray rayToMouse;
	private Ray leftCatapultToProjectile;

	private float maxStretchSqr;
	private float circleRadius;
	private bool clickedOn;
	private Vector2 prevVelocity;

	void Awake()
	{
		spring = GetComponent <SpringJoint2D> ();
		catapult = spring.connectedBody.transform;
	}


	void Start () 
	{
		LineRendererSetUP ();
		rayToMouse = new Ray (catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray (catapultLineFront.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		CircleCollider2D circle = collider2D as CircleCollider2D;
		circleRadius = circle.radius;   //circle of Asteriod
	}

	void Update () 
	{
	     if (clickedOn)
		      Dragging ();
		 if (spring != null) {
						if (!rigidbody2D.isKinematic && prevVelocity.sqrMagnitude > rigidbody2D.velocity.sqrMagnitude) {
								Destroy (spring);

								rigidbody2D.velocity = prevVelocity;   //avoid unexcepted velocity change
						} 
						if (!clickedOn)
								prevVelocity = rigidbody2D.velocity;

						LineRenderUpdate ();
				} else {
						catapultLineBack.enabled = false;
						catapultLineFront.enabled = false;
				}     
	}
	void LineRendererSetUP ()
	{
		catapultLineFront.SetPosition (0, catapultLineFront.transform.position);
		catapultLineBack.SetPosition(0, catapultLineBack.transform.position);

		catapultLineFront.sortingLayerName = "middle";      
		catapultLineBack.sortingLayerName = "middle";


		catapultLineFront.sortingOrder = 2;
		catapultLineBack.sortingOrder = 0;


	}

	void OnMouseDown()
	{
		spring.enabled = false;
		clickedOn = true;
	}
	void OnMouseUp()
	{

		spring.enabled = true;
		rigidbody2D.isKinematic = false;
		clickedOn = false;
	}
	void Dragging()
	{
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint- catapult.position;

		if (catapultToMouse.sqrMagnitude > maxStretchSqr) 
		{
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
		}

		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}

	void LineRenderUpdate()
	{
			Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
	    	leftCatapultToProjectile.direction =catapultToProjectile; 
			Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
		catapultLineBack.SetPosition(1,holdPoint);
		catapultLineFront.SetPosition(1,holdPoint);
	}
}
