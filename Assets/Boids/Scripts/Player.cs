using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boids
{
    public class Player : MonoBehaviour
    {

        #region Initialization

        /// <summary>
        /// Executes once on start.
        /// </summary>
        private void Awake()
        {
            // Extract rigid body
            Rigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Initiaizes the player.
        /// </summary>
        public void Initialize(Team team, float w, float mV, float a, float mE)
        {
            // Give the player a small push
            Rigidbody.velocity = transform.forward.normalized * team.TeamSettings.MinSpeed;

            // Reference the team this player belongs to
            Team = team;

            // Initialize attributes
            weight = w;
            maxVelocity = mV;
            aggressiveness = a;
            maxExhaustion = mE;
            exhaustion = 0;
         }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// References the rigidbody attached to this object.
        /// </summary>
        private Rigidbody Rigidbody;

        /// <summary>
        /// The team this player belongs to.
        /// </summary>
        private Team Team;

        /// <summary>
        /// The team this player belongs to.
        /// </summary>
        private Vector3 spawnPoint;

        /// <summary>
        /// Player attributes
        /// </summary>
        private float weight;
        private float maxVelocity;
        public float aggressiveness;
        public float maxExhaustion;
        public float exhaustion;
        public float recoveryRate;
        public bool  steadFast;

        private GameObject Snitch;

        /// <summary>
        /// Possible States
        /// </summary>

        private bool falling = false;
        private bool resting = false;

        #endregion

        #region Methods

        /// <summary>
        /// Continuous update the speed and rotation of the player.
        /// </summary>
        private void Update()
        {
            // Initialize the new velocity
            Vector3 acceleration = Vector3.zero;

            Vector3 velocity = Rigidbody.velocity;

            if (exhaustion >= (maxExhaustion - 10.0f))
            {

                resting = true;
            }

            if (exhaustion <= 10.0f)
            {

                resting = false;
            }

            if (exhaustion >= maxExhaustion) {

                falling = true;
            }

            if (falling && (transform.localPosition.y <= 0.05f)) {

                transform.localPosition = spawnPoint;
                // Give the player another small push
                Rigidbody.velocity = transform.forward.normalized * Team.TeamSettings.MinSpeed;

                falling = false;
                exhaustion = 0.0f;
            }

            if (!falling)
            {

                // Compute cohesion
                acceleration += NormalizeSteeringForce(ComputeCohesionForce())
                  * Team.TeamSettings.CohesionForceWeight;

                // Compute seperation
                acceleration += NormalizeSteeringForce(ComputeSeperationForce())
                   * Team.TeamSettings.SeperationForceWeight;

                // Compute alignment
                //acceleration += NormalizeSteeringForce(ComputeAlignmentForce())
                 // * Team.TeamSettings.AlignmentForceWeight;

                // Compute collision avoidance
                //acceleration += NormalizeSteeringForce(ComputeCollisionAvoidanceForce())
                  //  * Team.TeamSettings.CollisionAvoidanceForceWeight;

                velocity += acceleration * Time.deltaTime;

                // Ensure the velocity remains within the accepted range
                velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude,
                    Team.TeamSettings.MinSpeed, maxVelocity);

            }
            else {

                acceleration.y = -1.0f;
                velocity += acceleration * Time.deltaTime;

            }

            if (resting) { 
                
                velocity = Vector3.zero;
                exhaustion -= 0.25f;
            }

            exhaustion += (velocity.x + velocity.y + velocity.z) / 20.0f;

            // Apply velocity
            Rigidbody.velocity = velocity;

            // Update rotation
            transform.forward = Rigidbody.velocity.normalized;
        }

        /// <summary>
        /// Normalizes the steering force and clamps it.
        /// </summary>
        private Vector3 NormalizeSteeringForce(Vector3 force)
        {
            return force.normalized * Mathf.Clamp(force.magnitude, 0, Team.TeamSettings.MaxSteerForce);
        }

        /// <summary>
        /// Computes the Cohesion force that will pull the player back to the center of the team.
        /// </summary>
        private Vector3 ComputeCohesionForce()
        {

            // Get current center of the team
            //Vector3 center = Team.SnitchPosition;

            //Vector3 snitch = Team.manager.GetComponent<MainSceneManager>().Snitch.GetComponent<Snitch>().Position;
            Vector3 snitch = Team.Snitch.transform.position;

            // Get rid of this player's position from the center
            //float newCenterX = center.x * Team.Players.Count - transform.localPosition.x;
            //float newCenterY = center.y * Team.Players.Count - transform.localPosition.y;
            //float newCenterZ = center.z * Team.Players.Count - transform.localPosition.z;
            //Vector3 newCenter = new Vector3(newCenterX, newCenterY, newCenterZ) / (Team.Players.Count - 1);


            // Compute force
            //return newCenter - transform.localPosition;
            return snitch - transform.position;

            /*
            // Else, use the center of the neighbor players
            float centerX = 0, centerY = 0, centerZ = 0;
            int count = 0;
            foreach (Player player in Team.Players)
            {
                if (player == this
                    || (player.transform.position - transform.position).magnitude > Team.TeamSettings.CohesionRadiusThreshold)
                    continue;

                centerX += player.transform.localPosition.x;
                centerY += player.transform.localPosition.y;
                centerZ += player.transform.localPosition.z;
                count++;
            }

            // Compute force
            return count == 0 
                ? Vector3.zero 
                : new Vector3(centerX, centerY, centerZ) / count;
            */

        }

        /// <summary>
        /// Computes the seperation force that ensures a safe distance is kept between the players.
        /// </summary>
        private Vector3 ComputeSeperationForce()
        {
            // Initialize seperation force
            Vector3 force = Vector3.zero;

            // Find nearby players
            foreach (Player player in Team.Players)
            {
                if (player == this
                    || (player.transform.position - transform.position).magnitude > Team.TeamSettings.SeperationRadiusThreshold)
                    continue;

                // Repel aaway
                force += transform.position - player.transform.position;
            }

            return force;
        }

        /// <summary>
        /// Computes the alignment force that aligns this player with nearby players.
        /// </summary>
        private Vector3 ComputeAlignmentForce()
        {
            // Initialize alignment force
            Vector3 force = Vector3.zero;

            // Find nearby players
            foreach (Player player in Team.Players)
            {
                if (player == this
                    || (player.transform.position - transform.position).magnitude > Team.TeamSettings.AlignmentRadiusThreshold)
                    continue;

                force += player.transform.forward;
            }

            return force;
        }

        /// <summary>
        /// Computes the force that helps avoid collision.
        /// </summary>
        private Vector3 ComputeCollisionAvoidanceForce()
        {
            // Check if heading to collision
            if (!Physics.SphereCast(transform.position,
                Team.TeamSettings.CollisionAvoidanceRadiusThreshold, 
                transform.forward, 
                out RaycastHit hitInfo,
                Team.TeamSettings.CollisionAvoidanceRadiusThreshold))
                return Vector3.zero;

            // Compute force
            return transform.position - hitInfo.point;
        }

        /// <summary>
        /// Handle player or snitch collisions
        /// </summary>
        private void OnTriggerEnter(Collider col)
        {
            // Check that collision is with rigidbody object
            if (col.GetComponent<Rigidbody>() == null) return;

            Rigidbody targetRigidbody = col.GetComponent<Rigidbody>();

            if (targetRigidbody.GetComponent<Player>() != null) {

                float targetExhaustion = targetRigidbody.GetComponent<Player>().exhaustion;
                float targetMaxExhaustion = targetRigidbody.GetComponent<Player>().maxExhaustion;
                float targetAggressiveness = targetRigidbody.GetComponent<Player>().aggressiveness;

                System.Random rnd = new System.Random();

                float playerValue = aggressiveness * (((float)rnd.NextDouble()) * (1.2f - 0.8f) * (1.0f - (exhaustion / maxExhaustion)));
                float targetValue = targetAggressiveness * (((float)rnd.NextDouble()) * (1.2f - 0.8f) * (1.0f - (targetExhaustion / targetMaxExhaustion)));

                if (playerValue < targetValue) {

                    if (targetRigidbody.GetComponent<Player>().Team) 
                    {
                        if (rnd.NextDouble() <= 0.05) 
                        {
                            falling = true;
                        }
                    }
                    else
                    {
                        falling = true;
                    }
                }

            } else if (targetRigidbody.GetComponent<Snitch>() != null) {


                if (targetRigidbody.GetComponent<Snitch>().getLastWinBy() == Team)
                {
                    Team.score += 2;
                }
                else {
                    Team.score += 1;
                }

                targetRigidbody.GetComponent<Snitch>().setLastWinBy(Team);
            }
        }

            #endregion

        }
}
