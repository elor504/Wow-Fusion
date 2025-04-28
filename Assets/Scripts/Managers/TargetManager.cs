using System;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
   public Camera camera;
   public LayerMask layerMask;
   private ITargetableEntity _currentTarget;
   
   public ITargetableEntity CurrentTarget => _currentTarget;

   private static ITargetableEntity _currentHoveredEntity;
   public static bool IsHoveredOnEntity => _currentHoveredEntity != null;

   private void OnEnable()
   {
      InputManager.OnClickLeftMouse += ClickOnEntity;
   }
   private void OnDisable()
   {
      InputManager.OnClickLeftMouse -= ClickOnEntity;
   }

   public void ClickOnEntity()
   {
      ///Check that we don't click on ui
      RaycastHit hit;
      Ray ray = camera.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out hit, 100, layerMask) && hit.transform.gameObject.TryGetComponent<ITargetableEntity>(out var target))
      {
         TargetEntity(target);
      }
   }
   

   public void TargetEntity(ITargetableEntity entity)
   {
      _currentTarget?.OnStopTargeting();
      _currentTarget = entity;
      _currentTarget?.OnTargeted();
   }
   public static void SetCurrentHoveredEntity(ITargetableEntity entity)
   {
      if (_currentHoveredEntity != null)
      {
         _currentHoveredEntity.OnStoppedHovering();
      }
      
      _currentHoveredEntity = entity;
      _currentHoveredEntity?.OnHovering();
   }
}
