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
  + **说明：**收费插件需自行在 Unity Asset Store 上购买。

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

拾取操作说明：

按住拾取键，Player 的手会向前伸尝试拾取物体，若拾取到了物体，则拾取结束；否则，最多保持拾取动作 3s，之后有 1s 的冷却时间（无法拾取）。

### 制作可拾取的物品

1. 在场景中导入 3D 模型，添加相应的碰撞体。碰撞体可以是这个 3D 模型的子物体。
2. 为 3D 模型添加 Rigidbody 和 Item Info 组件。
3. 设置这个物体的 breakForce 和 breakTorqu。 
4. 将做好的这个可拾取的物品放在 Assets/03.Environment/Prefabs/Dragable Items 文件夹下。
5. 完成！

### 制作一个新场景

1. 在 Assets/01 Scenes/Dev 里添加一个新 Scene。命名规则：dev_{标号}。
2. 在这个新场景里添加一个新Gameobjec，命名为 InControl，给这个物体添加 InControlManager 脚本。