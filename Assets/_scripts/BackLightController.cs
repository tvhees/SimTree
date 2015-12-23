using UnityEngine;
using System.Collections;

public class BackLightController : MonoBehaviour {
	public float rotateSpeed = 1.0f;
	public float x_Rotate = 30.0f;
	public float rotateAmount = 110.0f;

	private Light backLight;
	private float y_Rotate = 0;

	void Start(){
		backLight = GetComponent<Light> ();
	}

	// Update is called once per frame
	void Update () {
		y_Rotate = Mathf.PingPong (Time.time * rotateAmount * rotateSpeed, rotateAmount) + 180 - rotateAmount;
		backLight.transform.rotation = Quaternion.Euler (x_Rotate, y_Rotate, 0.0f);
	}
}
