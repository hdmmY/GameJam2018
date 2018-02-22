# GameJam2018

一个基于物理的抢球游戏。

### 文件夹说明

+ "01.Scene" :
  + Dev 文件夹中存放当前正在开发的场景。Legency 文件夹中存放的是 GGJ 时构建的场景。

+ "03.Environment" :
  + Models/Kenney Assets/ 中存放的是可供使用的 3D 模型，用于构建场景。
  + 其中每个子文件夹中，均有一个 "Preview" 和 "Sample" 图片，用于预览该文件所包含的所有 3D 模型。如 "Assets/03.Enviroment/Models/Kenney Assets/Castle Kit/Preview"。
  + 所有 3D 模型来自于免费的 3D 模型包 : http://kenney.nl/。

+ "04.Characters/Prefabs" :
  + ECS 文件夹中存放尝试用 ECS 模式开发的人物控制器。
  + Legency 文件夹中存放 GGJ 时开发的人物控制器。
  + Player 为最新的，用 Unity Component-System 模式开发的人物控制器。这个控制器现在很稳定，应使用这个控制器。

+ "04.Characters/Script" :
  + PlayerCore ECS 文件夹中存放的是用 ECS 模式开发的人物控制器脚本。
  + PlayerCore 文件夹中存放的是用一般的 Unity 组件模式开发的人物控制器脚本。
  + Legency 文件夹中存放的是 GGJ 时开发的所有逻辑脚本（包括人物控制器）。

+ "Test" :
    测试新功能的脚本 + 场景

+ "Plugins"
  + InControl : 一个用于处理输入的插件。
  + PostProcessing : Unity官方的后处理插件。 
  + Sirenix/Odin Inspector : 一个自定义编辑器 + 序列化插件。
  + TextMeshPro : 一个增强 Unity Text 等 UI 的插件。
  + Trinary Software/MoT : 一个类似 ITween 的自定义运动轨迹的插件。

### 人物控制器使用说明

Player Prefab 存放地址 : Assets/04.Characters/Prefabs/Player

通过 Player Prefab 中的 *PlayerInput* 脚本中的 *Use Key Board* 来选择是否使用鼠标或手柄操纵。

键盘：

+ WASD 控制移动。
+ 空格键 跳跃。
+ 鼠标右键点击拾取物品，鼠标左键点击丢弃物品

手柄(以 XBOX 为例，PS4 也差不多)：

+ 左摇杆控制移动。
+ A 键跳跃。
+ X 键拾取物品，B 键丢弃物品。

### 制作可拾取的物品

1. 在场景中导入 3D 模型，添加相应的碰撞体。碰撞体可以是这个 3D 模型的子物体。
2. 为 3D 模型添加 Rigidbody 和 Item Info 组件。
3. 将这个 3D 模型作为 Player/Rigidbodies/Torso 的子物体，调整它的位置到你认为应该抓取时物体的摆放位置。
4. 记录这个 3D 模型当前的 Transform.position，将其 Item Info 组件的 GrabPosOffset 属性设置刚刚记录下来的坐标的负值。
5. 将这个 3D 模型从 Player/Rigidbodies/Torso 的子物体放回至世界坐标的子物体，并放在任何你想要的放置的位置。
6. 将做好的这个可拾取的物品放在 Assets/03.Environment/Prefabs/Dragable Items 文件夹下。
7. 完成！