using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBrain
{
    public abstract void ChangeState(int state);
    public abstract void UpdateState();
    public abstract void FixedUpdateState();

    public abstract void OnAnimationCallFunction(int eventID);
}
