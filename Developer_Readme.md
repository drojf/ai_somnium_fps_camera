# Developer Readme

## Reproduction Instructions

These instructions are for developers ONLY!

BIG NOTE: when you do "edit class" with dnspy, it may screw up the code depending on your settings. In this case, you should re-start with the original exe if you made changes, or just close/open the program again. Maybe some DnSpy experts can tell me what's happeneing or if I'm doing something wrong.

- Download DnSpy and extract it somewhere
- Navigate to the `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed folder`
- Open the `Assembly-CSharp.dll` with dnspy, **while it's in the same folder as all the other DLLs**. If it's not in the same folder as the other DLLS, dnspy won't be able to find them.
- Expand the `Assembly-CSharp.dll` arrow
- Expand the `Game` arrow
- Navigate to the `InputProc` class
- Right click the class and click "add class members"
  - copy in the below code just below the `InputProc` class definition
- Save the module. (You should enable "save extra metadata" or else the variable names will be lost in the saved DLLs?).

ALWAYS remember to save the module (with the game closed?), otherwise your changes won't be seen.

## Source Code

The source code to be merged into the existing `InputProc` class is located in `InputProc.fragment.cs`

## Setting up debugging

NOTE: sometimes the debug point may fail to be set - try restarting DnSpy and trying again

I just followed the instructions from https://github.com/0xd4d/dnSpy/wiki/Debugging-Unity-Games, but the below might be useful to you anyway.

The current version of the game runs Unity 2017.4.17, 64-bit
- Download Unity 2017.4.17, 64-bit from https://github.com/0xd4d/dnSpy/releases
- Replace your existing one in the `AI The Somnium Files\AI_TheSomniumFiles_Data\Mono\EmbedRuntime` folder (keep a backup)
- Click the green > button in dnspy
- **In the dropdown, click "Unity game" option**. This setting resets when DnSpy is closed, so remember to set it.
- I think you must launch the 64 bit version of DnSpy for debugging to work
- Find the game .exe `AI_TheSomniumFiles.exe`
- Start debugging

## TODO

- add button to hide the UI ... is there already one in the game?
- Reduce near field clip of camera to allow being closer to objects without clipping

### Camera names

- "Right Camera" - main camera for ADV mode?
- "Character Camera" - Somnium camera (interacts with Cinemachine)
- "Camera" - used for character portraits
- Other camera names: BackgroundCamera, UICamera, UICamera2, UICamera3D, ButtonCamer, FrontCamera, MiddleCamera, AIBALL_RENDER_Camera, RightWindow

Camera List in Somnium:

- BackgroundCamera
- UICamera
- UICamera2
- UICamera3D
- MiddleCamera
- FrontCamera
- UICamera
- AIBALL_RENDER_Camera
- Character Camera
- RightWindow

Camera list in ADV mode:

- BackgroundCamera
- UICamera
- FrontCamera
- ButtonCamera
- UICamera
- Camera
- RightCamera
- Camera01 (Cinematic camera?)

## Resources Used

- https://www.unknowncheats.me/forum/unity/285864-beginners-guide-hacking-unity-games.html
- DnSpy
- https://docs.unity3d.com/Packages/com.unity.cinemachine@2.3/manual/CinemachineOverview.html
- https://docs.unity3d.com/Packages/com.unity.cinemachine@2.3/manual/CinemachineFreeLook.html

### Unity Pages

https://docs.unity3d.com/ScriptReference/Camera-allCameras.html
