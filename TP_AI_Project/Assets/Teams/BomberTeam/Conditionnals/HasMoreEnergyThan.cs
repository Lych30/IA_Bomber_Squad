using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.ObjectDrawers;
using BehaviorDesigner.Runtime.Tasks;

public class HasMoreEnergyThan : Conditional
{
    [FloatSlider(0,1)]
    public float moreEnergyThan;

    private BehaviorTree behaviorTree;

    public override void OnStart()
    {
        behaviorTree = this.gameObject.GetComponent<BehaviorTree>();
    }

    public override TaskStatus OnUpdate()
    {
        if ((float)behaviorTree.GetVariable("shipCurrentEnergy").GetValue() >
            moreEnergyThan)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
