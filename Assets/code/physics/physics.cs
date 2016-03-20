using UnityEngine;
using System.Collections;

public class physics : MonoBehaviour
{
    bool collided = false;
    bool hasexited = true;
    Rigidbody rb;
    Vector3 lastpos;
    Vector3 collnormal;
    [SerializeField]
    Vector3 gravity = new Vector3(0, 0, 0);
    [SerializeField]
    Vector3 velocity = new Vector3(0.1f, 0, 0);
    [SerializeField]
    float bouncyness = 1;
    [SerializeField]
    float friction = 0;
    //ideas for later:
    //air resistance,
    //wind force
    //stickyness


    // Use this for initialization
    void Start()
    {
        collnormal = Vector3.zero;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(collided && hasexited)
        {
            //if the vectors almost cancel each other out, assume that they did and fully reflect the vector
            Bounce(collnormal);
            velocity += gravity * Time.fixedDeltaTime;
            rb.MovePosition(transform.position += velocity * Time.fixedDeltaTime);
            hasexited = false;
        }
        else if(collided)
        {
        }
        else
        {
            velocity += gravity * Time.fixedDeltaTime;
            lastpos = transform.position;
            Debug.DrawRay(transform.position, velocity.normalized, Color.red, 5);
            rb.MovePosition(transform.position += velocity * Time.fixedDeltaTime);
            hasexited = true;
        }
        collnormal = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        collided = true;
        foreach(ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal * 5, Color.black, 5);
        }
        collnormal += collision.contacts[0].normal;
        transform.position = lastpos;

    }

    void OnCollisionExit(Collision collision)
    {
        collided = false;
    }

    //includes friction
    void Bounce(Vector3 normal)
    {
        Vector3 neg = -velocity;
        Vector3 proj = Vector3.Project(neg, normal.normalized);
        velocity = (velocity + proj) * (1 - friction) + proj * bouncyness;
        Debug.DrawRay(transform.position, velocity, Color.green, 5);
        Debug.Log(normal);
    }
}