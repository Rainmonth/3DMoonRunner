using UnityEngine;
using System.Collections;

/// <summary>
/// 道具基类，道具通用属性有：
/// 1. 旋转速度
/// 2. 碰撞效果
/// 3. 与玩家碰撞后，使用相应的道具效果
/// </summary>
public class Item : MonoBehaviour {

    public float rotateSpeed = 1;
    public GameObject hitEffect;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    /// <summary>
    /// 道具碰撞执行方法
    /// </summary>
    public virtual void HitItem()
    {
        PlayHitAudio();
        GameObject effect = Instantiate(hitEffect);
        effect.transform.parent = PlayerController.instance.gameObject.transform;
        effect.transform.localPosition = new Vector3(0, 0.5f, 0);

        Destroy(gameObject);
    }

    /// <summary>
    /// 播放道具被碰撞的音效
    /// </summary>
    public virtual void PlayHitAudio()
    {
        AudioManager.instance.PlayGetItemAudio();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("Item OnTriggerEnter, tag:" + other.tag);
        if (other.tag == "Player")
        {
            HitItem();
        }
    }
}
