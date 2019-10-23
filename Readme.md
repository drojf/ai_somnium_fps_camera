# AI: The Somnium Files FPS Camera Mod

This mod adds the following features to the game:

- A "Noclip Mode" FPS style camera allowing you to explore ADV mode scenes and Somniums. You freely rotate and translate the camera, and can move faster with `SHIFT` key to get around large levels (example: https://www.youtube.com/watch?v=6zuE3RdaCFU)
- Screenshot helper features like:
  - the ability to entirely hide the game GUI (and the mod's shortcuts cheatsheet)
  - the ability to slow down the game by 10x/100x

This mod may be useful for taking screenshots or videos of the game. You can also explore the maps of the game - but note that the game appears to be fairly "clean" - there are a few wierd T-posing/strangely posed characters and such, but overall everything is as expected. I don't recommend your first playthrough have this mod enabled, as various things may break unexpectedly.

## Tips from the developer

- Please try out all the major hotkeys (displayed in-game), even if you don't think you'll use them - you might find them handy at some point
- Please see the "Known bugs and wierd behaviors" section of this page if you have problems with the mod.

## Installation

- Go to the [Releases Page](https://github.com/drojf/ai_somnium_freecam/releases)
- Download the latest modded `Assembly-CSharp.dll`
- Open the `Managed` folder of your game, located at: `Steam\steamapps\common\AI The Somnium Files\AI_TheSomniumFiles_Data\Managed`
  - You can also right click on the game in steam, click "properties", click the "local files" tab, then click "Browse Local Files..." to open the game folder. Then navigate to `\AI_TheSomniumFiles_Data\Managed`
- Replace your `Assembly-CSharp.dll` with the downloaded one (BUT, remember to keep a backup of your original `.dll` file!)

## How to Use the Mod / Controls

**Please note that this mod only works with mouse and keyboard** (... and is PC only)

Update: The keyboard shortcuts are now shown in-game. To hide the shortcuts cheat-sheet, press F11.

### Movement Controls

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

### Screenshot Controls

These shortcuts are useful when taking screenshots to hide the GUI or slow down the action.

| Key | Action |
| --- | --- |
| `F11` | Hide shortcuts menu (for taking screenshots) |
| `F10` | Hide Game GUI (for taking screenshots) |
| `F2` | Disable Slow Motion (revert to normal speed) |
| `F3` | 10x Slow Motion |
| `F4` | 100x Slow Motion (almost freezes the game) |
| `F7` | Toggle clip distance (get closer without clipping) |

### Optional / Rarely Used Controls

| Key | Action |
| --- | --- |
| `F6` | Enable Noclip/FPS Mode with Magenta Box under player |
| `[` | Move vertically upwards (lift player in somnium with Magenta Box) |
| `]` | Move vertically downwards |

### Common input problems

- Menus might not operate properly, or be invisible while in FPS mode. If this happens **Hit `F9` to revert to normal mode**.
- In Somniums, don't hold right click to rotate - use only the mouse. If you right click, you'll rotate in two different ways at once.

### Clip Distance

- In some scenes, the camera will clip into objects very easily. By pressiong `F7` you can toggle to a shorter clip distance. Please note that doing this may cause graphical artifacts with shadows. In addition, ADV mode deliberately uses clipping to avoid seeing the characters own head - with this mode enabled and the camera in the default ADV position, you'll see the inside of the character's head.

### F6 mode / magenta box

- You can press `F6` instead of `F8` to enter FPS mode. If you do this, a magenta box will spawn directly underneath the player (it's the standard Unity cube).
- In somniums, the box can be used to lift and push the player around.
- If the magenta box gets in the way of the camera, hold right click while moving the mouse and you can rotate the camera around the magenta box.

#### Known bugs and wierd behaviors / Reporting bugs

As this is a just a hack of the game and a side project for me, some "bugs" might not be worth fixing. I'll state the known bugs below, but might not ever fix them:

- Cameras may not always revert to their original position when you press F9 (mainly during cinematics)
  - Workaround: None
- The camera may starts rotating randomly in somniums
  - Workaround: Disable the FPS camera, move a bit, then enable it again. You can also try entering and leaving the main menu.
- When a new version of the game is released, probably everything will break until a new version of the mod is issued.
  - Workaround: wait for a new release of the mod
- A few scenes look like they're in-game, but they are actually videos. Since it's a video, you cannot move the camera.
  - Workaround: None
- Menus Disappear in FPS mode
  - Press `F9` to disable FPS mode when attempting to navigate menus
  - Press `F10` to toggle GUI hiding, incase you forgot you left it on
- In somniunms, the "choice" buttons and some other GUI are not hidden while making the choice. Immediately after making the choice, they go away, so this is not really a problem.
  - Workaround: None

## Reporting Bugs

Please report bugs by raising an issue on this github repository, and I might *eventually* get around to fixing them. If you don't want to create a Github account, you can PM me @drojf0 on twitter.

## Interesting findings

Insert Spoiler Tagged Images here

## TODO

- ~~Add button to hide the UI (for taking screenshots) ... is there already a button for this in the game?~~
- ~~Reduce near field clip of camera to allow being closer to objects without clipping~~ (https://forum.unity.com/threads/recommended-minimum-near-clipping-plane-of-cameras.348620/)
- Allow selectively hiding AIBALL? need to run some tests to see if this will work as intended
- Should this be added? Add momentum/smoothing to movement?

## Developer's Instructions / Reproduction Instructions

See the [Developer Readme](Developer_Readme.md) for reproduction instructions and various other useful information. You may be able to port the code to other Unity games.
