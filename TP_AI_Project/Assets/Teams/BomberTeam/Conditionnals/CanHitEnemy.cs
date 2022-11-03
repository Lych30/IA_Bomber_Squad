using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using TaskStatus = BehaviorDesigner.Runtime.Tasks.TaskStatus;

public class CanHitEnemy : Conditional
{
    public override TaskStatus OnUpdate()
    {
        BehaviorTree behaviorTree = this.gameObject.GetComponent<BehaviorTree>();
        if (CanHit((Vector2)behaviorTree.GetVariable("shipPosition").GetValue(), (float)behaviorTree.GetVariable("shipOrientation").GetValue(), (Vector2)behaviorTree.GetVariable("otherShipPosition").GetValue(), (Vector2)behaviorTree.GetVariable("otherShipVelocity").GetValue(), 0.15f))
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
    
    
    public static bool CanHit(Vector2 spaceshipPosition,float spaceshipOrientation, Vector2 targetPosition, Vector2 targetVelocity, float hitTimeTolerance)
    {                
        if(hitTimeTolerance <= 0) {
            Debug.LogError("Hit time tolerence must be greater than 0");
            return false;
        }

        float shootAngle = Mathf.Deg2Rad * spaceshipOrientation;
        Vector2 shootDirection = new Vector2(Mathf.Cos(shootAngle), Mathf.Sin(shootAngle));

        Vector2 intersection;
        bool canIntersect = ComputeIntersection(spaceshipPosition, shootDirection, targetPosition, targetVelocity, out intersection);        
        if (!canIntersect) { // Cannot shoot if directions never cross eachother (parallel)
            return false;
        }

        Vector2 spaceshipToIntersection = intersection - spaceshipPosition;        
        if (Vector2.Dot(spaceshipToIntersection, shootDirection) <= 0) // Cannot shoot if target is behind
            return false;

        Vector2 targetToIntersection = intersection - targetPosition;
        float bulletTimeToIntersection = spaceshipToIntersection.magnitude / Bullet.Speed;
        float targetTimeToIntersection = targetToIntersection.magnitude / targetVelocity.magnitude;
        targetTimeToIntersection *= Vector2.Dot(targetToIntersection, targetVelocity) > 0 ? 1 : -1;

        float timeDiff = bulletTimeToIntersection - targetTimeToIntersection;        
        return Mathf.Abs(timeDiff) < hitTimeTolerance;
    }
    
    public static bool ComputeIntersection(Vector2 A, Vector2 dirA, Vector2 B, Vector2 dirB, out Vector2 C)
    {
        C = Vector2.zero;
        if (dirA.sqrMagnitude == 0 || dirB.sqrMagnitude == 0)
            return false;
        dirA.Normalize();
        dirB.Normalize();

        float denominator = dirA.x * dirB.y - dirA.y * dirB.x;
        if (denominator == 0)
            return false; // can't divide by 0;

        float k = ((A.y - B.y) * dirB.x + (B.x - A.x) * dirB.y) / denominator;
        C = A + dirA * k;
        return true;
    }
}
