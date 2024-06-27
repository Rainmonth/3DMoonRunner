using UnityEngine;
using System.Collections;

/// <summary>
/// 金币道具
/// </summary>
public class Coin : Item {

    //public override void OnTriggerEnter(Collider other)
    //{
    //    base.OnTriggerEnter(other);
    //    if (other.tag == "Player")
    //    {
    //        GameAttribute.instance.AddCoin(1);
    //    }
    //}

    public override void HitItem()
    {
        base.HitItem();
        GameAttribute.instance.AddCoin(1);
    }

    public override void PlayHitAudio()
    {
        AudioManager.instance.PlayCoinAudio();
    }
}