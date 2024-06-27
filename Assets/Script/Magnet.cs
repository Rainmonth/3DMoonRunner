using UnityEngine;
using System.Collections;

/// <summary>
/// 磁铁道具
/// <para></para>
/// 使用时，激活绑定在角色控制器上的 MagnetCollider Object，这个对象 上有MagnetCollider 脚本，
/// 实现了 OnTriggerEnter 方法，当角色碰撞到磁铁道具时，会激活角色控制器上的 MagnetCollider Object，这
/// 个对象上又实现了 OnTriggerEnter 方法，里面会检测 对象一定范围内是否存在金币，如果存在，则获取金币，增加积分
/// </summary>
public class Magnet : Item {
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Player")
        {
            PlayerController.instance.UseMagnet();
        }
    }
}
