using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boids
{
    [CreateAssetMenu(fileName = "FlockSettings", menuName = "ScriptableObjects/FlockSettingsScriptableObject", order = 1)]
    public class FlockSettingScriptable : ScriptableObject
    {

        [Header("General")]

        /// <summary>
        /// States if the sphere representing the center of the flock should be visible.
        /// </summary>
        [Tooltip("States if the sphere representing the center should be visible.")]
        public bool IsCenterVisible;

        /// <summary>
        /// The number of birds to generate on awake.
        /// </summary>
        [Tooltip("The number of birds to generate on awake.")]
        public int NumberOfBirdsToGenerateOnAwake = 10;

        /// <summary>
        /// The minimum speed a bird can fly.
        /// </summary>
        [Tooltip("The minimum speed a bird can fly.")]
        public float MinSpeed = 1;

        /// <summary>
        /// The maximum speed a bird can fly.
        /// </summary>
        [Tooltip("The maximum speed a bird can fly.")]
        public float MaxSpeed = 2.5f;

        /// <summary>
        /// The maximum steering force that can be applied at any frame rate.
        /// </summary>
        [Tooltip("The maximum steering force that can be applied at any frame rate.")]
        public float MaxSteerForce = 1.5f;



        [Header("Cohesion Force")]

        /// <summary>
        /// The weight applied to the cohesion steering force.
        /// </summary>
        [Tooltip("The weight applied to the cohesion steering force.")]
        public float CohesionForceWeight = 1;

        /// <summary>
        /// Uses the center of the flock when enforcing cohesion.
        /// The other option is to use neighbor birds.
        /// </summary>
        [Tooltip("Uses the center of the flock when enforcing cohesion. The other option is to use neighbor birds.")]
        public bool UseCenterForCohesion = true;

        /// <summary>
        /// The distance used to find nearby birds that we need to stay around.
        /// </summary>
        [Tooltip("The distance used to find nearby birds that we need to stay around.")]
        public float CohesionRadiusThreshold = 4;



        [Header("Seperation Force")]

        /// <summary>
        /// The weight applied to the seperation steering force.
        /// </summary>
        [Tooltip("The weight applied to the seperation steering force.")]
        public float SeperationForceWeight = 1;

        /// <summary>
        /// The distance used to find nearby birds that we need to keep distance from.
        /// </summary>
        [Tooltip("The distance used to find nearby birds that we need to keep distance from.")]
        public float SeperationRadiusThreshold = 1;



        [Header("Alignment Force")]

        /// <summary>
        /// The weight applied to the alignment steering force.
        /// </summary>
        [Tooltip("The weight applied to the alignment steering force.")]
        public float AlignmentForceWeight = 1;

        /// <summary>
        /// The distance used to find nearby birds that we need to stay aligned with.
        /// </summary>
        [Tooltip("The distance used to find nearby birds that we need to stay aligned with.")]
        public float AlignmentRadiusThreshold = 2;



        [Header("Collision Avoidance Force")]

        /// <summary>
        /// The weight applied to the collision avoidance steering force.
        /// </summary>
        [Tooltip("The weight applied to the collision avoidance steering force.")]
        public float CollisionAvoidanceForceWeight = 5;

        /// <summary>
        /// The distance used to find nearby obstacles that we need to avoid.
        /// </summary>
        [Tooltip("The distance used to find nearby obstacles that we need to avoid.")]
        public float CollisionAvoidanceRadiusThreshold = 1;

        // Code referenced from discussion of generating normal distribution-compliant values found here: 
        // https://stats.stackexchange.com/questions/16334/how-to-sample-from-a-normal-distribution-with-known-mean-and-variance-using-a-co
        public Tuple<float, float> GeneratePlayerSettings(float mean, float standDev)
        {

            bool generate = true;

            float x = 0;
            float y = 0;

            while (generate)
            {
                System.Random rnd = new System.Random();
                
                // first generate number between -1 and 1 (precision only to 0.001)
                float u = ((float)rnd.Next(-1000,1000))/1000.0f;
                float v = ((float)rnd.Next(-1000, 1000)) / 1000.0f;
                float w = (float)Math.Pow(u, 2.0) + (float)Math.Pow(v, 2.0);
                if (w < 1)
                {
                    generate = false;
                    float z = (float)Math.Sqrt((-2 * Math.Log(w)) / w);
                    x = u * z;
                    y = v * z;
                }

            }

            // Convert generated random numbers to deviation from mean
            var numbers = Tuple.Create((x * standDev + mean), (y * standDev + mean));

            return numbers;

        }

    }


}
