# 🐋 DeepSeek Harness Launcher（鲸鱼少女版）

一个 Windows 一键启动器：**双击图标 → 自动启动 DeepSeek Harness Web 服务 → 自动打开浏览器进入操作界面**。附赠鲸鱼少女主题图标。

> DeepSeek Harness 的 Web 界面默认运行在 <http://127.0.0.1:3080>。

## ✨ 特性
图片<img width="191" height="215" alt="241a90e4e46322364a6545c3214889c8" src="https://github.com/user-attachments/assets/82650ba5-37a6-4a8e-b550-cc4e0e2c1d57" />
<img width="910" height="941" alt="original-whale-girl" src="https://github.com/user-attachments/assets/0c0b462f-8341-4751-86df-857442eb8908" />

<img width="512" height="512" alt="DeepSeekHarness-WhaleGirl-preview" src="https://github.com/user-attachments/assets/1e54f5d2-2faf-46b0-b6a0-b5da4e14f545" />


- **一键直达**：双击即启动服务，并自动打开浏览器进入 UI，无需手动敲命令、无需自己开浏览器
- **智能检测**：服务已在运行时不会重复启动，直接打开界面
- **优雅停止**：关闭服务控制台窗口即停止（服务窗口标题有明确提示）
- **多尺寸图标**：`.ico` 内置 16 / 24 / 32 / 48 / 64 / 128 / 256 共 7 种尺寸，任何缩放都清晰
- **透明背景**：图标已去除原图白色背景，边缘柔和羽化，适配深浅主题的任务栏

## 📁 文件说明

| 文件 | 说明 |
|---|---|
| `start-dsh.cmd` | 智能启动器（核心脚本） |
| `DeepSeekHarness-WhaleGirl.ico` | 鲸鱼少女图标（7 种尺寸，PNG 内嵌格式） |
| `DeepSeekHarness-WhaleGirl-preview.png` | 图标效果预览图 |

## 🖥️ 环境要求

- Windows 10 / 11
- Node.js（DeepSeek Harness 运行依赖）
- 已通过 `npx @deepseek-ai/dsh` 或全局安装方式安装过 DeepSeek Harness

## 🚀 使用方法

### 1. 配置路径（重要）

`start-dsh.cmd` 顶部的三行路径需要改成**你自己机器上**的实际情况：

```bat
set "NODE=D:\node.js\node.exe"                          ← 你的 node.exe 路径（cmd 里输入 where node 可查）
set "BIN=C:\Users\<你的用户名>\AppData\Local\npm-cache\_npx\<缓存目录>\node_modules\@deepseek-ai\dsh\lib\bin.js"
set "URL=http://127.0.0.1:3080"                          ← DSH Web 界面地址，一般不用改
```

> 找不到 `bin.js`？在命令行执行 `npx @deepseek-ai/dsh web --dump-config` 会输出实际使用的目录，或者直接搜索 `@deepseek-ai\dsh\lib\bin.js`。

### 2. 启动

- 直接双击 `start-dsh.cmd`；或
- 创建桌面快捷方式（见下）后双击快捷方式

启动流程：
1. 检测端口 `3080` 是否已被占用（即服务是否已在运行）
2. 未运行 → 弹出服务控制台窗口并启动服务
3. 等待服务就绪（最多 30 秒）→ 自动打开默认浏览器进入 UI

### 3. 创建桌面快捷方式（可选）

1. 右键 `start-dsh.cmd` → **创建快捷方式**
2. 把生成的快捷方式改名为 `DeepSeek Harness` 并移动到桌面
3. 右键快捷方式 → **属性** → **更改图标** → 选择 `DeepSeekHarness-WhaleGirl.ico`

### 4. 固定到任务栏（Windows 11 注意）

**快捷方式的目标必须是 exe 才会显示「固定到任务栏」选项**。如果目标直接是 `.cmd` 文件，右键菜单里没有固定选项。

推荐做法：把快捷方式的目标改为 `cmd.exe`：

```
目标:  C:\Windows\System32\cmd.exe
参数:  /d /c "C:\path\to\start-dsh.cmd"
```

然后右键快捷方式 → **固定到任务栏** 即可。之后桌面快捷方式可以删除，不影响任务栏图标。

> Windows 11 较新版本（24H2+）出于安全限制，**不支持程序化固定任务栏**（COM 动词、注册表修改均不可靠），手动右键固定是唯一稳妥方式。

### 5. 停止服务

关闭标题为 `DeepSeek Harness Server - close this window to stop` 的控制台窗口，服务即停止。

## ❓ 常见问题

**Q：双击后浏览器没自动打开？**
A：等 1~2 秒手动打开 <http://127.0.0.1:3080>。脚本最多等待 30 秒，超时会提示并照常打开浏览器。

**Q：端口 3080 被其他程序占用？**
A：`start-dsh.cmd` 检测到端口被占用会直接打开浏览器（可能不是 DSH 的服务）。请确认占用者后处理，或修改 `网站` 与 DSH 实际端口一致。

**Q：任务栏图标是空白/默认图标？**
A：确认快捷方式「更改图标」选择了本项目的 `.ico`，且文件没有被移动。

## 📄 许可

本项目按 **MIT 许可证** 发布，详情见 [LICENSE](LICENSE)。

图标素材（鲸鱼少女）版权归原作者所有；如用于公开分发，请确认你有权使用该素材。
