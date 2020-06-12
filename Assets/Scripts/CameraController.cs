using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float RotationSpeed = 7f;

	void Start()
    {

	}

    void Update()
    {
		transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0f, 0f) * RotationSpeed);
	}
}
