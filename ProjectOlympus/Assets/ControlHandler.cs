using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHandler : MonoBehaviour {

    [Header("NewtonVR Buttons")]
    [SerializeField]
    private NewtonVR.NVRButton startButton;
    [SerializeField]
    private NewtonVR.NVRButton artButton;
    [SerializeField]
    private NewtonVR.NVRButton musicButton;
    [SerializeField]
    private NewtonVR.NVRButton assetsButton;
    [SerializeField]
    private NewtonVR.NVRButton backButton;

    #region States
    // Note that each of these states should stand alone
    private bool prestart    = true; // before the option to select levels, the start button
    private bool levelselect = false; // level selection
    private bool artlevel    = false; // 2D and 3D art room
    private bool musiclevel  = false; // music room
    private bool assetslevel = false; // armory
    #endregion

    private void Start () {
        AllFalseButRef(ref prestart);
	}
	
	private void Update () {
        StateManager();
	}

    private void StateManager()
    { // to be used in update; handles buttons for different states
        if (prestart)
        {
            AllFalseButRef(ref prestart);
            if (startButton.ButtonIsPushed)
            {
                Debug.Log("Yea baby yea!");
                // Load Main Selection Area here
                AllFalseButRef(ref levelselect);
            }
        }
        if (levelselect)
        {
            AllFalseButRef(ref levelselect);
            if (artButton.ButtonIsPushed)
            {
                // Load art level here
                AllFalseButRef(ref artlevel);
            }
            if (musicButton.ButtonIsPushed)
            {
                // Load music level here
                AllFalseButRef(ref musiclevel);
            }
            if (assetsButton.ButtonIsPushed)
            {
                // Load armory here
                AllFalseButRef(ref assetslevel);
            }
            if (backButton.ButtonIsPushed)
            {
                // Load start menu here
                AllFalseButRef(ref prestart);
            }
        }
        if (artlevel)
        {
            if (backButton.ButtonIsPushed)
            {
                // Load level select here
                AllFalseButRef(ref prestart);
            }
        }
        if (musiclevel)
        {
            if (backButton.ButtonIsPushed)
            {
                // Load level select here
                AllFalseButRef(ref prestart);
            }
        }
        if (assetslevel)
        {
            if (backButton.ButtonIsPushed)
            {
                // Load level select here
                AllFalseButRef(ref prestart);
            }
        }
    }

    private void AllFalseButRef(ref bool b)
    { // used for fail-proofing state changes
        prestart = false;
        levelselect = false;
        artlevel = false;
        musiclevel = false;
        assetslevel = false;

        b = true;
    }
}
