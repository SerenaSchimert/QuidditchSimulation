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

            }
            else if (_TeamSettings.team == "Slytherin") {

                weightMean = 75;
                weightStdev = 12;
                maxVelocityMean = 18;
                maxVeloctiyStdev = 2;
                aggressionMean = 22;
                aggressionStdev = 3;
                maxExhaustionMean = 65;
                maxExhaustionStdev = 13;

            }

            // Create new players (number of players should be even for better distribution results)
            for (int i = 0; i < numberOfPlayers - 1; i += 2) {

                    // Generate 2 values along team attribute distribution for each player attribute

                    var w = GeneratePlayerSettings(weightMean, weightStdev);
                    var mV = GeneratePlayerSettings(maxVelocityMean, maxVeloctiyStdev);
                    var a = GeneratePlayerSettings(aggressionMean, aggressionStdev);
                    var mE = GeneratePlayerSettings(maxExhaustionMean, maxExhaustionStdev);

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

        public int score;

        public float weightMean = 0;
        public float weightStdev = 0;
        public float aggressionMean = 0;
        public float aggressionStdev = 0;
        public float maxVelocityMean = 0;
        public float maxVeloctiyStdev = 0;
        public float maxExhaustionMean = 0;
        public float maxExhaustionStdev = 0;

        [Header("Center")]

        /// <summary>
        /// The sphere representing the center of the Team.
        /// </summary>
        [SerializeField]
        [Tooltip("The sphere representing the center of the Team.")]
        private GameObject Center;

        /// <summary>
        /// The current center (local position) of the Team.
        /// </summary>
        [SerializeField]
        [Tooltip("The current center (local position) of the Team.")]
        private Vector3 _CenterPosition;

        /// <summary>
        /// The current center (local position) of the Team.
        /// </summary>
        public Vector3 CenterPosition { get { return _CenterPosition; } }



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
            score += 1;

            if(team == "Gryffindor")
                GryffindorScore.text = string.Format("G: {0}", score);
            else if (team == "Slytherin")
                SlytherinScore.text = string.Format("S: {0}", score);
        }

        /// <summary>
        /// Continuously compute the position of the center of the Team.
        /// </summary>
        private void Update()
        {
            DisplayScoreBoard();
            // Compute the center
            float centerX = 0, centerY = 0, centerZ = 0;
            foreach (Player player in _Players)
            {
                centerX += player.transform.localPosition.x;
                centerY += player.transform.localPosition.y;
                centerZ += player.transform.localPosition.z;
            }
            _CenterPosition = new Vector3(centerX, centerY, centerZ) / _Players.Count();

            // Move the sphere to the center
            Center.transform.localPosition = _CenterPosition;

            // Update sphere visibility
            Center.gameObject.SetActive(_TeamSettings.IsCenterVisible);

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
        public Tuple<float, float> GeneratePlayerSettings(float mean, float standDev)
        {

            bool generate = true;

            float x = 0;
            float y = 0;

            while (generate)
            {
                System.Random rnd = new System.Random();

                // first generate number between -1 and 1 (precision only to 0.001)
                float u = ((float)rnd.Next(-1000, 1000)) / 1000.0f;
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

        #endregion

    }
}
