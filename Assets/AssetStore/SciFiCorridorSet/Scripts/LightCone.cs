using UnityEngine;
using System.Collections;

public class LightCone : MonoBehaviour {

	public Transform mCameraTr;
	
	void Start()
	{
		if (!mCameraTr)
			mCameraTr = Camera.main.transform;
	}
	
	void Update()
	{
		transform.LookAt(transform.position + mCameraTr.rotation * Vector3.forward, mCameraTr.rotation * Vector3.up);
	}
}
