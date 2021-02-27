using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boids
{
    public class Snitch : MonoBehaviour
    {

        #region Initialization

        /// <summary>
        /// Executes once on start.
        /// </summary>
        private void Awake()
        {
            // Extract rigid body
            // Rigidbody = GetComponent<Rigidbody>();
            // Extract rigid body
            Rigidbody = GetComponent<Rigidbody>();
            //Initialize();

        }

        /// <summary>
        /// Initiaizes snitch.
        /// </summary>
        
        public void Initialize()
        {

            Rigidbody.velocity = transform.forward.normalized * minVelocity;
            // Instantiate snitch
            //snitch = GameObject.Instantiate(PlayerTemplate, PlayersParent.transform);

            // Extract its script
            //Player playerScript = player.GetComponent<Player>();
            //_Players.Add(playerScript);

            /*
            // Set random location
            transform.localPosition = new Vector3
            (
                UnityEngine.Random.Range(-2f, 2f),
                UnityEngine.Random.Range(-2f, 2f),
                UnityEngine.Random.Range(-2f, 2f)
            );

            // Set random rotation
            transform.localEulerAngles = new Vector3
            (
                UnityEngine.Random.Range(0f, 360f),
                UnityEngine.Random.Range(0f, 360f),
                UnityEngine.Random.Range(0f, 360f)
            );     

            // Give the player a small push
            Rigidbody.velocity = transform.forward.normalized * minVelocity;
            */
        }

        #endregion

        #region Fields/Properties

        //[Header("Snitch prefab")]

        /// <summary>
        /// The prefab template used to generate players in this Team.
        /// </summary>
       // [SerializeField]
        //[Tooltip("The prefab template used to generate players in this Team.")]
        //public GameObject PlayerTemplate;

        /// <summary>
        /// The parent holding all the generated players.
        /// </summary>
        //[SerializeField]
        //[Tooltip("The parent holding all the generated players.")]
        //public GameObject PlayersParent;


        /// <summary>
        /// References the rigidbody attached to this object.
        /// </summary>
        private Rigidbody Rigidbody;

        //Last won team
        private Team lastWinBy;

        public int GryffindorScore =  0;
        public int SlytherinScore = 0;
        public string winner = null;

        /// <summary>
        /// Snitch attributes
        /// </summary>
        private float weight = 10.0f;
        private float maxVelocity = 50.0f;
        private float minVelocity = 1.0f;

        public float MaxSteerForce = 1.5f;


        /*
        /// <summary>
        /// The weight applied to the cohesion steering force.
        /// </summary>
        private float CohesionForceWeight = 1;

        /// <summary>
        /// Uses the center of the Team when enforcing cohesion.
        /// The other option is to use neighbor players.
        /// </summary>
        private bool UseCenterForCohesion = false;

        /// <summary>
        /// The distance used to find nearby players that we need to stay around.
        /// </summary>
        private float CohesionRadiusThreshold = 4;

        /// <summary>
        /// The weight applied to the seperation steering force.
        /// </summary>
        private float SeperationForceWeight = 1;

        /// <summary>
        /// The distance used to find nearby players that we need to keep distance from.
        /// </summary>
        private float SeperationRadiusThreshold = 1;

        /// <summary>
        /// The weight applied to the alignment steering force.
        /// </summary>
        private float AlignmentForceWeight = 1;

        /// <summary>
        /// The distance used to find nearby players that we need to stay aligned with.
        /// </summary>
        private float AlignmentRadiusThreshold = 2;
        */

        /// <summary>
        /// The weight applied to the collision avoidance steering force.
        /// </summary>
        private float CollisionAvoidanceForceWeight = 5;

        /// <summary>
        /// The distance used to find nearby obstacles that we need to avoid.
        /// </summary>
        private float CollisionAvoidanceRadiusThreshold = 1;


        #endregion

        #region Methods

        public void setLastWinBy(Team team)
        {
            lastWinBy = team;
        }

        public Team getLastWinBy()
        {
            return lastWinBy;
        }

        public string getWinner() 
        {
            return winner;
        }

        public int getGryffindorScore()
        {
            return GryffindorScore;
        }

        public int getSlytherinScore()
        {
            return SlytherinScore;
        }

        /// <summary>
        /// Continuous update the speed and rotation of the player.
        /// </summary>
        private void Update()
        {
            // Initialize the new velocity
            Vector3 acceleration = Vector3.zero;

            if (SlytherinScore >= 100 && GryffindorScore < 100) winner = "Slytherin";

            else if (SlytherinScore < 100 && GryffindorScore >= 100) winner = "Gryffindor";

            /*
            // Compute cohesion
            acceleration += NormalizeSteeringForce(ComputeCohisionForce())
                * CohesionForceWeight;

            // Compute seperation
            acceleration += NormalizeSteeringForce(ComputeSeperationForce())
                * SeperationForceWeight;

            // Compute alignment
            acceleration += NormalizeSteeringForce(ComputeAlignmentForce())
                * AlignmentForceWeight;
            */

            Vector3 avoidance = ComputeCollisionAvoidanceForce();

            // Compute collision avoidance
            acceleration += NormalizeSteeringForce(avoidance) 
                * CollisionAvoidanceForceWeight;

            if (true) { 
                
                System.Random rnd = new System.Random();
                acceleration.x += 0.5f - ((float) rnd.NextDouble());
                acceleration.y += 2.0f - ((float) rnd.NextDouble());
                acceleration.z += 0.5f - ((float) rnd.NextDouble());
            }

            // Compute the new velocity
            Vector3 velocity = Rigidbody.velocity;
            velocity += acceleration * Time.deltaTime;

            // Ensure the velocity remains within the accepted range
            velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude,
                minVelocity, maxVelocity);

            // Apply velocity
            Rigidbody.velocity = velocity;

            // Update rotation
            transform.forward = Rigidbody.velocity.normalized;

            //position = transform.localPosition;
        }

        /// <summary>
        /// Normalizes the steering force and clamps it.
        /// </summary>
        private Vector3 NormalizeSteeringForce(Vector3 force)
        {
            return force.normalized * Mathf.Clamp(force.magnitude, 0, MaxSteerForce);
        }

        /*
        /// <summary>
        /// Computes the cohision force that will pull the player back to the center of the team.
        /// </summary>
        private Vector3 ComputeCohisionForce()
        {
            // Check if this is the only player in the team
            if (Team.Snitchs.Count == 1)
                return Vector3.zero;

            // Check if we are using the center of the team
            if (UseCenterForCohesion)
            {
                // Get current center of the team
                Vector3 center =;

                // Get rid of this player's position from the center
                float newCenterX = center.x * Team.Snitchs.Count - transform.localPosition.x;
                float newCenterY = center.y * Team.Snitchs.Count - transform.localPosition.y;
                float newCenterZ = center.z * Team.Snitchs.Count - transform.localPosition.z;
                Vector3 newCenter = new Vector3(newCenterX, newCenterY, newCenterZ) / (Team.Snitchs.Count - 1);

                // Compute force
                return newCenter - transform.localPosition;
            }

            // Else, use the center of the neighbor players
            float centerX = 0, centerY = 0, centerZ = 0;
            int count = 0;
            foreach (Snitch player in Team.Snitchs)
            {
                if (player == this
                    || (player.transform.position - transform.position).magnitude > CohesionRadiusThreshold)
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
        }

        /// <summary>
        /// Computes the seperation force that ensures a safe distance is kept between the players.
        /// </summary>
        private Vector3 ComputeSeperationForce()
        {
            // Initialize seperation force
            Vector3 force = Vector3.zero;

            // Find nearby players
            foreach (Snitch player in Team.Snitchs)
            {
                if (player == this
                    || (player.transform.position - transform.position).magnitude > SeperationRadiusThreshold)
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
            foreach (Snitch player in Team.Snitchs)
            {
                if (player == this
                    || (player.transform.position - transform.position).magnitude > AlignmentRadiusThreshold)
                    continue;

                force += player.transform.forward;
            }

            return force;
        }
        */

        /// <summary>
        /// Computes the force that helps avoid collision.
        /// </summary>
        private Vector3 ComputeCollisionAvoidanceForce()
        {
            // Check if heading to collision
            if (!Physics.SphereCast(transform.position,
                CollisionAvoidanceRadiusThreshold, 
                transform.forward, 
                out RaycastHit hitInfo,
                CollisionAvoidanceRadiusThreshold))
                return Vector3.zero;

            // Compute force
            return transform.position - hitInfo.point;
        }

        /*
        private void OnTriggerEnter(Collider col)
        {
            // Check that collision is with rigidbody object
            if (col.GetComponent<Rigidbody>() == null) return;

            Rigidbody targetRigidbody = col.GetComponent<Rigidbody>();

            if (targetRigidbody.GetComponent<Player>() != null) {

                if (targetRigidbody.GetComponent<Player>().Team.team == "Slytherin")
                {

                    if (lastWinBy == targetRigidbody.GetComponent<Player>().Team) SlytherinScore += 2;
                    else SlytherinScore += 1;
                }
                else {
                    if (lastWinBy == targetRigidbody.GetComponent<Player>().Team) GryffindorScore += 2;
                    else GryffindorScore += 1;
                }

            }

        }
        */

        #endregion

    }
}
