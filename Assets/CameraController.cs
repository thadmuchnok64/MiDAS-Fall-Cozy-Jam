using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject cam;
    public GameObject[] positionPoints;
    public int startingPoint;

    // The time at which the animation started.
    private float startTime;

    // Time to move from sunrise to sunset position, in seconds.
    public float journeyTime = 1.0f;



    // Start is called before the first frame update
    void Start()
    {
        //rotateCamera();

        camSlerp();
    }

    // Update is called once per frame
    void Update()
    {
        


        // Interpolate over the arc relative to center
        Vector3 slerpStart = cam.transform.position;
        Vector3 slerpEnd = positionPoints[startingPoint].transform.position;

        // The fraction of the animation that has happened so far is
        // equal to the elapsed time divided by the desired time for
        // the total journey.
        float fracComplete = (Time.time - startTime) / journeyTime;

        cam.transform.position = Vector3.Slerp(slerpStart, slerpEnd, fracComplete);

        
        Vector3 slerpRotStart = cam.transform.eulerAngles;
        Vector3 slerpRotEnd = slerpRotStart;
        slerpRotEnd.y = 45 + 90 * startingPoint;
        cam.transform.eulerAngles = Vector3.Slerp(slerpRotStart, slerpRotEnd, fracComplete);



    }

    private void rotateCamera()
    {
        cam.transform.position = positionPoints[startingPoint].transform.position;
        Vector3 rot = cam.transform.eulerAngles;
        rot.y = 45 + 90 * startingPoint;
        cam.transform.eulerAngles = rot;
    }


    private void camSlerp()
    {
        // Note the time at the start of the animation.
        startTime = Time.time;
    }
    

}
