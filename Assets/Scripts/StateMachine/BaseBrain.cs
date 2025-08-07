using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBrain
{
    public abstract void RPC_ChangeState(int state);
    public abstract void UpdateState(float time);
    public abstract void FixedUpdateState(float fixedDeltaTime);

    public abstract void OnAnimationCallFunction(int eventID);
}
