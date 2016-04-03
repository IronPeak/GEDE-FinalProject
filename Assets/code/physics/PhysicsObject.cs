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
            if (collided)
            {

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
        //Debug.DrawRay(transform.position, velocity.normalized,Color.red,4);
        if (adj == 1)
        {
            adj = precalc(velocity);
            rb.MovePosition(transform.position += velocity * adj * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(transform.position += velocity * adj * Time.fixedDeltaTime);
            adj = 1;
        }

    }

    //used to scale down velocity to avoid ugly clipping
    
    float precalc(Vector3 vel)
    {
        float mag = (vel*Time.fixedDeltaTime).magnitude;
        Vector3 edge = transform.position + vel.normalized * radius;
        RaycastHit hitinfo;
        if (!(Physics.Linecast(edge, edge + vel * Time.fixedDeltaTime,out hitinfo)))
        {
            return 1;
        }
        Vector3 diff = (hitinfo.point - edge) * 1.001f;
        return diff.magnitude / mag;
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
                    PhysicsObject script = coll.gameObject.GetComponent<PhysicsObject>();
                    if (script != null)
                    {
                        script.AddForce(-(1 - bouncyness) * proj );
                    }
                    done = false;
                }
            }
            //if collision cant be solved properly in x steps just return what you have
            if(count > 30)
            {
                return tempvel;
            }
        }
        return tempvel;
    }

    Vector3 forceCorrection(Vector3 force)
    {
        bool done = false;
        Vector3 tempvel = velocity + force;
        int count = 0;
        bool conflict = false;
        foreach (Collision coll in colliderNormals)
        {

            Vector3 normal = coll.contacts[0].normal;
            if (Vector3.Dot(normal, force) <= -0.0001)
            {
                conflict = true;
                break;
            }
        }
        if (!conflict) {
            return tempvel;
        }
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