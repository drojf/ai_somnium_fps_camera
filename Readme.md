# AI: The Somnium Files FPS Camera Mod

This mod adds a FPS style camera to most parts of the game, to inspect parts of the game you can't normally see.

I don't recommend your first playthrough have this mod enabled, as various things may break unexpectedly.

I also don't want to disappoint anyone - while I haven't thoroughly inspected all of the levels, the game appears to be fairly "clean" and I haven't found anything out of the ordinary. But if you just want to take some nice screenshots this mod might be useful.

## How to Install/Use

### Installation

- Go to the [Releases Page](https://github.com/drojf/ai_somnium_freecam/releases)
- Download the latest modded `Assembly-CSharp.dll`
- Open the `Managed` folder of your game, located at: `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed`
- Replace your game DLL with the downloaded one. Remember to keep a backup of your original `.dll`!

### Using the Mod / Keyboard Bindings

- This mod only works with mouse and keyboard (on PC)

- FPS Controls:
  - Press **F8 to enable FPS mode**, and **F9 to revert to normal mode**
  - Put your fingers on the P, `;`, L, and `'` keys, and pretend they're the arrow keys
  - Use P and `;` to move forwards and backwards
  - Use L and `'` to move left and right
  - Hold the shift key to move faster
  - Move the mouse without holding any mouse buttons to rotate the camera.
  - Optional: In Somniums, press F7 to enter FPS mode and also spawn a pink box.

- Slow Motion Controls:
  - F2: Disables Slow Motion (1x speed)
  - F3: Slow motion at 10x slower than normal
  - F4: Slow motion at 100x slower than normal (useful if you want to take a screenshot of something)

- Common Problems:
  - Don't hold right click in somniums to rotate - use only the mouse.
  - Menus might not operate properly, or be invisible while in FPS mode. If this happens **Hit F9 to revert to normal mode**.
  - A few scenes look like they're in-game, but they are actually videos. Since it's a video, you cannot move the camera.

#### Known Bugs and wierd behaviors / Reporting Bugs

Please report bugs by raising an issue on this github repository, and I might *eventually* get around to fixing them. If you don't want to create a Github account, you can PM me @drojf0 on twitter.

As this is a just a hack of the game and a side project for me, some "bugs" might not be worth fixing.

- Cameras may not always revert to their original position when you press F9
- If the camera starts rotating randomly in somniums, enter and leave the main menu and it should stop.
- If a new version of the game is released, probably everything will break until a new version of the mod is issued.
- Somniums using the "Apartment" level may behave incorrectly (presumably because the base for the apartment is the "tutorial" level which may have it's cameras setup differently). It may be possible other levels that I've not tested also have problems, while other levels work just fine.

##### F7 mode / Pink box

- When pressing F7, a pink box will spawn directly underneath the player (it's the standard Unity cube).
- The box can be used to lift and push the player around.
- If the pink box gets in the way, hold right click while moving the mouse and you can rotate the camera around the pink box.

#### TODO

- Add button to hide the UI (for taking screenshots) ... is there already a button for this in the game?
- Reduce near field clip of camera to allow being closer to objects without clipping

#### Interesting findings

- I actually haven't found anything "unexpected", like unused rooms etc in the game - it seems fairly clean/expected.
- Although you cannot jump in somniums, the game has a falling animation for characters (presumably it comes as a default feature of whatever they used). You can also walk and fall down from any collidable object in the game.

## Developer's Instructions / Reproduction Instructions

See the [Developer Readme](Developer_Readme.md) for reproduction instructions and various other useful information.
