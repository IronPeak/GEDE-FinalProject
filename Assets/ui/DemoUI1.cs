using UnityEngine;
using UnityEngine.UI;

public class DemoUI1 : MonoBehaviour
{

    public GravityObject Earth;
    public Slider EarthMass;

    public GameObject Objects1;
    public GameObject Objects2;
    public GameObject Objects3;

    public PhysicsObject PhysicsObject1;
    public PhysicsObject PhysicsObject2;
    public PhysicsObject PhysicsObject3;

    public Slider Moon1Mass;
    public Slider Moon1Bouncyness;

    public Slider Moon2Mass;
    public Slider Moon2Bouncyness;

    public Slider Moon3Mass;
    public Slider Moon3Bouncyness;

    public GameObject Objects1UI;
    public GameObject Objects2UI;
    public GameObject Objects3UI;

    public void Update()
    {
        Earth.Mass = EarthMass.value * 1000000;
        if(Objects1.gameObject.activeInHierarchy)
        {
            PhysicsObject1.mass = Moon1Mass.value;
            PhysicsObject1.bouncyness = Moon1Bouncyness.value;
        }
        if(Objects2.gameObject.activeInHierarchy)
        {
            PhysicsObject2.mass = Moon2Mass.value;
            PhysicsObject2.bouncyness = Moon2Bouncyness.value;
        }
        if(Objects3.gameObject.activeInHierarchy)
        {
            PhysicsObject3.mass = Moon3Mass.value;
            PhysicsObject3.bouncyness = Moon3Bouncyness.value;
        }
    }

    public void Spawn1()
    {
        Debug.Log("Moon 1 Spawn");
        Objects1UI.SetActive(true);
        Objects1.SetActive(true);
    }

    public void Spawn2()
    {
        Debug.Log("Moon 2 Spawn");
        Objects2UI.SetActive(true);
        Objects2.SetActive(true);
    }

    public void Spawn3()
    {
        Debug.Log("Moon 3 Spawn");
        Objects3UI.SetActive(true);
        Objects3.SetActive(true);
    }

}
