using Boids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{

    #region Initialization and Updates

    /// <summary>
    /// Team settings.
    /// </summary>
    public TeamSettingScriptable Settings;

    //public GameObject Gryffindor;
    //public GameObject Slytherin;

    //Settings.setManager(this);

    //public GameObject SnitchTemplate;

    private GameObject Snitch;

    private bool GameEnd = false;

    //public Snitch snitch;
    //public GameObject SnitchPrefab;
    //public GameObject SnitchParent;

    //public GameObject Snitch;

    //Last won team
    //private Team lastWinBy;

    //GameObject snitch;

    /// <summary>
    /// Executes once on start.
    /// </summary>
    private void Start()
    {

        Snitch = GameObject.FindGameObjectWithTag("Snitch");

        /*
        //Clear existing snitch
        Clear();

        // Create snitch
        Snitch = GameObject.Instantiate(SnitchPrefab, SnitchParent.transform);

        // Extract its script
        Snitch snitchScript = Snitch.GetComponent<Snitch>();

        // Set random location
        Snitch.transform.localPosition = new Vector3
        (
            UnityEngine.Random.Range(-2f, 2f),
            UnityEngine.Random.Range(-2f, 2f),
            UnityEngine.Random.Range(-2f, 2f)
        );

        // Set random rotation
        Snitch.transform.localEulerAngles = new Vector3
        (
            UnityEngine.Random.Range(0f, 360f),
            UnityEngine.Random.Range(0f, 360f),
            UnityEngine.Random.Range(0f, 360f)
        );
        */

        // Display the app ScoreBoard
        DisplayScoreBoard();

        // Set and display general settings
        DisplayGeneralSettings(true);

        // Set and display cohesion settings
        DisplayCohesionSettings(true);

        // Set and display separation settings
        DisplaySeparationSettings(true);

        // Set and display alignment settings
        DisplayAlignmentSettings(true);
    }

    /// <summary>
    /// Continuously updates the settings.
    /// </summary>
    private void Update()
    {

        if (Snitch.GetComponent<Snitch>().winner == "Slytherin" || Snitch.GetComponent<Snitch>().winner == "Gryffindor") {

            Winner.text = "The Winner is " + Snitch.GetComponent<Snitch>().getWinner() + "!";
            if (Snitch.GetComponent<Snitch>().getWinner() == "Slytherin") Winner.color = Color.green;
            else Winner.color = Color.red;
        }


        // General settings
        UpdateGeneralSettings();
        DisplayGeneralSettings();

        // Cohesion settings
        UpdateCohesionSettings();
        DisplayCohesionSettings();

        // Separation settings
        UpdateSeparationSettings();
        DisplaySeparationSettings();

        // Alignment settings
        UpdateAlignmentSettings();
        DisplayAlignmentSettings();
    }

    #endregion

    #region ScoreBoard

    /// <summary>
    /// Text UI element displaying the app ScoreBoard.
    /// </summary>
    [SerializeField]
    [Tooltip("Text UI element displaying the app ScoreBoard.")]
    public Text ScoreBoard;
    [SerializeField]
    [Tooltip("Text UI element displaying the gryffindor score.")]
    public Text GryffindorScore;
    [SerializeField]
    [Tooltip("Text UI element displaying the slytherin score.")]
    public Text SlytherinScore;
    [SerializeField]
    [Tooltip("Text UI element displaying the winner.")]
    public Text Winner;

    /// <summary>
    /// Displays current project's ScoreBoard.
    /// </summary>
    private void DisplayScoreBoard()
    {
        ScoreBoard.text = string.Format("ScoreBoard");
        GryffindorScore.text = string.Format("G: 0");
        SlytherinScore.text = string.Format("S: 0");
    }
   
    #endregion

    
    #region General Settings

    [Header("General")]

    /// <summary>
    /// Text UI element displaying the minimum speed.
    /// </summary>
    private Text MinimumSpeedTextUI;

    /// <summary>
    /// Slider UI element displaying the minimum speed.
    /// </summary>
    private Slider MinimumSpeedSliderUI;

    /// <summary>
    /// Text UI element displaying the maximum speed.
    /// </summary>
    private Text MaximumSpeedTextUI;

    /// <summary>
    /// Slider UI element displaying the minimum speed.
    /// </summary>
    private Slider MaximumSpeedSliderUI;

    /// <summary>
    /// Text UI element displaying the maximum steering force.
    /// </summary>
    private Text MaximumSteeringForceTextUI;

    /// <summary>
    /// Slider UI element displaying the maximum steering force.
    /// </summary>
    private Slider MaximumSteeringForceSliderUI;

    /// <summary>
    /// Display the current general settings.
    /// </summary>
    
    private void DisplayGeneralSettings(bool initialize = false)
    {
        MinimumSpeedTextUI.text = string.Format("Minimum speed ({0:0.00})", Settings.MinSpeed);
        //MaximumSpeedTextUI.text = string.Format("Maximum speed ({0:0.00})", Settings.MaxSpeed);
        MaximumSteeringForceTextUI.text = string.Format("Max steering force ({0:0.00})", Settings.MaxSteerForce);

        if (initialize)
        {
            MinimumSpeedSliderUI.value = Settings.MinSpeed;
            //MaximumSpeedSliderUI.value = Settings.MaxSpeed;
            MaximumSteeringForceSliderUI.value = Settings.MaxSteerForce;
        }
    }

    /// <summary>
    /// Updates the general settings.
    /// </summary>
    private void UpdateGeneralSettings()
    {
        Settings.MinSpeed = MinimumSpeedSliderUI.value;
        //Settings.MaxSpeed = MaximumSpeedSliderUI.value;
        Settings.MaxSteerForce = MaximumSteeringForceSliderUI.value;
    }

    /// <summary>
    /// Deletes snitches.
    /// </summary>
    /*
    private void Clear()
    {
        GameObject.Destroy(SnitchParent.transform);
    }
    */

    #endregion

    #region Cohesion Settings

    [Header("Cohesion")]

    /// <summary>
    /// Text UI element displaying the cohision force weight.
    /// </summary>
    private Text CohesionForceWeightTextUI;

    /// <summary>
    /// Slider UI element displaying the cohision force weight.
    /// </summary>
    private Slider CohesionForceWeightSliderUI;

    /// <summary>
    /// Text UI element displaying the cohision radius.
    /// </summary>
    private Text CohesionRadiusTextUI;

    /// <summary>
    /// Slider UI element displaying the cohision radius.
    /// </summary>
    private Slider CohesionRadiusSliderUI;

    /// <summary>
    /// Toggle UI element displaying the center status.
    /// </summary>
    private Toggle CohesionUseCenterToggleUI;

    /// <summary>
    /// Display the current cohesion settings.
    /// </summary>
    private void DisplayCohesionSettings(bool initialize = false)
    {
        CohesionForceWeightTextUI.text = string.Format("Force weight ({0:0.00})", Settings.CohesionForceWeight);
        CohesionRadiusTextUI.text = string.Format("Radius ({0:0.00})", Settings.CohesionRadiusThreshold);

        if (initialize)
        {
            CohesionForceWeightSliderUI.value = Settings.CohesionForceWeight;
            CohesionRadiusSliderUI.value = Settings.CohesionRadiusThreshold;
            CohesionUseCenterToggleUI.isOn = Settings.UseCenterForCohesion;
        }
    }

    /// <summary>
    /// Updates the cohesion settings.
    /// </summary>
    private void UpdateCohesionSettings()
    {
        Settings.CohesionForceWeight = CohesionForceWeightSliderUI.value;
        Settings.CohesionRadiusThreshold = CohesionRadiusSliderUI.value;
        Settings.UseCenterForCohesion = CohesionUseCenterToggleUI.isOn;
        Settings.IsCenterVisible = CohesionUseCenterToggleUI.isOn;
    }

    #endregion

    #region Separation Settings

    [Header("Separation")]

    /// <summary>
    /// Text UI element displaying the separation force weight.
    /// </summary>
    private Text SeparationForceWeightTextUI;

    /// <summary>
    /// Slider UI element displaying the separation force weight.
    /// </summary>
    private Slider SeparationForceWeightSliderUI;

    /// <summary>
    /// Text UI element displaying the separation radius.
    /// </summary>
    private Text SeparationRadiusTextUI;

    /// <summary>
    /// Slider UI element displaying the separation radius.
    /// </summary>
    private Slider SeparationRadiusSliderUI;

    /// <summary>
    /// Display the current separation settings.
    /// </summary>
    private void DisplaySeparationSettings(bool initialize = false)
    {
        SeparationForceWeightTextUI.text = string.Format("Force weight ({0:0.00})", Settings.SeperationForceWeight);
        SeparationRadiusTextUI.text = string.Format("Radius ({0:0.00})", Settings.SeperationRadiusThreshold);

        if (initialize)
        {
            SeparationForceWeightSliderUI.value = Settings.SeperationForceWeight;
            SeparationRadiusSliderUI.value = Settings.SeperationRadiusThreshold;
        }
    }

    /// <summary>
    /// Updates the separation settings.
    /// </summary>
    private void UpdateSeparationSettings()
    {
        Settings.SeperationForceWeight = SeparationForceWeightSliderUI.value;
        Settings.SeperationRadiusThreshold = SeparationRadiusSliderUI.value;
    }

    #endregion

    #region Alignment Settings

    [Header("Alignment")]

    /// <summary>
    /// Text UI element displaying the alignment force weight.
    /// </summary>
    private Text AlignmentForceWeightTextUI;

    /// <summary>
    /// Slider UI element displaying the alignment force weight.
    /// </summary>
    private Slider AlignmentForceWeightSliderUI;

    /// <summary>
    /// Text UI element displaying the alignment radius.
    /// </summary>
    private Text AlignmentRadiusTextUI;

    /// <summary>
    /// Slider UI element displaying the alignment radius.
    /// </summary>
    private Slider AlignmentRadiusSliderUI;

    /// <summary>
    /// Display the current alignment settings.
    /// </summary>
    private void DisplayAlignmentSettings(bool initialize = false)
    {
        AlignmentForceWeightTextUI.text = string.Format("Force weight ({0:0.00})", Settings.AlignmentForceWeight);
        AlignmentRadiusTextUI.text = string.Format("Radius ({0:0.00})", Settings.AlignmentRadiusThreshold);

        if (initialize)
        {
            AlignmentForceWeightSliderUI.value = Settings.AlignmentForceWeight;
            AlignmentRadiusSliderUI.value = Settings.AlignmentRadiusThreshold;
        }
    }

    /// <summary>
    /// Updates the alignment settings.
    /// </summary>
    private void UpdateAlignmentSettings()
    {
        Settings.AlignmentForceWeight = AlignmentForceWeightSliderUI.value;
        Settings.AlignmentRadiusThreshold = AlignmentRadiusSliderUI.value;
    }

    #endregion

}
