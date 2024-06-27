using UnityEngine;
using System.Collections;

/// <summary>
/// 冲刺道具
/// </summary>
public class Star : Item {
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Player")
        {
            PlayerController.instance.QuickMove();
        }
    }
}