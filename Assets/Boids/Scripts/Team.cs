using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Boids
{
    public class Team : MonoBehaviour
    {

        #region Initialization

        /// <summary>
        /// Executes once on awake.
        /// </summary>
        private void Awake()
        {
            if (_TeamSettings == null)
                _TeamSettings = ScriptableObject.CreateInstance<TeamSettingScriptable>();

            if (_TeamSettings.NumberOfPlayersToGenerateOnAwake > 0)
                Initialize(_TeamSettings.NumberOfPlayersToGenerateOnAwake);

            team = _TeamSettings.team;

            Snitch = GameObject.FindGameObjectWithTag("Snitch");

            //_SnitchPosition = Snitch.transform.localPosition;
        }

        /// <summary>
        /// Generates the players in the Team.
        /// </summary>
        /// <param name="numberOfPlayers">The number of players to be generated in this Team.</param>
        public void Initialize(int numberOfPlayers)
        {
            // Clear any existing player
            Clear();

            /*
                weightMean = _TeamSettings.wMean;
                weightStdev = _TeamSettings.wStdev;
                aggressionMean = _TeamSettings.aMean;
                aggressionStdev = _TeamSettings.aStdev;
                maxVelocityMean = _TeamSettings.mVMean;
                maxVeloctiyStdev = _TeamSettings.mVStdev;
                maxExhaustionMean = _TeamSettings.mEMean;
                maxExhaustionStdev = _TeamSettings.mEStdev;
            */

            System.Random rnd = new System.Random();

            if (_TeamSettings.team == "Slytherin")
            {

                weightMean = 85;
                weightStdev = 17;
                maxVelocityMean = 16;
                maxVeloctiyStdev = 2;
                aggressionMean = 30;
                aggressionStdev = 7;
                maxExhaustionMean = 50;
                maxExhaustionStdev = 15;

                // added/chosen attributes
                steadFastProbMean = 0.35f;
                steadFastProbStdev = 0.15f;
                recoveryRateMean = 0.50f;
                recoveryRateStdev = 0.10f;

                spawnPoint = spawnPointSlytherin.position;

            }
            else if (_TeamSettings.team == "Gryffindor") {

                weightMean = 75;
                weightStdev = 12;
                maxVelocityMean = 18;
                maxVeloctiyStdev = 2;
                aggressionMean = 22;
                aggressionStdev = 3;
                maxExhaustionMean = 65;
                maxExhaustionStdev = 13;

                // added/chosen attributes
                steadFastProbMean = 0.55f;
                steadFastProbStdev = 0.25f;
                recoveryRateMean = 0.60f;
                recoveryRateStdev = 0.15f;

                spawnPoint = spawnPointGryffindor.position;

            }

            // Create new players (number of players should be even for better distribution results)
            for (int i = 0; i < numberOfPlayers - 1; i += 2)
            {

                // Generate 2 values along team attribute distribution for each player attribute

                var w = GeneratePlayerSettings(weightMean, weightStdev, ref rnd);
                var mV = GeneratePlayerSettings(maxVelocityMean, maxVeloctiyStdev, ref rnd);
                var a = GeneratePlayerSettings(aggressionMean, aggressionStdev, ref rnd);
                var mE = GeneratePlayerSettings(maxExhaustionMean, maxExhaustionStdev, ref rnd);

                // Create 2 new players
                CreatePlayer(w.Item1, mV.Item1, a.Item1, mE.Item1);
                CreatePlayer(w.Item2, mV.Item2, a.Item2, mE.Item2);
            }


        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// A scriptable object instance that contains the Team's settings.
        /// </summary>
        [Tooltip("A scriptable object instance that contains the Team's settings.")]
        [SerializeField]
        private TeamSettingScriptable _TeamSettings;

        /// <summary>
        /// A scriptable object instance that contains the Team's settings.
        /// </summary>
        public TeamSettingScriptable TeamSettings { get { return _TeamSettings; } }

        public Vector3 spawnPoint = Vector3.zero;

        public GameObject Snitch;

        public int score;

        public float weightMean = 0;
        public float weightStdev = 0;
        public float aggressionMean = 0;
        public float aggressionStdev = 0;
        public float maxVelocityMean = 0;
        public float maxVeloctiyStdev = 0;
        public float maxExhaustionMean = 0;
        public float maxExhaustionStdev = 0;

        // added/chosen attributes
        public float steadFastProbMean = 0;
        public float steadFastProbStdev = 0;
        public float recoveryRateMean = 0;
        public float recoveryRateStdev = 0;

        /*
        [Header("Snitch")]

        /// <summary>
        /// The sphere representing the Snitch of the Team.
        /// </summary>
        [SerializeField]
        [Tooltip("The sphere representing the Snitch of the Team.")]
        public GameObject Snitch;
        */

        /*
        /// <summary>
        /// The current snitch (local position) of the Team.
        /// </summary>
        [SerializeField]
        [Tooltip("The current snitch position")]
        private Vector3 _SnitchPosition;

        /// <summary>
        /// The current Snitch (local position) of the Team.
        /// </summary>
        public Vector3 SnitchPosition { get { return _SnitchPosition; } }
        */

        public Transform spawnPointSlytherin;
        public Transform spawnPointGryffindor;



        [Header("Players")]

        /// <summary>
        /// The prefab template used to generate players in this Team.
        /// </summary>
        [SerializeField]
        [Tooltip("The prefab template used to generate players in this Team.")]
        private GameObject PlayerTemplate;

        /// <summary>
        /// The parent holding all the generated players.
        /// </summary>
        [SerializeField]
        [Tooltip("The parent holding all the generated players.")]
        private GameObject PlayersParent;

        /// <summary>
        /// List of all the players in this Team.
        /// </summary>
        [SerializeField]
        [Tooltip("List of all the players in this Team.")]
        private List<Player> _Players;

        /// <summary>
        /// List of all the players in this Team.
        /// </summary>
        public List<Player> Players { get { return _Players; } }

        #endregion

        #region Methods

        [SerializeField]
        [Tooltip("Text UI element displaying the gryffindor score.")]
        private Text GryffindorScore;
        [SerializeField]
        [Tooltip("Text UI element displaying the slytherin score.")]
        private Text SlytherinScore;
        private string team;

        private void DisplayScoreBoard()
        {
            //score += 1;

            if(team == "Gryffindor")
                GryffindorScore.text = string.Format("G: {0}", score);
            else if (team == "Slytherin")
                SlytherinScore.text = string.Format("S: {0}", score);
        }

        /// <summary>
        /// Continuously compute the position of the snitch of the Team.
        /// </summary>
        private void Update()
        {
            DisplayScoreBoard();

            /*
            // Compute the snitch
            float snitchX = 0, snitchY = 0, snitchZ = 0;
            foreach (Player player in _Players)
            {
                snitchX += player.transform.localPosition.x;
                snitchY += player.transform.localPosition.y;
                snitchZ += player.transform.localPosition.z;
            }
            //_SnitchPosition = new Vector3(snitchX, snitchY, snitchZ) / _Players.Count();

            */

            //_SnitchPosition = Snitch.transform.localPosition;

            // Update sphere visibility
            //Snitch.gameObject.SetActive(true);

            //txt.GetComponent<UnityEngine.UI.Text>().text = score.ToString();
        }

        /// <summary>
        /// Deletes all generated players.
        /// </summary>
        private void Clear()
        {
            _Players = new List<Player>();
            foreach (Transform player in PlayersParent.transform)
                GameObject.Destroy(player.transform);
        }

        /// <summary>
        /// Adds a new player to the team.
        /// </summary>
        private void CreatePlayer(float weight, float maxVelocity, float aggressiveness, float maxExhaustion)
        {
            // Initialize list
            if (_Players == null)
                _Players = new List<Player>();

            // Create new player
            GameObject player = GameObject.Instantiate(PlayerTemplate, PlayersParent.transform);

            // Extract its script
            Player playerScript = player.GetComponent<Player>();
            _Players.Add(playerScript);

            // Set random location
            player.transform.localPosition = new Vector3
            (
                UnityEngine.Random.Range(-2f, 2f),
                UnityEngine.Random.Range(-2f, 2f),
                UnityEngine.Random.Range(-2f, 2f)
            );

            // Set random rotation
            player.transform.localEulerAngles = new Vector3
            (
                UnityEngine.Random.Range(0f, 360f),
                UnityEngine.Random.Range(0f, 360f),
                UnityEngine.Random.Range(0f, 360f)
            );

            // Initialize player attributes
            playerScript.Initialize(this, weight, maxVelocity, aggressiveness, maxExhaustion);
        }



        /// <summary>
        /// Code referenced/exprapolated from discussion of generating normal distribution-compliant values found here: 
        /// https://stats.stackexchange.com/questions/16334/how-to-sample-from-a-normal-distribution-with-known-mean-and-variance-using-a-co
        /// <summary>
        public Tuple<float, float> GeneratePlayerSettings(float mean, float standDev, ref System.Random rnd)
        {

            // first generate 2 numbers between 0 and 1
            float u1 = (float)rnd.NextDouble();
            float u2 = (float)rnd.NextDouble();

            float x = (float) ( Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2) );
            float y = (float) ( Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2) );

            // Convert generated random numbers to deviation from mean
            var numbers = Tuple.Create((x * standDev + mean), (y * standDev + mean));

            return numbers;

        }

        #endregion

    }
}
