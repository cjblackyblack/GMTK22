using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Smart Objects and Projectiles with Derive From This, Or any time a Physical object will need a state machine, but this object does naturally receive velocity, so it could act like a rock
public class PhysicalObject : TangibleObject
{
	public Rigidbody rbody => GetComponent<Rigidbody>();
	public LayerMask hittable;
	public float activeTime;
	public int currentTime;
	public Vector3 velocity;
	public float gravity;
	public float terminalYVel;
	public float friction;

	public virtual void ObjectUpdate()
	{

	}
}