# 3D 游戏地图 - 开放世界

这是一个为 Unity 2022.3.62f3c1 创建的小型 3D 开放世界地图项目，风格类似于暗黑破坏神，但采用明亮的色彩风格。

## 项目特性

- 🗺️ **平坦地形** - 带有小路和自然地形起伏
- 💧 **水流系统** - 河流和水体
- 🏠 **建筑群** - 房屋、建筑物
- 🕳️ **洞穴** - 可探索的地下洞穴入口
- 📷 **45度俯视角** - 类似暗黑刷宝的视角
- ☀️ **明亮风格** - 高亮度、清爽的光照

## 目录结构

```
Assets/
├── Scenes/
│   └── GameMap/
│       └── MainMap.unity          # 主游戏场景
├── Scripts/
│   ├── Terrain/
│   │   ├── TerrainGenerator.cs    # 地形生成脚本
│   │   └── TerrainConfig.cs       # 地形配置
│   ├── Player/
│   │   ├── PlayerController.cs    # 玩家控制脚本
│   │   └── CameraController.cs    # 相机控制脚本（45度俯视）
│   └── Environment/
│       ├── BuildingManager.cs     # 建筑管理
│       └── CaveManager.cs         # 洞穴管理
├── Prefabs/
│   ├── Buildings/
│   │   ├── House.prefab
│   │   └── Cave.prefab
│   ├── Environment/
│   │   ├── Tree.prefab
│   │   ├── Rock.prefab
│   │   └── Water.prefab
│   └── Player/
│       └── Player.prefab
├── Materials/
│   ├── Terrain.mat
│   ├── Water.mat
│   ├── Stone.mat
│   └── Wood.mat
└── Textures/
    ├── grass.png
    ├── stone.png
    ├── water.png
    └── wood.png
```

## 快速开始

### 方法1：使用一键设置脚本（推荐）

1. 在 Unity 2022.3.62f3c1 中打开项目
2. 在菜单栏找到 **Game > Setup Scene**
3. 点击自动生成场景
4. 点击 Play 按钮开始游戏

### 方法2：手动设置

1. 在 Unity 中打开项目
2. 打开 `Assets/Scenes/GameMap/MainMap.unity` 场景
3. 点击 Play 按钮开始游戏

## 控制方式

- **WASD** - 移动角色
- **鼠标** - 自动跟随（相机会自动跟随玩家）
- **Space** - 预留（可扩展）

## 系统要求

- Unity 2022.3.62f3c1
- Windows/Mac/Linux
- 最低配置：4GB RAM，显卡支持 DirectX 11

## 脚本说明

### PlayerController.cs
- 处理玩家输入（WASD移动）
- 角色朝向控制
- 重力和碰撞处理

### CameraController.cs
- 45度俯视角实现
- 平滑跟随玩家
- 相机距离调整

### TerrainGenerator.cs
- 程序化生成地形
- 创建路径系统
- 生成水体
- 随机放置装饰物（树、岩石、房屋、洞穴）

### BuildingManager.cs
- 建筑材质管理
- 房屋初始化

### CaveManager.cs
- 洞穴灯光管理
- 洞穴材质设置

## 自定义设置

编辑 `TerrainConfig.cs` 可以调整：
- 地图大小（mapWidth, mapLength）
- 装饰物数量（treeCount, houseCount 等）
- 颜色配置（grassColor, waterColor 等）
- 地形参数（terrainScale, waterLevel 等）

## 扩展功能

你可以轻松添加：
- 敌人和 NPC
- 物品和拾取系统
- UI 界面
- 音效和背景音乐
- 任务系统
- 存档系统

## 许可证

MIT License
