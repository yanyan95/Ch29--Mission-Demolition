using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

    static public FollowCam S;     //a FollowCam Singleton

    //fields set in the Unity Inspector pane
    public float
        easing = 0.05f;
    public Vector2
        minXY;
    public bool
        ____________________________;
    //fields set dynamically
    public GameObject
        poi;     //the point of interest
    public float
        camZ;   //the desired Z pos of the camera

    void Awake ()
    {
        S = this;
        camZ = this.transform.position.z;
    }


    void FixedUpdate ()
    {
        Vector3 destination;
        //if there is no poi, return to P:[0,0,0]
        if(poi == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //get the position of the poi
            destination = poi.transform.position;
            //if poi is a Projectile, check to see if it's at rest
            if (poi.tag == "Projectile")
            {
                //if it is sleeping (that is, not moving)
                if(poi.GetComponent<Rigidbody>().IsSleeping())
                {
                    //return to default view
                    poi = null;
                    MissionDemolition.SwitchView("Both");
                    //in the next update
                    return;
                }
            }
        }


        //limit the X & Y to minmum values
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //interpoloate from the current Camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        //retain a destination.z of camZ
        destination.z = camZ;
        //set the camera to the destination
        transform.position = destination;
        //set the orthographicSize of the Camera to keep Ground in view
        this.GetComponent<Camera>().orthographicSize = destination.y + 10;
    }
}
