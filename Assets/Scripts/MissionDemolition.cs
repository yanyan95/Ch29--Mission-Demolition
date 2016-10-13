using UnityEngine;
using System.Collections;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour {
    static public MissionDemolition
        S;   //a Singleton

    //fields set in the Unity Inspector pane
    public GameObject[]
        castles; //an array of the castles
    public GUIText
        gtLevel;   //the GT_Level GUIText
    public GUIText
        gtScore;  //the GT_Score GUIText
    public Vector3
        castlePos;   //the place to put castles

    public bool
        ____________________________;

    //fields set dynamically
    public int
        level;   //the current level
    public int
        levelMax;   //the number of levels
    public int
        shotsTaken;
    public GameObject
        castle;   //the current castle
    public GameMode
        mode = GameMode.idle;
    public string
        showing = "Slingshot";   //FollowCam mode

    void Start()
    {
        S = this;   //define the Singleton

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        //get rid of the old castle if one exists
        if (castle != null)
        {
            Destroy(castle);
        }

        //destroy old projectiles if they exist
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //Instantiate the new castle
        castle = Instantiate(castles[level]) as GameObject;
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //reset the camera
        SwitchView("Both");
        ProjectileLine.S.Clear();

        //reset the goal
        Goal.goalMet = false;

        ShowGT();

        mode = GameMode.playing;
    }
    

    void ShowGT()
    {
        //show the data in the GUITexts
        gtLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        gtScore.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        ShowGT();

        //check for level end
        if (mode == GameMode.playing && Goal.goalMet)
        {
            //change mode to stop checking for level end
            mode = GameMode.levelEnd;
            //zoom out
            SwitchView("Both");
            //start the next level in 2 seconds 
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if(level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    void OnGUI()
    {
        //draw the GUI button for view switching at the top of the screen
        Rect buttonRect = new Rect((Screen.width / 2) - 50, 10, 100, 24);

        switch (showing)
        {
            case "Slingshot":
                if (GUI.Button (buttonRect, "Show Castle"))
                {
                    SwitchView("Castle");
                }
                break;

            case "Castle":
                if (GUI.Button(buttonRect, "Show Both"))
                {
                    SwitchView("Both");
                }
                break;

            case "Both":
                if (GUI.Button(buttonRect, "Show Slingshot"))
                {
                    SwitchView("Slingshot");
                }
                break;
        }
    }

    //static method that allows code anywhere to request a veiw change
    static public void SwichView (string eView)
    {
        S.showing = eView;
        switch (S.showing)
        {
            case "Slingshot":
                FollowCam.S.poi = null;
                break;

            case "Castle":
                FollowCam.S.poi = S.castle;
                break;

            case "Both":
                FollowCam.S.poi = GameObject.Find("ViewBoth");
                break;
        }
    }


    //static method that allows code anywhere to increment shotsTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
