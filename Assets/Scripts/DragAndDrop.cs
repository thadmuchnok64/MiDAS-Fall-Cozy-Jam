using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;


public class DragAndDrop : MonoBehaviour

{
    public AudioClip DragNoise;
    public AudioClip DropNoise;
    public GameObject dust;
    Vector3 mPosition;
    float mouseZ;
    public LayerMask itemMask;
    public static bool isDragging = false;
    public static bool canDrag = true;
    public static bool isResetting;
    float scrollCooldown = .5f;
    float scrolltimer = 0;
    Vector3 initialPos,initialRot;
    Transform initialParent;
    Rigidbody rb;
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
        initialPos = transform.position;
        initialRot = transform.eulerAngles;
        initialParent = transform.parent;
	}

    public void ResetToDefaultLocation()
    {
        rb.isKinematic = true;
        transform.position = initialPos;
        transform.eulerAngles = initialRot;
        transform.parent = initialParent;
    }
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
        var box = GetComponent<BoxCollider>();
        var dimensions =  new Vector3(
            (box.size.x/2.0f)*transform.localScale.x+.05f,
			(box.size.y / 2.0f)*transform.localScale.y+.05f,
			(box.size.z / 2.0f) * transform.localScale.z+ .05f
            );
        var modifier = new Vector3(
            AdjustBasedOnDimension(position, transform.right, dimensions.x),
            AdjustBasedOnDimension(position, transform.up, dimensions.y),
            AdjustBasedOnDimension(position, transform.forward, dimensions.z));

        modifier = UnityEngine.Quaternion.AngleAxis(transform.localEulerAngles.y, Vector3.up) * modifier;
		modifier = UnityEngine.Quaternion.AngleAxis(transform.localEulerAngles.x, Vector3.right) * modifier;
		modifier = UnityEngine.Quaternion.AngleAxis(transform.localEulerAngles.z, Vector3.forward) * modifier;


		modifier =  new Vector3(position.x - modifier.x, position.y - modifier.y, position.z - modifier.z);
        return modifier;
	}

    private float AdjustBasedOnDimension(Vector3 position,Vector3 direction, float distance)
    {
		Ray ray = new Ray(position, direction);
		RaycastHit hit;

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
            if(Input.mouseScrollDelta.y > 0 && scrolltimer > scrollCooldown)
            {
                scrolltimer = 0;
                StartCoroutine(Scroll(true));
            } else if (Input.mouseScrollDelta.y < 0 && scrolltimer > scrollCooldown)
			{
				scrolltimer = 0;
				StartCoroutine(Scroll(false));
			}
			//Vector3 wMouse = GetMouseAsWorldPoint();
			Vector3 wMouse = GetPositionFromRaycast();
            wMouse = AdjustBasedOnBounds(wMouse);
			mPosition = new Vector3(wMouse.x, wMouse.y, wMouse.z);
            transform.position = mPosition;
        }
    }

    
	private void OnMouseDown()
	{
		CustomerManager.Instance.heldItem = GetComponent<Item>();
        transform.parent = null;
        SoundEffectRequest.instance.PlaySound(DragNoise);
	}
	void OnMouseUp()
    {
        if (canDrag == true)
        {
            isDragging = true;
            rb.isKinematic= false;
            //StartCoroutine(FallDown());
            //implement audio when dragged
            //AudioSource audio = GetComponent<AudioSource>();
            //audio.clip = DragNoise;
            //audio.Play();
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.tag == "ItemRespawn")
            ResetToDefaultLocation();
	}
	private void OnCollisionEnter(Collision collision)
	{
        rb.isKinematic = true;
        transform.parent = collision.transform;
		SoundEffectRequest.instance.PlaySound(DropNoise);
        Instantiate(dust, collision.GetContact(0).point, Quaternion.identity);
	}

	/*
    public IEnumerator FallDown()
    {
        var offset = Vector3.Scale(GetComponent<BoxCollider>().size,transform.localScale);
		RaycastHit hit;
        Vector3 target;
        if (Physics.BoxCast(transform.position, offset / 3.0f, Vector3.down,out hit, transform.rotation, 100, itemMask))
        {
            target = new Vector3(transform.position.x, (hit.point.y + offset.y/2f),transform.position.z);
            Debug.Log(target.y);
            for (float i = 0; i < 30; i++)
            {

				Debug.DrawLine(transform.position, target);
                transform.position = Vector3.Lerp(transform.position, target, i / 30.0f);
                yield return new WaitForFixedUpdate();
            }
			transform.position = Vector3.Lerp(transform.position, target, 1);
            transform.parent = hit.transform;
		}

	}
    */
	public IEnumerator Scroll(bool positive)
    {
        Quaternion attemptrot;
        if (positive) { attemptrot= Quaternion.AngleAxis(transform.eulerAngles.y + 90, Vector3.up); }
		else { attemptrot = Quaternion.AngleAxis(transform.eulerAngles.y - 90, Vector3.up); }


		for (float i = 0; i < 30; i++) {
            transform.rotation = Quaternion.Lerp(transform.rotation, attemptrot,i/30.0f);
            yield return new WaitForFixedUpdate();
        }
		transform.rotation = Quaternion.Lerp(transform.rotation, attemptrot, 1);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,90*Mathf.Round(transform.eulerAngles.y/90), transform.eulerAngles.z);

	}
	void Update()
    {
        scrolltimer += Time.deltaTime;
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