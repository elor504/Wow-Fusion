using System;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public Camera camera;
    public LayerMask layerMask;
    private static TargetManager _instance;
    private ITargetableEntity _currentTarget;
    
    public static TargetManager Instance => _instance;
    public ITargetableEntity CurrentTarget => _currentTarget;
    public static bool IsHoveredOnEntity => _currentHoveredEntity != null;

    private static ITargetableEntity _currentHoveredEntity;

    private void OnEnable()
    {
        InputManager.OnClickLeftMouse += ClickOnEntity;
    }

    private void OnDisable()
    {
        InputManager.OnClickLeftMouse -= ClickOnEntity;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ClickOnEntity()
    {
        ///Check that we don't click on ui
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, layerMask) &&
            hit.transform.gameObject.TryGetComponent<ITargetableEntity>(out var target))
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