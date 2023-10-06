using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragAndDrop : MonoBehaviour

{
    public AudioClip DragNoise;
    Vector3 mPosition;
    float mouseZ;
    public static bool isDragging = false;
    public static bool canDrag = true;
    public static bool isResetting;
    private void Start()
    {

    }
    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    void OnMouseDrag()
    {
        if (canDrag == true)
        {
            Vector3 wMouse = GetMouseAsWorldPoint();
            mPosition = new Vector3(wMouse.x, wMouse.y, 0);
            transform.position = mPosition;
        }
    }
    void OnMouseUp()
    {
        if (canDrag == true)
        {
            isDragging = true;
            //implement audio when dragged
            //AudioSource audio = GetComponent<AudioSource>();
            //audio.clip = DragNoise;
            //audio.Play();
        }
    }
    void Update()
    {

        if (isDragging == true)
        {
            isDragging = false;
            //canDrag = false;
            Debug.Log("Has Dragged");
        }
        if (isResetting == true)
        {
            Debug.Log("Reset");
        }
    }
}