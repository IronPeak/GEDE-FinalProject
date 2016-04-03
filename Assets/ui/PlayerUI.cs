using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    public Gun gun;

    public Slider Velocity;
    public Slider Gravity;
    public Slider Bouncyness;
    public Slider Friction;

    public void Awake()
    {
        Velocity.value = gun.velocity / 20;
        Gravity.value = 1;
        Bouncyness.value = gun.bouncyness;
        Friction.value = gun.friction;
    }

    public void Update()
    {
        gun.velocity = Velocity.value * 20;
        gun.gravity.y = -(Gravity.value * 20 - 10);
        gun.bouncyness = Bouncyness.value;
        gun.friction = Friction.value;
    }

}
