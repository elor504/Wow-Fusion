using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    public bool CanBeTargeted();

    public void OnTargeted();
    public void OnStopTargeting();

    public void OnHovering();
    public void OnStoppedHovering();
}
