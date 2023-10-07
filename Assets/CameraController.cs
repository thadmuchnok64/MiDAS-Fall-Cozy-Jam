using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CameraController : MonoBehaviour
{

    public GameObject cam;
    public GameObject[] positionPoints;
    public int camDestinationPoint = 0;
    public int camRotationCount = 0; //makes the math on rotation work

    // The time at which the animation started.
    private float startTime = 0;

    // Time to rotate room.
     float journeyTime = 5.0f; //mess with these values in an insane way to get the slerp times to change


    Vector3 slerpStart = new Vector3();
    Vector3 slerpEnd = new Vector3();

    public GameObject xGroup;
    public GameObject zGroup;
    public GameObject negXGroup;
    public GameObject negZGroup;

    bool mustHideObjects = false;


    // Start is called before the first frame update
    void Start()
    {
        //camRotationCount = camDestinationPoint;
        setPosAndRotOfCamera();

        hideShowObjects();

        beginSlerp();

        
    }

    // Update is called once per frame
    void Update()
    {

        

        // Interpolate over the arc relative to center
        slerpStart = cam.transform.position;
        slerpEnd = positionPoints[camDestinationPoint].transform.position;

        // The fraction of the animation that has happened so far is
        // equal to the elapsed time divided by the desired time for
        // the total journey.
        float fracComplete = ((Time.time - startTime) / journeyTime)/10;

        cam.transform.position = Vector3.Slerp(slerpStart, slerpEnd, fracComplete);
        Debug.Log(fracComplete);

        Vector3 slerpRotStart = cam.transform.eulerAngles;
        Vector3 slerpRotEnd = slerpRotStart;
        slerpRotEnd.y = 45 + 90 * camRotationCount;
        cam.transform.rotation = Quaternion.Slerp(Quaternion.Euler(slerpRotStart), Quaternion.Euler(positionPoints[camDestinationPoint].transform.eulerAngles), fracComplete);


        
        if (mustHideObjects)
        {
            if (Time.time - startTime > .6) //set when hideShow objects runs
            {
                hideShowObjects();
            }
            
        }

        

    }

    private void setPosAndRotOfCamera()
    {
        cam.transform.position = positionPoints[camDestinationPoint].transform.position;
        Vector3 rot = cam.transform.eulerAngles;
        rot.y = 45 + 90 * camRotationCount;
        cam.transform.rotation = Quaternion.Euler( rot);
    }


    private void beginSlerp()
    {
        // Note the time at the start of the animation.
        startTime = Time.time;
        mustHideObjects = true;


    }


    public void TurnClockwise()
    {

        if((cam.transform.position - slerpEnd).magnitude > .2) //if the current camera location is close to the destination, you can press the rotate button
        {
            Debug.Log("wait until rotation finished before running again");
            return;
        }

        int newCamDest = camDestinationPoint + 1;
        camRotationCount++;
        if (newCamDest > 3)
        {
            newCamDest = 0;
        }
        camDestinationPoint = newCamDest;
         beginSlerp();
    }

    public void TurnCounterClockwise()
    {

        if ((cam.transform.position - slerpEnd).magnitude > .2) //if the current camera location is close to the destination, you can press the rotate button
        {
            Debug.Log("wait until rotation finished before running again");
            return;
        }

       int newCamDest = camDestinationPoint - 1;
        camRotationCount--;
        if(camRotationCount < 0)
        {
            camRotationCount = 3;
        }
        if (newCamDest < 0)
        {
            newCamDest = 3;
        }
        camDestinationPoint = newCamDest;
        beginSlerp();
    }


    //disables and enables the appropriate parent objects
    //if we want to change this to an animation, do it here and disable and enable objects after the animation finishes
    void hideShowObjects()
    {
        if(camDestinationPoint == 0)
        {
            xGroup.SetActive(true);
            zGroup.SetActive(true);
            negXGroup.SetActive(false);
            negZGroup.SetActive(false);

        }
        else if (camDestinationPoint == 1)
        {
            xGroup.SetActive(true);
            zGroup.SetActive(false);
            negXGroup.SetActive(false);
            negZGroup.SetActive(true);
        }
        else if (camDestinationPoint == 2)
        {
            xGroup.SetActive(false);
            zGroup.SetActive(false);
            negXGroup.SetActive(true);
            negZGroup.SetActive(true);
        }
        else if (camDestinationPoint == 3)
        {
            xGroup.SetActive(false);
            zGroup.SetActive(true);
            negXGroup.SetActive(true);
            negZGroup.SetActive(false);
        }

        mustHideObjects = false;
    }


}
