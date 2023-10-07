using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragAndDrop : MonoBehaviour

{
    public AudioClip DragNoise;
    Vector3 mPosition;
    float mouseZ;
    public LayerMask itemMask;
    public static bool isDragging = false;
    public static bool canDrag = true;
    public static bool isResetting;
    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    private Vector3 GetPositionFromRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,100,itemMask))
        {
            return hit.point + hit.normal*.25f;
        }
        return ray.origin + ray.direction * 5; // changes this to scroll on the screen even if not touching room
    }
    private Vector3 AdjustBasedOnBounds(Vector3 position)
    {
        //IMPORTANT: This only works if ever object has a boxcollider, and specifically a BOX collider.
        var dimensions = GetComponent<BoxCollider>().size/2;
        var modifier = new Vector3(
            position.x - AdjustBasedOnDimension(position, transform.right, dimensions.x),
            position.y - AdjustBasedOnDimension(position, transform.up, dimensions.y),
            position.z - AdjustBasedOnDimension(position, transform.forward, dimensions.z));
        return modifier;
	}

    private float AdjustBasedOnDimension(Vector3 position,Vector3 direction, float distance)
    {
		Ray ray = new Ray(position, direction);
		RaycastHit hit;
		Debug.DrawRay(position, -direction);

		if (Physics.Raycast(ray,out hit, distance,itemMask))
        {
            return distance - hit.distance;
        }
		Ray ray2 = new Ray(position, -direction);
		RaycastHit hit2;

		if (Physics.Raycast(ray2, out hit2, distance,itemMask))
		{
			return -distance + hit2.distance;
		}
		return 0;
	}
    void OnMouseDrag()
    {
        if (canDrag == true)
        {
			//Vector3 wMouse = GetMouseAsWorldPoint();
			Vector3 wMouse = GetPositionFromRaycast();
            wMouse = AdjustBasedOnBounds(wMouse);
			mPosition = new Vector3(wMouse.x, wMouse.y, wMouse.z);
            transform.position = mPosition;
        }
    }

	private void OnMouseDown()
	{
		CustomerManager.Instance.heldItem = GetComponent<Item>().itemName;
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