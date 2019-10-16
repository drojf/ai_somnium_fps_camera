# AI: The Somnium Files FPS Camera Mod

This mod adds a "Noclip Mode" FPS style camera to most parts of the game, allowing you to explore ADV mode scenes and Somniums.

It also allows you to slow down the game by 10x/100x, which may be useful when taking screenshots.

See this video for an example: https://www.youtube.com/watch?v=6zuE3RdaCFU

I don't recommend your first playthrough have this mod enabled, as various things may break unexpectedly.

I also don't want to disappoint anyone - while I haven't thoroughly inspected all of the levels, the game appears to be fairly "clean" and I haven't found anything out of the ordinary. But if you just want to take some nice screenshots this mod might be useful.

## Installation

- Go to the [Releases Page](https://github.com/drojf/ai_somnium_freecam/releases)
- Download the latest modded `Assembly-CSharp.dll`
- Open the `Managed` folder of your game, located at: `Steam\steamapps\common\AI The Somnium Files\AI_TheSomniumFiles_Data\Managed`
  - You can also right click on the game in steam, click "properties", click the "local files" tab, then click "Browse Local Files..." to open the game folder. Then navigate to `\AI_TheSomniumFiles_Data\Managed`
- Replace your `Assembly-CSharp.dll` with the downloaded one (BUT, remember to keep a backup of your original `.dll` file!)

## How to Use the Mod / Controls

**Please note that this mod only works with mouse and keyboard** (... and is PC only)

### Basic Controls

| Key | Action |
| --- | --- |
| `F8` | Enable Noclip/FPS Mode |
| `F9` | Disable Noclip/FPS Mode |
| `P` | Move Forward (Hold `SHIFT` to move faster) |
| `;` | Move Backward (Hold `SHIFT` to move faster) |
| `L` | Strafe Left (Hold `SHIFT` to move faster) |
| `'` | Strafe Right (Hold `SHIFT` to move faster) |
| `SHIFT` | While held, you move faster ("Sprint" key) |
| Mouse Rotation | Rotate the camera (while in FPS mode) |

### Extra Controls

| Key | Action |
| --- | --- |
| `F2` | Disable Slow Motion (revert to normal speed) |
| `F3` | 10x Slow Motion |
| `F4` | 100x Slow Motion (almost freezes the game) |
| `F7` | Enable Noclip/FPS Mode with Magenta Box |
| `[` | Move vertically upwards |
| `]` | Move vertically downwards |

### Common input problems

- Menus might not operate properly, or be invisible while in FPS mode. If this happens **Hit `F9` to revert to normal mode**.
- In Somniums, don't hold right click to rotate - use only the mouse. If you right click, you'll rotate in two different ways at once.

### F7 mode / magenta box

- You can press `F7` instead of `F8` to enter FPS mode. If you do this, a magenta box will spawn directly underneath the player (it's the standard Unity cube).
- In somniums, the box can be used to lift and push the player around.
- If the magenta box gets in the way of the camera, hold right click while moving the mouse and you can rotate the camera around the magenta box.

#### Known Bugs and wierd behaviors / Reporting Bugs

As this is a just a hack of the game and a side project for me, some "bugs" might not be worth fixing. I'll state the known bugs below, but might not ever fix them:

- Cameras may not always revert to their original position when you press F9 (mainly during cinematics)
  - Workaround: None
- The camera may starts rotating randomly in somniums
  - Workaround: enter and leave the main menu and it should stop.
- When a new version of the game is released, probably everything will break until a new version of the mod is issued.
  - Workaround: wait for a new release of the mod
- A few scenes look like they're in-game, but they are actually videos. Since it's a video, you cannot move the camera.
  - None
- Menus Disappear in FPS mode
  - Press `F9` to disable FPS mode when attempting to navigate menus

## Reporting Bugs

Please report bugs by raising an issue on this github repository, and I might *eventually* get around to fixing them. If you don't want to create a Github account, you can PM me @drojf0 on twitter.

## Interesting findings

Insert Spoiler Tagged Images here

## TODO

- Add button to hide the UI (for taking screenshots) ... is there already a button for this in the game?
- Reduce near field clip of camera to allow being closer to objects without clipping

## Developer's Instructions / Reproduction Instructions

See the [Developer Readme](Developer_Readme.md) for reproduction instructions and various other useful information. You may be able to port the code to other Unity games.
