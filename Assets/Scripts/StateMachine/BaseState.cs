using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{


    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState(float deltaTime);
    public abstract void FixedUpdateState(float fixedDeltaTime);

    public abstract bool CompareID(int id);
}
