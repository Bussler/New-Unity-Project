using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamera : MonoBehaviour {

    public Transform player;
    public float upDownSpeed = 50;
    private float maxUp=50;
    private float maxDown=-50;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").transform;
        gameObject.transform.SetPositionAndRotation(player.position,player.rotation);//fpCamera pos
        gameObject.transform.SetParent(player);//follow the player
	}

    private void LateUpdate()
    {
        TurnUpDown();
    }

    private void TurnUpDown()
    {
        if (Mathf.Abs(Input.GetAxis("Mouse Y")) > 0)
        {
            float Appliedrotation= (Input.GetAxis("Mouse Y")) * upDownSpeed * Time.deltaTime;
            transform.Rotate(Vector3.left * Appliedrotation);

            //TODO clamping
        }

    }

}
