using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DemoUI3 : MonoBehaviour
{
    public float force = 100;

    public PhysicsObject Object000;
    public PhysicsObject Object001;
    public PhysicsObject Object005;
    public PhysicsObject Object010;
    public PhysicsObject Object050;
    public PhysicsObject Object100;

    public void SendObject000()
    {
        Object000.AddForce(Vector3.right * force);
    }

    public void SendObject001()
    {
        Object001.AddForce(Vector3.right * force);
    }

    public void SendObject005()
    {
        Object005.AddForce(Vector3.right * force);
    }

    public void SendObject010()
    {
        Object010.AddForce(Vector3.right * force);
    }

    public void SendObject050()
    {
        Object050.AddForce(Vector3.right * force);
    }

    public void SendObject100()
    {
        Object100.AddForce(Vector3.right * force);
    }
}
