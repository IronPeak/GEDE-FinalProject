using UnityEngine;
using System.Collections.Generic;

public class GravityObject : MonoBehaviour
{

    public static List<GravityObject> Objects = new List<GravityObject>();

    public float Mass = 10;

	void Awake()
    {
        Objects.Add(this);
	}
	
}
