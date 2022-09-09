using System;
using UnityEngine;

public class BasicRigidBodyPush : MonoBehaviour
{
	public LayerMask pushLayers;
	public bool canPush;
	[Range(0.0f, 5f)] public float strength = 1.1f;
	public CharacterController cat;

	private void OnTriggerEnter(Collider hit)
	{
		if (canPush) PushRigidBodies(hit);
	}

	private void PushRigidBodies(Collider hit)
	{
		print("Collision");
		// make sure we hit a non kinematic rigidbody
		Rigidbody body = hit.attachedRigidbody;
		if (body == null || body.isKinematic) return;

		// make sure we only push desired layer(s)
		var bodyLayerMask = 1 << body.gameObject.layer;
		if ((bodyLayerMask & pushLayers.value) == 0) return;

		// Calculate push direction from move direction, horizontal motion only
		Vector3 pushDir = new Vector3(cat.transform.position.x, 0.0f, cat.transform.position.z);

		// Apply the push and take strength into account
		body.AddForce(pushDir * strength, ForceMode.Impulse);
	}
}