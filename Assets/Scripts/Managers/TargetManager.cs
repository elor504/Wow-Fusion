using System;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private static ITargetableEntity _currentHoveredEntity;

    public LayerMask layerMask;

    private ITargetableEntity _currentTarget;

    public ITargetableEntity CurrentTarget => _currentTarget;
    public static bool IsHoveredOnEntity => _currentHoveredEntity != null;

    public static string FRIENDLY_TAG = "Friendly";
    public static string MY_PLAYER_TAG = "Player";
    public static string ENEMY_TAG = "Enemy";

    public event Action<ITargetableEntity> OnTarget;

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
        if (GameTest.LocalCharacter == null)
            return;

        if (GameTest.LocalCharacter.InputManager.IsMouseOverUI)
        {
            return;
        }
        
        RaycastHit hit;
        Ray ray =  Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, layerMask) &&
            hit.transform.gameObject.TryGetComponent<ITargetableEntity>(out var target))
        {
            TargetEntity(target);
        }
        else
        {
            TargetEntity(null);
        }
    }


    public void TargetEntity(ITargetableEntity entity)
    {
        _currentTarget?.OnStopTargeting();
        _currentTarget = entity;
        _currentTarget?.OnTargeted();
        OnTarget?.Invoke(_currentTarget);
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