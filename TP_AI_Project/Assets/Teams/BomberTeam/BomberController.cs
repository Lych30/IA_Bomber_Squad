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
        public AnimationCurve AngleScoreCurve;
        public AnimationCurve DistanceScore;
        public List<WayPointView> Points;
        public List<AsteroidView> Asteroids;
        private Vector2 closest;
        public float AngleView;
        public override void Initialize(SpaceShipView spaceship, GameData data)
        {
            Points = data.WayPoints;
            Asteroids = data.Asteroids;
            closest = new Vector3(0,0,0);
            
            foreach (WayPointView point in Points)
            {
                if (Vector2.Distance(point.Position, spaceship.Position) < Vector2.Distance(closest, spaceship.Position))
                {
                    
                    closest = point.Position;
                    

                }
                
            }
            Debug.Log(closest);



        }

        public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			
			
			BehaviorTree.SetVariableValue("shipPosition", spaceship.Position);
			BehaviorTree.SetVariableValue("shipOrientation", spaceship.Orientation);
			BehaviorTree.SetVariableValue("otherShipPosition", otherSpaceship.Position);
			BehaviorTree.SetVariableValue("otherShipVelocity", otherSpaceship.Velocity);
			
			BehaviorTree.SetVariableValue("shipCurrentEnergy", spaceship.Energy);
			
			bool shootNext = (bool)BehaviorTree.GetVariable("shootNext").GetValue();
			if (shootNext) BehaviorTree.SetVariableValue("shootNext", false);
			
			bool useShockwaveNext = (bool)BehaviorTree.GetVariable("useShockwaveNext").GetValue();

            if (Vector2.Distance(spaceship.Position, otherSpaceship.Position) < 0.2f)
            {
                BehaviorTree.SetVariableValue("useShockwaveNext", true);
            }
            if (useShockwaveNext) BehaviorTree.SetVariableValue("useShockwaveNext", false);
			
			bool useMineNext = (bool)BehaviorTree.GetVariable("useMineNext").GetValue();

            if ((closest - spaceship.Position).magnitude < 0.2f && spaceship.Energy > 0.6f)
            {
                BehaviorTree.SetVariableValue("useMineNext", true);
            }


            if (useMineNext) BehaviorTree.SetVariableValue("useMineNext", false);

            


            //Movement
            float MaxScore = 0;
            foreach (WayPointView point in Points)
            {
                Vector3 Tempdir = (Vector2)point.Position - spaceship.Position;
                Tempdir = transform.InverseTransformDirection(Tempdir);
                float Tempangle = Mathf.Abs(Mathf.Atan2(Tempdir.y, Tempdir.x) * Mathf.Rad2Deg);

                float score = 1 / (point.Position - spaceship.Position).magnitude;


                

                if ((point.Position - spaceship.Position).magnitude <= (closest - spaceship.Position).magnitude)
                {
                    score *= DistanceScore.Evaluate(((Vector3)point.Position - transform.position).magnitude / 11)*5;
                    //Debug.Log(((Vector3)point.Position - transform.position).magnitude);
                }

                if (Tempangle <= AngleView / 2)
                {
                    score *= AngleScoreCurve.Evaluate(Tempangle / (AngleView / 2));

                }
                if (point.Owner == spaceship.Owner)
                {
                    score = 0;
                }

                if (score > MaxScore)
                {
                    MaxScore = score;
                    closest = point.Position;
                }


            }

            
            
            float thrust = 1f;
            float angle = AimingHelpers.ComputeSteeringOrient(spaceship, closest, 1.2f);
            float targetOrient = transform.rotation.z + angle;

            return new InputData(thrust, targetOrient, shootNext, useMineNext, useShockwaveNext);
		}
	}

}
