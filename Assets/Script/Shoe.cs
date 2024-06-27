using UnityEngine;
using System.Collections;

/// <summary>
/// 连跳道具
/// </summary>
public class Shoe : Item {
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Player")
        {
            PlayerController.instance.UseShoe();
        }
    }
}