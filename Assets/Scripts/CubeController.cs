using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
	public Vector3 GetCreationPosition(RaycastHit hit)
	{
		var face = hit.normal - Vector3.up;

		if (face == new Vector3(0, -1, -1))
			return transform.position + new Vector3(0, 0, -1);

		if (face == new Vector3(0, -1, 1))
			return transform.position + new Vector3(0, 0, 1);

		if (face == new Vector3(0, 0, 0))
			return transform.position + new Vector3(0, 1, 0);

		if (face == new Vector3(1, 1, 1))
			return transform.position + new Vector3(0, -1, 0);

		if (face == new Vector3(-1, -1, 0))
			return transform.position + new Vector3(-1, 0, 0);

		if (face == new Vector3(1, -1, 0))
			return transform.position + new Vector3(1, 0, 0);

		return transform.position;
	}
}
