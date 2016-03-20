using UnityEngine;
using System.Collections.Generic;

public class PhysicsObject : MonoBehaviour
{
    Rigidbody rb;
    Vector3 lastvel;
    bool collided = false;
    List<Collision> colliderNormals;
    public Vector3 GlobalGravity = new Vector3(0, 0, 0);
    public Vector3 gravity = new Vector3(0, 0, 0);
    public Vector3 velocity = new Vector3(0, 0, 0);
    public float bouncyness = 1;
    public float friction = 0;
    public float mass = 1;

    public float maxVelocity = 0;

    void Awake()
    {
        colliderNormals = new List<Collision>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        gravity = CalculateGravity();
        if (colliderNormals.Count >= 1)
        {
            collided = true;
            velocity = Contact();
            velocity = gravityCorrection();
        }
        else
        {
            if (collided) {

                Vector3 tempvel = velocity + gravity * Time.fixedDeltaTime;
                if (!spherecollision(tempvel))
                {
                    velocity = tempvel;
                    collided = false;
                }
            }
            else
            {
                velocity += gravity * Time.fixedDeltaTime;
            }
        }
        if(maxVelocity != 0 && maxVelocity < velocity.magnitude)
        {
            velocity = velocity.normalized * maxVelocity;
        }
        transform.position += velocity * Time.fixedDeltaTime;
        lastvel = velocity;
    }

    Vector3 CalculateGravity()
    {
        Vector3 gravity = GlobalGravity;
        for(int i = 0; i < GravityObject.Objects.Count; i++)
        {
            GravityObject obj = GravityObject.Objects[i];
            if(obj.gameObject == gameObject)
                continue;
            Vector3 direction = obj.transform.position - transform.position;
            float distance = Vector3.Distance(obj.transform.position, transform.position);
            gravity += (direction.normalized / (distance * distance + 1f)) * obj.Mass * mass;
        }
        return gravity;
    }

    bool spherecollision(Vector3 direction)
    {
        return Physics.OverlapSphere(transform.position + direction * Time.deltaTime, 0.5f).Length >= 2;
    }

    void OnCollisionEnter(Collision collision)
    {
        colliderNormals.Add(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        for (int i = 0; i < colliderNormals.Count; i++)
        {
            if (collision.collider.GetInstanceID() == colliderNormals[i].collider.GetInstanceID())
            {
                colliderNormals.RemoveAt(i);
                return;
            }
        }
    }

    Vector3 Contact()
    {
        bool done = false;
        Vector3 tempvel = velocity;
        int count = 0;
        while (!done)
        {
            count++;
            done = true;
            foreach (Collision coll in colliderNormals)
            {
                Vector3 normal = coll.contacts[0].normal;
                if (Vector3.Dot(tempvel, normal) < 0)
                {
                    Vector3 neg = -tempvel;
                    Vector3 proj = Vector3.Project(neg, normal.normalized);
                    tempvel = (tempvel + proj) * (1 - friction) + proj * bouncyness;
                    done = false;
                }
            }
            if(count > 20)
            {
                return -velocity;
            }
        }
        return tempvel;
    }

    Vector3 gravityCorrection()
    {
        bool done = false;
        Vector3 tempvel = velocity + gravity * Time.fixedDeltaTime;
        bool[] grounded = new bool[colliderNormals.Count];
        for (int i = 0; i < colliderNormals.Count; i++)
        {
            grounded[i] = false;
        }
        int count = 0;
        while (!done)
        {
            count++;
            done = true;
            for (int i = 0; i < colliderNormals.Count; i++)
            {
                Vector3 normal = colliderNormals[i].contacts[0].normal;
                if (Vector3.Dot((tempvel), normal) < 0)
                {
                    Vector3 neg = -tempvel;
                    Vector3 proj = Vector3.Project(neg, normal.normalized);
                    tempvel = tempvel + proj;
                    grounded[i] = true;
                    done = false;
                }
            }
            if (count > 50)
            {
                return tempvel - gravity * Time.fixedDeltaTime * 0.5f;
            }
        }
        return tempvel - gravity * Time.fixedDeltaTime * 0.5f;
    }
}