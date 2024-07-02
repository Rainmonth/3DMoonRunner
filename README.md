# 3DMoonRunner
月光跑酷3D版  

## 开发环境  
* Unity2021.3.3f1 + C# + Visual Studio Code 1.90.2

## 游戏场景的搭建
这是一个竖屏的跑酷游戏，跑酷游戏，很关键的一点就是跑酷背景的生成。关于背景的生成，有一个很通用的做法就是采用两段背景进行轮换。

## 游戏UI
1. 手机的金币数量显示；
2. 手机静音设置；
3. 道具使用状态限制；
4. 游戏状态；


## 游戏操作
角色前进的道路被分为三部分：左中右，通过，左右滑动，可以进行左右移动。
通过鼠标前后左右滑动，可以产生以下四种操作命令：
- 左滑，角色会切换到左边相邻的位置（如果可以的话）
- 右滑，角色会切换到右边相邻的位置（如果可以的话）
- 上滑，角色会进行跳跃动作
    - 如果开启了连跳，再次上滑会继续跳跃
- 下滑，角色会滑动前进（类似于足球的铲球动作） 


## 游戏道具
### 普通物品
可以获得金币。
### 磁铁
可以吸收一定范围内的金币，
### 连跳
获取连跳道具，可以在跳跃的时候再次进行跳跃；
### 双倍金币
获取2倍金币道具，可以获得2倍金币
### 冲刺
获取冲刺道具，角色移动速度会增加


## 实现过程中遇到的问题
1. 背景的自动生成，背景没有按照预想的进行自动生成；
> 解决方案：为Player添加FloorSetter 脚本，指定FloorOnRunning 和 FloorForward 即可
2. 角色没有播放相应的动画，需要弄清楚一下问题：
    - Animation 动画的使用；
    - Animation 和 Animator 的区别；


> 两个FBX文件，一个有模型，一个有动画，如果需要采用动画，需要将将两个FBX文件同时拖入场景中。

3. 道具的碰撞没有产生预期的效果；
    - 搞清楚为什么 OnTriggerEnter 没有生效；(问题原因：用来进行碰撞比较的Player 的tag 没有正确设置)

4. 完成游戏UI的实现

5. 角色动画播放异常，不流畅


## 游戏预览  
![](./Previews/previews1.png)  
![](./Previews/previews2.png)  
![](./Previews/previews3.png)  

## 下载地址  
* [Android版](https://github.com/XINCGer/3DMoonRunner/releases/download/0.0.1Beta/Android.0.0.1Beta.apk)  
* [Windows版](https://github.com/XINCGer/3DMoonRunner/releases/download/0.0.1Beta/Window.0.0.1Beta.rar)  
