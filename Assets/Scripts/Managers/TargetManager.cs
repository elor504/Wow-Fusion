using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
   private ITargetableEntity _currentTarget;
   
   public ITargetableEntity CurrentTarget => _currentTarget;

   private static ITargetableEntity _currentHoveredEntity;
   public static bool IsHoveredOnEntity => _currentHoveredEntity != null;
   
   

   public void TargetEntity(ITargetableEntity entity)
   {
      _currentTarget = entity;
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
