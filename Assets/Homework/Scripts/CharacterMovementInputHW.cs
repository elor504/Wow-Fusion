using UnityEngine;
using Fusion;
using System.Collections.Specialized;
public class CharacterMovementInputHW : NetworkBehaviour
{
	[SerializeField] private CharacterController characterController;
	[SerializeField] private float movementSpeed;

	private bool _initialized;
	public override void Spawned()
	{
		base.Spawned();
		_initialized = true;
	}

	public override void FixedUpdateNetwork()
	{
		base.FixedUpdateNetwork();
		if (!_initialized || !Object.HasStateAuthority) return;

		var xInput = Input.GetAxis("Horizontal");
		var yInput = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(xInput * movementSpeed * Time.fixedDeltaTime, 0, yInput * movementSpeed * Time.fixedDeltaTime);
		characterController.Move(movement);
	}

}
