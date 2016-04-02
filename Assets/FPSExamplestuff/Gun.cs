using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public GameObject projectile;
    public float velocity;
    public Vector3 gravity;
    public float bouncyness = 1;
    public float friction = 0;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1")) {
            GameObject proj = (GameObject)Instantiate(projectile, transform.position + transform.forward*0.3f, Quaternion.identity);
            PhysicsObject script = proj.GetComponent<PhysicsObject>();
            script.SetGlobalGravity(gravity);
            script.SetVelocity(transform.forward*velocity);
            script.bouncyness = bouncyness;
            script.friction = friction;
        }
	
	}
}
