using UnityEngine;
using UnityEngine.UI;

public class DemoUI2 : MonoBehaviour
{

    public Slider Earth1Mass;
    public Slider Earth2Mass;
    public Slider Earth3Mass;

    public GravityObject Earth1;
    public GravityObject Earth2;
    public GravityObject Earth3;

    void Update()
    {
        Earth1.Mass = Earth1Mass.value * 1000 - 500;
        Earth2.Mass = Earth2Mass.value * 1000 - 500;
        Earth3.Mass = Earth3Mass.value * 1000 - 500;
    }

}
