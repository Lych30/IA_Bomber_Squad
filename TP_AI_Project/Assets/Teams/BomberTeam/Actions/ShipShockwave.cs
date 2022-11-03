using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ShipShockwave : Action
{
    private BehaviorTree _behaviorTree;

    public override void OnStart()
    {
        _behaviorTree = this.gameObject.GetComponent<BehaviorTree>();
    }

    public override TaskStatus OnUpdate()
    {
        _behaviorTree.SetVariableValue("useShockwaveNext", true);
        return TaskStatus.Success;
    }
}
