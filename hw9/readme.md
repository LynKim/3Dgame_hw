@[TOC](UI系统)

# 血条（Health Bar）的预制设计
具体要求如下

* 分别使用 IMGUI 和 UGUI 实现
* 使用 UGUI，血条是游戏对象的一个子元素，任何时候需要面对主摄像机
* 分析两种实现的优缺点
* 给出预制的使用方法

## IMGUI实现
首先由于标准库自身原因，会报错，需要修改
ForcedReset.cs：

```csharp
using UnityEngine.UI;	//追加

//[RequireComponent(typeof (GUITexture))]
[RequireComponent(typeof(Image))]		//改

```

SimpleActivatorMenu.cs:

```csharp
using UnityEngine.UI;		//追加

namespace UnityStandardAssets.Utility
{
    public class SimpleActivatorMenu : MonoBehaviour
    {
        // An incredibly simple menu which, when given references
        // to gameobjects in the scene
        //public GUIText camSwitchButton;
        public Text camSwitchButton;		//改
        public GameObject[] objects;


        private int m_CurrentActiveObject;

```


需要用到OnGUI函数，通过HorizontalScrollbar实现血条:

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthBar{
    [RequireComponent(typeof(Health))]
    public class IMGUI : MonoBehaviour
    {
        private GameObject Character;
        private float curHP;
        private float fullHP;
        private void Start() {
            Character = this.gameObject;
        }

        private void OnGUI() {
            if(GUI.Button(new Rect(30,30,100,50),"失血"))
            {
                Character.GetComponent<Health>().Hurt();
            }
            if(GUI.Button(new Rect(140,30,100,50),"回血"))
            {
                Character.GetComponent<Health>().Recover();
            }

            //Linear interpolation to make HP change smoothly
            curHP = Character.GetComponent<Health>().curHP;
            fullHP = Character.GetComponent<Health>().fullHP;

            GUI.HorizontalScrollbar(new Rect(30,90,170,20), 0.0f, curHP, 0.0f, fullHP);
        }
    }
}
```
运行结果：
![](https://img-blog.csdnimg.cn/20201227220400266.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80Mzg3MDQ1MQ==,size_16,color_FFFFFF,t_70#pic_center)

## UGUI 实现
根据课件，稍微改动即可：
* 菜单 Assets -> Import Package -> Characters 导入资源
* 在层次视图，Context 菜单 -> 3D Object -> Plane 添加 Plane 对象
* 资源视图展开 Standard Assets :: Charactors :: ThirdPersonCharater :: Prefab
* 将 ThirdPersonController 预制拖放放入场景，改名为 Ethan
* 检查以下属性
Plane 的 Transform 的 Position = (0,0,0)
Ethan 的 Transform 的 Position = (0,0,0)
Main Camera 的 Transform 的 Position = (0,1,-10)
* 运行检查效果
* 选择 Ethan 用上下文菜单 -> UI -> Canvas, 添加画布子对象
* 选择 Ethan 的 Canvas，用上下文菜单 -> UI -> Slider 添加滑条作为血条子对象
* 运行检查效果
* 选择 Ethan 的 Canvas，在 Inspector 视图
* 设置 Canvas 组件 Render Mode 为 World Space
* 设置 Rect Transform 组件 (PosX，PosY，Width， Height) 为 (0,2,160,20)
* 设置 Rect Transform 组件 Scale （x,y） 为 (0.01,0.01)
* 运行检查效果，应该是头顶 Slider 的 Ethan，用键盘移动 Ethan，观察
展开 Slider
* 选择 Handle Slider Area，禁灰（disable）该元素
* 选择 Background，禁灰（disable）该元素
* 选择 Fill Area 的 Fill，修改 Image 组件的 Color 为 红色
* 选择 Slider 的 Slider 组件
* 设置 MaxValue 为 100
* 设置 Value 为 75
* 运行检查效果，发现血条随人物旋转

为slider添加脚本：

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HealthBar{
    public class HPChanged : MonoBehaviour
    {
        public GameObject character;
        private void Start() {
            //将当前血条设置为满
            this.gameObject.GetComponent<Slider>().maxValue = character.GetComponent<Health>().fullHP;
        }
        private void Update() {
            //获取并设置当前血量
            this.gameObject.GetComponent<Slider>().value = character.GetComponent<Health>().curHP;
        }
    }
}

```
运行结果：
![](https://img-blog.csdnimg.cn/202012272206580.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80Mzg3MDQ1MQ==,size_16,color_FFFFFF,t_70#pic_center)

## 分析优缺点

IMGUI：
* 优点：
如果不做复杂的界面，代码简单易用
在修改模型，渲染模型这样的经典游戏循环编程模式中，在渲染阶段之后，绘制 UI 界面无可挑剔
避免了 UI 元素保持在屏幕最前端，又有最佳的执行效率，一切控制掌握在程序员手中，这对早期计算和存储资源贫乏的游戏设备来说，更是弥足珍贵。
* 缺点：
传统代码驱动的 UI 开发效率低下，难以调试
IMGUI是其中GUI系统通常不保留有关GUI的信息，而是反复要求您重新指定控件是什么，控件在哪里等的控件。当您以函数调用的形式指定UI的每个部分时，会立即对其进行处理（绘制，单击等），并且任何用户交互的后果都会立即返回给您，而无需您进行查询。这对于游戏UI而言效率低下，由于一切都变得非常依赖于代码，因此对于设计师来说不方便使用。

但是它对于非实时情况，由代码驱动，希望根据当前状态轻松更改显示的控件的处理是非常方便的，也就是说，它是一个很好的编辑面板的选择。

UGUI：

* 优点：

为了让设计师也能参与参与程序开发，从简单的地图编辑器、菜单编辑器等工具应运而生。 设计师甚至不需要程序员帮助，使用这些工具就可直接创造游戏元素，乃至产生游戏程序。
所见即所得（WYSIWYG）设计工具
支持多模式、多摄像机渲染
面向对象的编程
## 预制的使用方法
IMGUI可以直接将脚本挂载到人物对象
UGUI将预制实例化后，添加到人物对象的子对象中
