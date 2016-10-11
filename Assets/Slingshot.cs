using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {
    //fields set in the Unity Inspector pane
    public GameObject
        prefabProjectile;
    public float
        velocityMult = 4f;
    public bool
        __________________________;

    //fields set dynamically
    public GameObject
        launchPoint;
    public Vector3
        launchPos;
    public GameObject
        projectile;
    public bool
        aimingMode;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

	void OnMouseEnter () {
        //print("Slingshot: OnMouseEnter()");
        launchPoint.SetActive(true);
	}
	
	void OnMouseExit () {
        //print("Slingshot: OnMouseExit()");
        launchPoint.SetActive(false);
	}

    void OnMouseDown ()
    {
        //the player has pressed the mouse button while over slingshot
        aimingMode = true;
        //instantiate a projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        //start it at the launchPoint
        projectile.transform.position = launchPos;
        //set it to isKinematic for now
        projectile.GetComponent<Rigidbody>().isKinematic = true;  //changed it a bit to see if there is still an error..LOOK AT BOOK FOR THE ORIGINAL VER.
    }


    void Update()
    {
        //if slingshot is not in aimingMode, don't run this code
        if (!aimingMode)
            return;
        //get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        //convert the mouse position to 3d world coordinates
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        //find the delta from the luanchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //limit mouseDelta to the radius of the Slingshot SphereCollider

    }
}
