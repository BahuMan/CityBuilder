using UnityEngine;
using System.Collections;

public class FPSFly : MonoBehaviour {

    public float lookSpeed = 150.0f;
    public float moveSpeed = 50.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        rotationX += Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        transform.position += transform.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += transform.right * moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
    }
}
