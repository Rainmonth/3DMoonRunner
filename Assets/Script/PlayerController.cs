using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
///  游戏角色控制器类，负责管理角色的动作和状态
/// </summary>
public class PlayerController : MonoBehaviour
{
    // 角色移动速度
    public float speed = 1;
    // 角色初始速度
    public float init_speed = 5;
    // 角色最大速度
    private float maxSpeed = 20;
    // 输入方向
    InputDirection inputDirection;
    // 鼠标位置
    Vector3 mousePos;
    // 输入是否激活
    bool activeInput;
    // 站立位置
    Position standPosition;
    // 来自的位置
    Position fromPosition;
    // x轴移动方向
    Vector3 xDirection;
    // 移动方向
    Vector3 moveDirection;
    // 角色控制器组件
    CharacterController characterController;
    // 是否滚动
    public bool isRoll = false;
    // 跳跃加速度
    float jumpValue = 7;
    // 重力加速度
    float gravity = 20;
    // 是否允许双跳
    public bool canDoubleJump = true;
    // 是否已经双跳
    bool doubleJump = false;
    // 是否快速移动
    bool isQuickMoving = false;
    // 保存的移动速度
    float saveSpeed;
    // 快速移动持续时间
    float quickMoveDuration = 3;
    // 快速移动剩余时间
    public float quickMoveTimeLeft;
    // 快速移动的Coroutine
    IEnumerator quickMoveCor;

    // 磁铁效果持续时间
    float magnetDuration = 15;
    // 磁铁效果剩余时间
    public float magnetTimeLeft;
    // 磁铁效果的Coroutine
    IEnumerator magnetCor;
    // 磁铁碰撞器游戏对象
    public GameObject MagnetCollider;

    // 鞋子效果持续时间
    float shoeDuration = 10;
    // 鞋子效果剩余时间
    public float shoeTimeLeft;
    // 鞋子效果的Coroutine
    IEnumerator shoeCor;
    // 多倍金币效果持续时间
    float multiplyDuration = 10;
    // 多倍金币效果剩余时间
    public float multiplyTimeLeft;
    // 定义一个IEnumerator类型的变量，用于存储乘数效果的协程
    IEnumerator multiplyCor;

    // 注释掉的Text对象，原用于显示状态信息
    //public Text statusText;
    // 公有Text对象，用于显示磁铁相关的文本信息
    public Text Text_Magnet;
    // 公有Text对象，用于显示鞋子相关的文本信息
    public Text Text_Shoe;
    // 公有Text对象，用于显示星星相关的文本信息
    public Text Text_Star;
    // 公有Text对象，用于显示乘数相关的文本信息
    public Text Text_Multiply;

    // 公有GameObject对象，用于道路1的显示控制
    public GameObject road1;
    // 公有GameObject对象，用于道路2的显示控制
    public GameObject road2;
    // 公有GameObject对象，用于起点1的显示控制
    public GameObject start1;
    // 公有GameObject对象，用于起点2的显示控制
    public GameObject start2;

    // 私有浮点型变量，用于控制速度增加的距离
    private float speedAddDistance = 300;
    // 私有浮点型变量，用于控制速度增加的速率
    private float speedAddRate = 0.5f;
    // 私有浮点型变量，用于记录速度增加的次数
    private float speedAddCount = 0;

    // 私有Animation对象，用于动画控制
    private Animation animation;

    // 静态的PlayerController实例，用于确保整个游戏只存在一个PlayerController实例
    public static PlayerController instance;

    // Use this for initialization
    void Start()
    {
        instance = this;
        speed = init_speed;
        animation = GetComponent<Animation>();
        characterController = GetComponent<CharacterController>();
        standPosition = Position.Middle;
        StartCoroutine(UpdateAction());
    }

    public void Play()
    {
        GameController.instance.isPause = false;
        GameController.instance.isPlay = true;
        StartCoroutine(UpdateAction());
    }

    private void SetSpeed(float newSpeed)
    {
        if (newSpeed <= maxSpeed)
            speed = newSpeed;
        else
            speed = maxSpeed;
    }

    IEnumerator UpdateAction()
    {
        while (GameAttribute.instance.life > 0)
        {
            if (GameController.instance.isPlay && !GameController.instance.isPause)
            {
                GetInputDirection();
                MoveLeftRight();
                MoveForward();
                UpdateSpeed();
            }
            else
            {
                animation.Stop();
            }
            yield return 0;
        }
        //Debug.Log("game over");
        speed = 0;
        GameController.instance.isPlay = false;
        xDirection = Vector3.zero;
        AnimationManager.instance.animationHandler = AnimationManager.instance.PlayDead;
        iTween.MoveTo(gameObject, gameObject.transform.position - new Vector3(0, 0, 2), 2.0f);
        yield return new WaitForSeconds(3);
        Debug.Log("restart");

        UIController.instance.ShowRestartUI();
        UIController.instance.HidePauseUI();
    }

    private void UpdateSpeed()
    {
        speedAddCount += speed * Time.deltaTime;
        if (speedAddCount >= speedAddDistance)
        {
            SetSpeed(speed + speedAddRate);
            speedAddCount = 0;
        }
    }

    void MoveForward()
    {
        if (inputDirection == InputDirection.Down)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayRoll;
        }
        if (characterController.isGrounded)
        {
            moveDirection = Vector3.zero;
            if (AnimationManager.instance.animationHandler != AnimationManager.instance.PlayRoll &&
                AnimationManager.instance.animationHandler != AnimationManager.instance.PlayTurnLeft &&
                AnimationManager.instance.animationHandler != AnimationManager.instance.PlayTurnRight)
            {
                AnimationManager.instance.animationHandler = AnimationManager.instance.PlayRun;
            }
            if (inputDirection == InputDirection.Up)
            {
                JumpUp();
                if (canDoubleJump)
                    doubleJump = true;
            }
        }
        else
        {
            if (inputDirection == InputDirection.Down)
            {
                QuickGround();
            }
            if (inputDirection == InputDirection.Up)
            {
                if (doubleJump)
                {
                    JumpDouble();
                    doubleJump = false;
                }
            }

            if (AnimationManager.instance.animationHandler != AnimationManager.instance.PlayJumpUp
                && AnimationManager.instance.animationHandler != AnimationManager.instance.PlayRoll
                && AnimationManager.instance.animationHandler != AnimationManager.instance.PlayDoubleJump)
            {
                AnimationManager.instance.animationHandler = AnimationManager.instance.PlayJumpLoop;
            }
        }

        moveDirection.z = speed;
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move((xDirection * 10 + moveDirection) * Time.deltaTime);
    }

    public void Reset()
    {
        speed = init_speed;
        inputDirection = InputDirection.NULL;
        activeInput = false;
        standPosition = Position.Middle;
        xDirection = Vector3.zero;
        moveDirection = Vector3.zero;
        isRoll = false;
        canDoubleJump = false;
        isQuickMoving = false;
        quickMoveTimeLeft = 0;
        magnetTimeLeft = 0;
        shoeTimeLeft = 0;
        multiplyTimeLeft = 0;
        speedAddCount = 0;

        gameObject.transform.position = new Vector3(0, 0, -64);
        Camera.main.transform.position = new Vector3(0, 5, -67);

        AnimationManager.instance.animationHandler = AnimationManager.instance.PlayRun;

        var newRoad1 = Respawn("road1", road1, new Vector3(0, 0, 0));
        var newRoad2 = Respawn("road2", road2, new Vector3(0, 0, 32));
        Respawn("start1", start1, new Vector3(0, 0, -32));
        Respawn("start2", start2, new Vector3(0, 0, -64));

        FloorSetter.instance.floorOnRunning = newRoad1;
        FloorSetter.instance.floorForward = newRoad2;
    }

    private GameObject Respawn(string name, GameObject prefab, Vector3 location)
    {
        var old = GameObject.Find(name);
        if (old != null)
        {
            Destroy(old);
            var newObj = Instantiate(prefab);
            newObj.name = name;
            newObj.transform.localPosition = location;
            return newObj;
        }
        return null;
    }

    void QuickGround()
    {
        moveDirection.y -= jumpValue * 3;
    }

    void JumpDouble()
    {
        AnimationManager.instance.animationHandler = AnimationManager.instance.PlayDoubleJump;
        moveDirection.y += jumpValue * 1.3f;
    }

    void JumpUp()
    {
        AnimationManager.instance.animationHandler = AnimationManager.instance.PlayJumpUp;
        moveDirection.y += jumpValue;
    }

    void MoveLeft()
    {
        if (standPosition != Position.Left)
        {
            GetComponent<Animation>().Stop();
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayTurnLeft;

            xDirection = Vector3.left;

            if (standPosition == Position.Middle)
            {
                standPosition = Position.Left;
                fromPosition = Position.Middle;
            }
            else if (standPosition == Position.Right)
            {
                standPosition = Position.Middle;
                fromPosition = Position.Right;
            }
        }
    }

    void MoveRight()
    {
        if (standPosition != Position.Right)
        {
            GetComponent<Animation>().Stop();
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayTurnRight;

            xDirection = Vector3.right;

            if (standPosition == Position.Middle)
            {
                standPosition = Position.Right;
                fromPosition = Position.Middle;
            }
            else if (standPosition == Position.Left)
            {
                standPosition = Position.Middle;
                fromPosition = Position.Left;
            }
        }
    }

    void MoveLeftRight()
    {
        if (inputDirection == InputDirection.Left)
        {
            MoveLeft();
        }
        else if (inputDirection == InputDirection.Right)
        {
            MoveRight();
        }

        if (standPosition == Position.Left)
        {
            if (transform.position.x <= -1.7f)
            {
                xDirection = Vector3.zero;
                transform.position = new Vector3(-1.7f, transform.position.y, transform.position.z);
            }
        }
        if (standPosition == Position.Middle)
        {
            if (fromPosition == Position.Left)
            {
                if (transform.position.x > 0)
                {
                    xDirection = Vector3.zero;
                    transform.position = new Vector3(0, transform.position.y, transform.position.z);
                }
            }
            else if (fromPosition == Position.Right)
            {
                if (transform.position.x < 0)
                {
                    xDirection = Vector3.zero;
                    transform.position = new Vector3(0, transform.position.y, transform.position.z);
                }
            }
        }

        if (standPosition == Position.Right)
        {
            if (transform.position.x >= 1.7f)
            {
                xDirection = Vector3.zero;
                transform.position = new Vector3(1.7f, transform.position.y, transform.position.z);
            }
        }
    }

    void PlayAnimation()
    {
        if (inputDirection == InputDirection.Left)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayTurnLeft;
        }
        else if (inputDirection == InputDirection.Right)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayTurnRight;
        }
        else if (inputDirection == InputDirection.Up)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayJumpUp;
        }
        else if (inputDirection == InputDirection.Down)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayRoll;
        }
    }

    public void QuickMove()
    {
        if (quickMoveCor != null)
            StopCoroutine(quickMoveCor);
        quickMoveCor = QuickMoveCoroutine();
        StartCoroutine(quickMoveCor);
    }

    public void UseMagnet()
    {
        if (magnetCor != null)
            StopCoroutine(magnetCor);
        magnetCor = MagnetCoroutine();
        StartCoroutine(magnetCor);
    }

    public void UseShoe()
    {
        if (shoeCor != null)
            StopCoroutine(shoeCor);
        shoeCor = ShoeCoroutine();
        StartCoroutine(shoeCor);
    }

    public void Multiply()
    {
        if (multiplyCor != null)
            StopCoroutine(multiplyCor);
        multiplyCor = MultiplyCoroutine();
        StartCoroutine(multiplyCor);
    }

    private bool CanPlay()
    {
        return !GameController.instance.isPause && GameController.instance.isPlay;
    }

    IEnumerator MultiplyCoroutine()
    {
        multiplyTimeLeft = multiplyDuration;
        GameAttribute.instance.multiply = 2;
        while (multiplyTimeLeft >= 0)
        {
            if (CanPlay())
                multiplyTimeLeft -= Time.deltaTime;
            yield return null;
        }
        GameAttribute.instance.multiply = 1;
    }

    IEnumerator ShoeCoroutine()
    {
        shoeTimeLeft = shoeDuration;
        PlayerController.instance.canDoubleJump = true;
        while (shoeTimeLeft >= 0)
        {
            if (CanPlay())
                shoeTimeLeft -= Time.deltaTime;
            yield return null;
        }
        PlayerController.instance.canDoubleJump = false;
    }

    IEnumerator MagnetCoroutine()
    {
        magnetTimeLeft = magnetDuration;
        MagnetCollider.SetActive(true);
        while (magnetTimeLeft >= 0)
        {
            if (CanPlay())
                magnetTimeLeft -= Time.deltaTime;
            yield return null;
        }
        MagnetCollider.SetActive(false);
    }

    IEnumerator QuickMoveCoroutine()
    {
        quickMoveTimeLeft = quickMoveDuration;
        if (!isQuickMoving)
            saveSpeed = speed;
        speed = 20;
        isQuickMoving = true;
        //yield return new WaitForSeconds(quickMoveDuration);
        while (quickMoveTimeLeft >= 0)
        {
            if (CanPlay())
                quickMoveTimeLeft -= Time.deltaTime;
            yield return null;
        }
        speed = saveSpeed;
        isQuickMoving = false;
    }

    void GetInputDirection()
    {
        inputDirection = InputDirection.NULL;
        if (Input.GetMouseButtonDown(0))
        {
            activeInput = true;
            mousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0) && activeInput)
        {
            Vector3 vec = Input.mousePosition - mousePos;
            if (vec.magnitude > 20)
            {
                var angleY = Mathf.Acos(Vector3.Dot(vec.normalized, Vector2.up)) * Mathf.Rad2Deg;
                var anglex = Mathf.Acos(Vector3.Dot(vec.normalized, Vector2.right)) * Mathf.Rad2Deg;
                if (angleY <= 45)
                {
                    inputDirection = InputDirection.Up;
                    AudioManager.instance.PlaySlideAudio();
                }
                else if (angleY >= 135)
                {
                    inputDirection = InputDirection.Down;
                    AudioManager.instance.PlaySlideAudio();
                }
                else if (anglex <= 45)
                {
                    inputDirection = InputDirection.Right;
                    AudioManager.instance.PlaySlideAudio();
                }
                else if (anglex >= 135)
                {
                    inputDirection = InputDirection.Left;
                    AudioManager.instance.PlaySlideAudio();
                }
                activeInput = false;
                //Debug.Log(inputDirection);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));


        //statusText.text = GetTime(multiplyTimeLeft);
        UpdateItemTime();
    }

    private void UpdateItemTime()
    {
        Text_Magnet.text = GetTime(magnetTimeLeft);
        Text_Multiply.text = GetTime(multiplyTimeLeft);
        Text_Shoe.text = GetTime(shoeTimeLeft);
        Text_Star.text = GetTime(quickMoveTimeLeft);
    }

    private string GetTime(float time)
    {
        if (time <= 0)
            return "";
        //return Mathf.RoundToInt(time).ToString();
        return ((int)time + 1).ToString() + "s";
    }
}

public enum InputDirection
{
    NULL,
    Left,
    Right,
    Up,
    Down
}

public enum Position
{
    Left,
    Middle,
    Right
}