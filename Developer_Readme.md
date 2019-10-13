# Developer Readme

## Reproduction Instructions

These instructions are for developers ONLY!

BIG NOTE: After editing the file multiple times with DnSpy (mainly if you use "edit class"), the code may fail to compile. In this case, you should close all documents and re-open the original, unmodified `Assembly-CSharp.dll`. I'm not sure why this happens.

- Download DnSpy and extract it somewhere
- Navigate to the `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed folder`
- Open the `Assembly-CSharp.dll` with dnspy, **while it's in the same folder as all the other DLLs**. If it's not in the same folder as the other DLLS, dnspy won't be able to find them.
- Expand the `Assembly-CSharp.dll` arrow
- Expand the `Game` arrow
- Navigate to the `InputProc` class
- Right click the class and click "add class members"
  - merge the code located in the `InputProc.fragment.cs` file of this github repository
- Save the module (eg write the changes to the .dll). (You should enable "save extra metadata" or else the variable names will be lost in the saved DLLs?).

ALWAYS remember to save the module (with the game closed?), otherwise your changes won't be seen. Also, check that steam is running, otherwise the game won't launch (will get stuck on a black screen).

## Source Code

The source code for the mod is located in [`InputProc.fragment.cs`](InputProc.fragment.cs). It's not a complete class - you need to merge it into the existing `InputProc` class using DnSpy.

## Setting up debugging

NOTE: sometimes the debug point may fail to be set - try restarting DnSpy and trying again
NOTE2: if steam is not running, the game will fail to launch

I just followed the instructions from https://github.com/0xd4d/dnSpy/wiki/Debugging-Unity-Games, but the below might be useful to you anyway.

The current version of the game runs Unity 2017.4.17, 64-bit

- Download Unity 2017.4.17, 64-bit from https://github.com/0xd4d/dnSpy/releases
- Replace your existing one in the `AI The Somnium Files\AI_TheSomniumFiles_Data\Mono\EmbedRuntime` folder (keep a backup)
- Click the green > button in dnspy
- **In the dropdown, click "Unity game" option**. This setting resets when DnSpy is closed, so remember to set it.
- I think you must launch the 64 bit version of DnSpy for debugging to work
- Find the game .exe `AI_TheSomniumFiles.exe`
- Start debugging

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
