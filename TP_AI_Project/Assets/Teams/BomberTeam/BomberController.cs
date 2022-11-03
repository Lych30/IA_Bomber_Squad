using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;
using UnityEngine;
using DoNotModify;
using IIM;

namespace BomberTeam {

	public class BomberController : BaseSpaceShipController
	{
		public BehaviorTree BehaviorTree;
		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
			
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 0f;
			float targetOrient = spaceship.Orientation;
			
			BehaviorTree.SetVariableValue("shipPosition", spaceship.Position);
			BehaviorTree.SetVariableValue("shipOrientation", spaceship.Orientation);
			BehaviorTree.SetVariableValue("otherShipPosition", otherSpaceship.Position);
			BehaviorTree.SetVariableValue("otherShipVelocity", otherSpaceship.Velocity);
			
			bool shootNext = (bool)BehaviorTree.GetVariable("shootNext").GetValue();
			if (shootNext) BehaviorTree.SetVariableValue("shootNext", false);
			
			return new InputData(thrust, targetOrient, shootNext, false, false);
		}
	}

}
