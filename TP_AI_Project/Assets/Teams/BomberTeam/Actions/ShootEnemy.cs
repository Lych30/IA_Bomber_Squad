using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ShootEnemy : Action
{
    
    public override TaskStatus OnUpdate()
    {
        this.gameObject.GetComponent<BehaviorTree>().SetVariableValue("shootNext", true);
        return TaskStatus.Success;
    }
}
