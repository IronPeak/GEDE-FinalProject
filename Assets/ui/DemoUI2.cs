using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DemoUI2 : MonoBehaviour
{

    public Slider Earth1Mass;
    public Slider Earth2Mass;
    public Slider Earth3Mass;
    public Slider Bouncyness;
    public Slider Friction;

    public GravityObject Earth1;
    public GravityObject Earth2;
    public GravityObject Earth3;

    public List<PhysicsObject> Spheres;

    float oldbouncevalue = 1;
    float oldfrictionvalue = 1;

    void Update()
    {
        Earth1.Mass = Earth1Mass.value * 1000 - 500;
        Earth2.Mass = Earth2Mass.value * 1000 - 500;
        Earth3.Mass = Earth3Mass.value * 1000 - 500;
        if(Bouncyness.value != oldbouncevalue)
        {
            oldbouncevalue = Bouncyness.value;
            foreach(PhysicsObject s in Spheres)
            {
                s.bouncyness = Bouncyness.value;
            }
        }
        if(Friction.value != oldfrictionvalue)
        {
            oldfrictionvalue = Friction.value;
            foreach(PhysicsObject s in Spheres)
            {
                s.friction = Friction.value;
            }
        }
    }

}
