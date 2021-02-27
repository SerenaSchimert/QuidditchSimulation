using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boids
{
    [CreateAssetMenu(fileName = "TeamSettings", menuName = "ScriptableObjects/TeamSettingsScriptableObject", order = 1)]
    public class TeamSettingScriptable : ScriptableObject
    {

        [Header("General")]

        /// <summary>
        /// Below give the parameters of player attribute distributions within this team
        /// </summary>
        /// 

        [Tooltip("Gryffindor or Slytherin")]
        public string team;

        /*
        [Tooltip("Weight mean")]
        public float wMean;

        [Tooltip("Weight stdev")]
        public float wStdev;

        [Tooltip("Aggresion mean")]
        public float aMean;

        [Tooltip("Aggresion stdev")]
        public float aStdev;

        [Tooltip("Max velocity mean")]
        public float mVMean;

        [Tooltip("Max velocity stdev")]
        public float mVStdev;

        [Tooltip("Max exhaustion mean")]
        public float mEMean;

        [Tooltip("Max exhaustion stdev")]
        public float mEStdev;
        */

        //public void setManager(MainSceneManager man) { manager = man; }

        /// <summary>
        /// States if the sphere representing the center of the Team should be visible.
        /// </summary>
        //[Tooltip("States if the sphere representing the center should be visible.")]
        public bool IsCenterVisible = false;

        /// <summary>
        /// The number of players to generate on awake.
        /// </summary>
        //[Tooltip("The number of players to generate on awake.")]
        public int NumberOfPlayersToGenerateOnAwake = 10;


        /// <summary>
        /// The minimum speed a bird can fly.
        /// </summary>
        //[Tooltip("The minimum speed a bird can fly.")]
        public float MinSpeed = 1;

        /// <summary>
        /// The maximum speed a bird can fly.
        /// </summary>
        //[Tooltip("The maximum speed a bird can fly.")]
        //public float MaxSpeed = 2.5f;

        /// <summary>
        /// The maximum steering force that can be applied at any frame rate.
        /// </summary>
        [Tooltip("The maximum steering force that can be applied at any frame rate.")]
        public float MaxSteerForce = 3.0f;


        [Header("Cohesion Force")]

        /// <summary>
        /// The weight applied to the cohesion steering force.
        /// </summary>
        [Tooltip("The weight applied to the cohesion steering force.")]
        public float CohesionForceWeight = 5;

        /// <summary>
        /// Uses the center of the Team when enforcing cohesion.
        /// The other option is to use neighbor players.
        /// </summary>
        [Tooltip("Uses the center of the Team when enforcing cohesion. The other option is to use neighbor players.")]
        public bool UseCenterForCohesion = true;

        /// <summary>
        /// The distance used to find nearby players that we need to stay around.
        /// </summary>
        [Tooltip("The distance used to find nearby players that we need to stay around.")]
        public float CohesionRadiusThreshold = 2;



        [Header("Seperation Force")]

        /// <summary>
        /// The weight applied to the seperation steering force.
        /// </summary>
        [Tooltip("The weight applied to the seperation steering force.")]
        public float SeperationForceWeight = 1;

        /// <summary>
        /// The distance used to find nearby players that we need to keep distance from.
        /// </summary>
        [Tooltip("The distance used to find nearby players that we need to keep distance from.")]
        public float SeperationRadiusThreshold = 1;



        [Header("Alignment Force")]

        /// <summary>
        /// The weight applied to the alignment steering force.
        /// </summary>
        [Tooltip("The weight applied to the alignment steering force.")]
        public float AlignmentForceWeight = 0;

        /// <summary>
        /// The distance used to find nearby players that we need to stay aligned with.
        /// </summary>
        [Tooltip("The distance used to find nearby players that we need to stay aligned with.")]
        public float AlignmentRadiusThreshold = 2;



        [Header("Collision Avoidance Force")]

        /// <summary>
        /// The weight applied to the collision avoidance steering force.
        /// </summary>
        [Tooltip("The weight applied to the collision avoidance steering force.")]
        public float CollisionAvoidanceForceWeight = 2;

        /// <summary>
        /// The distance used to find nearby obstacles that we need to avoid.
        /// </summary>
        [Tooltip("The distance used to find nearby obstacles that we need to avoid.")]
        public float CollisionAvoidanceRadiusThreshold = 4;


    }


}
