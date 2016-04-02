using UnityEngine;
using System.Collections.Generic;

public class PhysicsObject : MonoBehaviour
{
    Rigidbody rb;
    Vector3 lastpos;
    bool collided = false;
    private float radius;
    List<Collision> colliderNormals;
    public Vector3 GlobalGravity = new Vector3(0, 0, 0);
    public Vector3 gravity = new Vector3(0, 0, 0);
    public Vector3 velocity = new Vector3(0, 0, 0);
    public Vector3 windforce = new Vector3(0, 0, 0);
    public float bouncyness = 1;
    public float friction = 0;
    public float mass = 1;
    public float airResistance = 0;
    public float maxVelocity = 0;
    private float adj = 1;

    void Awake()
    {
        radius = transform.localScale.x * 0.5f;
        //Debug.Log(radius);
        colliderNormals = new List<Collision>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (bouncyness < 0.001)
        {
            bouncyness = 0.001f;
        }
        gravity = CalculateGravity();
        Vector3 wind = CalculateWind();
        if (colliderNormals.Count >= 1)
        {
            collided = true;
            velocity = Contact();
            velocity = forceCorrection((gravity+wind)*Time.fixedDeltaTime);
        }
        else
        {
            if (collided) {

                Vector3 tempvel = velocity + (wind + gravity)* Time.fixedDeltaTime;
                if (!spherecollision(tempvel))
                {
                    velocity = tempvel;
                    collided = false;
                }
            }
            else
            {
                velocity += (wind + gravity) * Time.fixedDeltaTime;
            }
        }
        if(maxVelocity != 0 && maxVelocity < velocity.magnitude)
        {
            velocity = velocity.normalized * maxVelocity;
        }
        Debug.DrawRay(transform.position, velocity.normalized,Color.red,4);
        if (adj == 1)
        {
            adj = precalc(velocity);
           // Debug.Log("adj:" + velocity * adj);
            rb.MovePosition(transform.position += velocity * adj * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(transform.position += velocity * adj * Time.fixedDeltaTime);
            //Debug.Log("reverse adj:" + velocity * adj);
            adj = 1;
        }

    }

    //used to scale down velocity to avoid ugly clipping
    // disabled for now since it doesnt work 100%
    
    float precalc(Vector3 vel)
    {
        float mag = (vel*Time.fixedDeltaTime).magnitude;
        // maximum allowed clipping limit by the ball
        float limit = 0.05f;
        if (mag < limit) {
            return 1;
        }
        Vector3 edge = transform.position + vel.normalized * radius;
        if (!(Physics.Linecast(edge, edge+vel*Time.fixedDeltaTime)))
        {
            return 1;
        }
        
        int iterations = 10;
        float scale = 0.25f;
        float search = 0.5f;
        float curr = 1.0f;
        for (int i = 0; i < iterations; i++) {
            if ((Physics.Linecast(edge, edge + search*vel * Time.fixedDeltaTime)))
            {
                curr = search;
                search -= scale;
                //Debug.Log("search decreased: " + search);
                if (curr*mag < limit)
                {
                  //  Debug.Log("broke out of loop");
                    break;
                }
            }
            else
            {
                search += scale;
                //Debug.Log("search increased: " + search);
            }
            scale *= 0.5f;
        }
        //Debug.Log(curr);
        return curr;
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

    Vector3 CalculateWind() {
        Vector3 total = windforce - velocity;
        return airResistance * total;
    }

    bool spherecollision(Vector3 direction)
    {
        return Physics.OverlapSphere(transform.position + direction * Time.deltaTime, radius).Length >= 2;
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

    Vector3 forceCorrection(Vector3 force)
    {
        bool done = false;
        Vector3 tempvel = velocity + force;
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
                return tempvel - force;
            }
        }
        return tempvel - force;
    }

    public void SetGlobalGravity(Vector3 grav) {
        GlobalGravity = grav;
    }

    public void AddForce(Vector3 force) {
        velocity += force;
    }

    public void SetVelocity(Vector3 vel) {
        velocity = vel;
    }
        
}