using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiver : MonoBehaviour {

    public GameObject arrowPrefab;
    private GameObject heldArrow;

    private Vector3 throwVelocity;
    private Vector3 previousPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (GvrController.ClickButtonDown)
        {
            CreateArrow();
        }

        if (heldArrow == null) return;

        CalculateThrowVelocity();

        if (GvrController.ClickButtonUp)
        {
            ReleaseArrow();
        }
	}


    void CreateArrow()
    {
        // create the arrow
        GameObject arrow = Instantiate(arrowPrefab);

        // position and orient the arrow near the arm
        HoldArrow(arrow);
    }

    void HoldArrow(GameObject arrow)
    {
        heldArrow = arrow;

        // make arrow a child of the player and rotate it appropriately

        heldArrow.transform.localPosition = new Vector3(0, 0, 1);
        heldArrow.transform.localEulerAngles = new Vector3(90, 0, 0);
        heldArrow.transform.SetParent(transform, false);
    }

    void ReleaseArrow()
    {
        // change the parent to the world
        heldArrow.transform.SetParent(null, true);

        // nullify current velocity
        Rigidbody arrowRigidbody = heldArrow.GetComponent<Rigidbody>();
        arrowRigidbody.velocity = Vector3.zero;
        arrowRigidbody.isKinematic = false;

        arrowRigidbody.AddForce(throwVelocity, ForceMode.VelocityChange);

        heldArrow = null;
    }

    void CalculateThrowVelocity ()
    {
        //the velocity is based on the previous position
        throwVelocity = (heldArrow.transform.position - previousPosition) / Time.deltaTime;

        // update previous position
        previousPosition = heldArrow.transform.position;
    }

}
