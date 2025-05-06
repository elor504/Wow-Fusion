using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   [SerializeField] private CharacterController controller;
   [SerializeField] private float moveSpeed = 10f;

   private Vector2 _movementInput;
   public bool IsPressingMovement => _movementInput != Vector2.zero;
   
   public void Move()
   {
      Vector3 direction = new Vector3(_movementInput.x, 0f, _movementInput.y);
      var velocity = direction * (moveSpeed * Time.deltaTime);

      controller.Move(velocity);
   }
   public void ListenToMovementInput(Vector2 movement)
   {
      _movementInput = movement;
   }
   private void OnEnable()
   {
      InputManager.OnMovementInput += ListenToMovementInput;
   }
   private void OnDisable()
   {
      InputManager.OnMovementInput -= ListenToMovementInput;
   }
   
}
