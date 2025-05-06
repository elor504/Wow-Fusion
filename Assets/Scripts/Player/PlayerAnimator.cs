using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
   [SerializeField] private Animator animator;

   private Vector2 _movementInput;
   public Vector2 MovementInput => _movementInput;


   private static readonly int movementX = Animator.StringToHash("MovementX");
   private static readonly int movementY = Animator.StringToHash("MovementY");


   public void SetBool(int id, bool value)
   {
      animator.SetBool(id, value);
   }
   public void SetFloat(int id, float value)
   {
      animator.SetFloat(id, value);
   }
   public void SetTrigger(int startCasting)
   {
      animator.SetTrigger(startCasting);
   }
   
   public void UpdateMovementOnAnimator()
   {
      SetFloat(movementX, _movementInput.x);
      SetFloat(movementY, _movementInput.y);
   }
   
   private void UpdateMovementInput(Vector2 input)
   {
      _movementInput = input;
   }
   
   private void OnEnable()
   {
      InputManager.OnMovementInput += UpdateMovementInput;
   }
   private void OnDisable()
   {
      InputManager.OnMovementInput -= UpdateMovementInput;
   }
}
