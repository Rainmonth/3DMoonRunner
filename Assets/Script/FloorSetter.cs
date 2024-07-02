using UnityEngine;
using System.Collections;

/// <summary>
/// 地板设置类，用于在游戏场景中动态设置和更新地板对象。
/// </summary>
public class FloorSetter : MonoBehaviour
{
    /// <summary>
    /// 正在运行时的地板对象，用于显示游戏中的当前地板。
    /// </summary>
    public GameObject floorOnRunning;
    
    /// <summary>
    /// 前进方向的地板对象，用于准备下一个将要显示的地板。
    /// </summary>
    public GameObject floorForward;

    /// <summary>
    /// 地板设置类的单例实例，用于全局访问地板设置功能。
    /// </summary>
    public static FloorSetter instance;

    /// <summary>
    /// 初始化组件，设置单例实例为当前对象。
    /// </summary>
	// Use this for initialization
	void Start () {
        instance = this;
	}

    /// <summary>
    /// 从地板上移除所有物品对象。
    /// </summary>
    /// <param name="floor">要移除物品的地板GameObject。</param>
    void RemoveItem(GameObject floor)
    {
        // 找到地板上 名字为 Item 的 GameObject 对象，
        var item = floor.transform.Find("Item");
        if (item != null)
        {
            Debug.Log("item size:" + item.childCount);
            // 遍历“Item”下的所有子对象并销毁它们。
            foreach (var child in item)
            {
                Transform childTranform = child as Transform;
                if (childTranform != null)
                {
                    Destroy(childTranform.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// 在地板上添加随机物品对象。
    /// </summary>
    /// <param name="floor">要添加物品的地板GameObject。</param>
    void AddItem(GameObject floor)
    {
        // 找到地板上 名字为 Item 的 GameObject 对象，
        var item = floor.transform.Find("Item");
        if (item != null)
        {
            // 获取模式管理器实例。
            var patternManager = PatternManager.instance;
            if (patternManager != null && patternManager.Patterns != null && patternManager.Patterns.Count > 0)
            {
                // 随机选择一个模式。
                var pattern = patternManager.Patterns[Random.Range(0, patternManager.Patterns.Count)];
                // 将 pattern 列表中的 物品添加到 “Item”下。
                if (pattern != null && pattern.PatternItems != null && pattern.PatternItems.Count > 0)
                {
                    // 遍历模式中的每个物品，克隆并放置到地板上。
                    foreach (var patternItem in pattern.PatternItems)
                    {
                        var newObj = Instantiate(patternItem.gameobject);
                        newObj.transform.parent = item;
                        newObj.transform.localPosition = patternItem.position;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 每帧更新一次，检查是否需要更新地板。
    /// </summary>
	// Update is called once per frame
	void Update () {
        // 当当前地板的Z位置比正在运行的地板的Z位置落后32个单位时，更新地板。
        if (transform.position.z > floorOnRunning.transform.position.z + 32)
        {
            // 从当前地板移除所有物品并添加新物品。
            RemoveItem(floorOnRunning);
            AddItem(floorOnRunning);

            // 更新当前地板位置为前进地板位置，并交换当前地板和前进地板。
            floorOnRunning.transform.position = new Vector3(0, 0, floorForward.transform.position.z + 32);
            GameObject temp = floorOnRunning;
            floorOnRunning = floorForward;
            floorForward = temp;
        }
	}
}