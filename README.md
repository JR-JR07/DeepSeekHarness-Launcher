# 🐋 DeepSeek Harness Launcher（鲸鱼少女版）

一个 Windows 一键启动器：**双击图标 → 自动启动 DeepSeek Harness Web 服务 → 自动打开浏览器进入操作界面**。附赠鲸鱼少女主题图标。

> DeepSeek Harness 的 Web 界面默认运行在 <http://127.0.0.1:3080>。

## 🖼️ 预览

<img width="191" height="215" alt="图标" src="https://github.com/user-attachments/assets/82650ba5-37a6-4a8e-b550-cc4e0e2c1d57" />
<img width="910" height="941" alt="original-whale-girl" src="https://github.com/user-attachments/assets/0c0b462f-8341-4751-86df-857442eb8908" />
<img width="512" height="512" alt="DeepSeekHarness-WhaleGirl-preview" src="https://github.com/user-attachments/assets/1e54f5d2-2faf-46b0-b6a0-b5da4e14f545" />

## ⬇️ 下载与安装（推荐）

1. 下载 **`DeepSeekHarness-Launcher-Setup.exe`**（仓库根目录）
2. 双击运行，按提示输入 `Y` 确认安装
3. 安装程序会自动：
   - 安装到 `%LOCALAPPDATA%\DeepSeekHarness-Launcher`
   - 在桌面创建「DeepSeek Harness」快捷方式（鲸鱼图标）
   - 启动服务并自动打开浏览器进入 UI

以后每次使用：双击桌面的「DeepSeek Harness」快捷方式（或再次运行 EXE）即可一键进入界面。

**前置要求**：Windows 10/11 + [Node.js](https://nodejs.org) + 已安装 DeepSeek Harness（`npx @deepseek-ai/dsh web` 运行过即可）。启动器会自动检测它们的位置，无需手动配置路径。

## ✨ 特性

- **一键直达**：自动启动服务并打开浏览器进入 UI，无需敲命令
- **智能检测**：自动定位 Node.js 与 DeepSeek Harness（npx 缓存/全局安装）；服务已在运行时不会重复启动
- **优雅停止**：关闭服务控制台窗口即停止（窗口标题有明确提示）
- **单文件安装**：EXE 内置图标与启动器，安装即用
- **多尺寸图标**：`.ico` 内置 16 / 24 / 32 / 48 / 64 / 128 / 256 共 7 种尺寸
- **透明背景**：图标已去除白色背景，边缘柔和羽化，适配深浅主题

## 📁 文件说明

| 文件 | 说明 |
|---|---|
| `DeepSeekHarness-Launcher-Setup.exe` | **一键安装版**（推荐下载这个） |
| `start-dsh.cmd` | 通用启动器（免安装的便携版，可配合快捷方式使用） |
| `DeepSeekHarness-WhaleGirl.ico` | 鲸鱼少女图标（7 种尺寸） |
| `DeepSeekHarness-WhaleGirl-preview.png` | 图标效果预览图 |
| `original-whale-girl.jpg` | 原图素材 |
| `src/Setup.cs` | 安装器源码（C#，.NET Framework） |
| `src/launcher-template.cmd` | 启动器模板（EXE 内嵌） |
| `src/build.cmd` | 构建脚本（编译生成 EXE） |

## 🚀 手动使用（便携版）

### 1. 直接运行

双击 `start-dsh.cmd`，启动器会自动检测 Node.js 与 DeepSeek Harness 并完成「启动服务 → 打开浏览器」。

> 如果 Node.js 或 DeepSeek Harness 安装在非常规位置导致检测失败，可编辑脚本顶部的 `NODE` / `BIN` 变量手动指定路径。

### 2. 创建桌面快捷方式（可选）

1. 右键 `start-dsh.cmd` → **创建快捷方式** → 移动并重命名为 `DeepSeek Harness`
2. 右键快捷方式 → **属性** → **更改图标** → 选择 `DeepSeekHarness-WhaleGirl.ico`
3. 固定到任务栏：**快捷方式的目标必须是 exe 才会显示「固定到任务栏」**。把「目标」改为 `C:\Windows\System32\cmd.exe`，「参数」填 `/d /c "start-dsh.cmd 的完整路径"`，再右键 → 固定到任务栏

> Windows 11 24H2+ 出于安全限制不支持程序化固定任务栏，手动右键固定是唯一稳妥方式。

### 3. 停止服务

关闭标题为 `DeepSeek Harness Server - close this window to stop` 的控制台窗口。

## 🔧 从源码构建 EXE

需要 Windows 自带的 .NET Framework（Win10/11 均内置，无需额外安装）：

```
cd src
build.cmd
```

输出：仓库根目录下的 `DeepSeekHarness-Launcher-Setup.exe`。

## ❓ 常见问题

**Q：双击后浏览器没自动打开？**
A：等 1~2 秒手动打开 <http://127.0.0.1:3080>。脚本最多等待 30 秒，超时会提示并照常打开浏览器。

**Q：提示 DeepSeek Harness was not found？**
A：先执行一次 `npx @deepseek-ai/dsh web` 完成安装，再运行启动器。

**Q：端口 3080 被其他程序占用？**
A：启动器检测到端口被占用会直接打开浏览器（可能不是 DSH 的服务）。请确认占用者后处理，或修改 `URL` 与 DSH 实际端口一致。

**Q：任务栏图标是空白/默认图标？**
A：确认快捷方式「更改图标」选择了本项目的 `.ico`，且文件没有被移动。

## 📄 许可

本项目按 **MIT 许可证** 发布，详情见 [LICENSE](LICENSE)。

图标素材（鲸鱼少女）版权归原作者所有；如用于公开分发，请确认你有权使用该素材。
