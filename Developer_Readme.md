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

### Rendering

From what I can gather, rendering works like this:

- Various cameras render to a texture
- This gets put on a UI UnityEngine.UI.RawImage type object (thats the base type at least) called "Image" (there are multiple of these named "Image" when viewed in the inspector)
- The camera named "UICamera" then renders the "Image" UI widget (and all the other UI widgets)

As a result, if you disable either the "UICamera" camera or disable the "Image" GUI widget, the 3D part of the game won't be displayed.

### Camera names

- "Right Camera" - main camera for ADV mode?
- "Character Camera" - Somnium camera (interacts with Cinemachine)
- "Camera" - used for character portraits
- Other camera names: BackgroundCamera, UICamera, UICamera2, UICamera3D, ButtonCamer, FrontCamera, MiddleCamera, AIBALL_RENDER_Camera, RightWindow

Camera List in Somnium:

- BackgroundCamera - might be used for UI?
- UICamera - Disabling this makes screen blank in somniums, so maybe it's the final compositing step. Cursor might be rendered also.
- UICamera2 - might be used for UI?
- UICamera3D - might be used for UI?
- MiddleCamera - might be used for UI?
- FrontCamera - might be used for UI?
- RightCamera - might be used for UI?
- RightWindow - might be used for UI?
- Character Camera - used for character portraits
- AIBALL_RENDER_Camera - used for AIBALL vision (I assume)

Camera list in ADV mode:

- BackgroundCamera
- UICamera
- FrontCamera
- ButtonCamera
- UICamera
- Camera - used for character portraits?
- RightCamera
- Camera01 (Cinematic camera?)

### GUI "Graphic" list

There are multiple graphics named "image" - these seem to be used as render targets.
One is used to render the main 3D display
One might be used for the AIBall display.
I wanted to selectively enable the AIBall display, but since it has the same name it's difficult to pick out.
The "Mask" graphic may also have something to do with the AIBall display

Boss's room example:

- array	{UnityEngine.UI.Graphic[0x00000060]}	UnityEngine.Object[] {UnityEngine.UI.Graphic[]}
- [0]	{TutorialWindow (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [1]	{Image3b (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [2]	{Image2 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [3]	{IconText (UnityEngine.UI.RawImage)}	UnityEngine.UI.Graphic {UnityEngine.UI.RawImage}
- [4]	{Image01 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [5]	{Prompt (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [6]	{Text (TMPro.TextMeshProUGUI)}	UnityEngine.UI.Graphic {TMPro.TextMeshProUGUI}
- [7]	{Image_R (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [8]	{TutorialWindow (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [9]	{TutorialWindow (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [10]	{TutorialWindow (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [11]	{TutorialWindow (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [12]	{Image4 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [13]	{Image1 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [14]	{Image5 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [15]	{Image3c (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [16]	{Text (TMPro.TextMeshProUGUI)}	UnityEngine.UI.Graphic {TMPro.TextMeshProUGUI}
- [17]	{Button (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [18]	{Image (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [19]	{Image (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [20]	{Text (TMPro.TextMeshProUGUI)}	UnityEngine.UI.Graphic {TMPro.TextMeshProUGUI}
- [21]	{Folder (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [22]	{Base (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [23]	{Image3 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [24]	{Button_02 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [25]	{FilterBlur (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [26]	{Background (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [27]	{BG01 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [28]	{Filter (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [29]	{Mask (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [30]	{Image2D (UnityEngine.UI.RawImage)}	UnityEngine.UI.Graphic {UnityEngine.UI.RawImage}
- [31]	{LeftFilter (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [32]	{Image (UnityEngine.UI.RawImage)}	UnityEngine.UI.Graphic {UnityEngine.UI.RawImage}
- [33]	{Image04 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [34]	{Image (UnityEngine.UI.RawImage)}	UnityEngine.UI.Graphic {UnityEngine.UI.RawImage}
- [35]	{ScreenScaler (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [36]	{Image (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [37]	{Outline_back (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [38]	{Normal (UnityEngine.UI.RawImage)}	UnityEngine.UI.Graphic {UnityEngine.UI.RawImage}
- [39]	{FilterTop (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [40]	{Text (TMPro.TextMeshProUGUI)}	UnityEngine.UI.Graphic {TMPro.TextMeshProUGUI}
- [41]	{[Normal] (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [42]	{Frame (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [43]	{Image00 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [44]	{Button_03 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [45]	{Image2D (UnityEngine.UI.RawImage)}	UnityEngine.UI.Graphic {UnityEngine.UI.RawImage}
- [46]	{Image (UnityEngine.UI.RawImage)}	UnityEngine.UI.Graphic {UnityEngine.UI.RawImage}
- [47]	{Mask (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [48]	{Button_01 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [49]	{ui_main_infoW02_hit_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [50]	{ui_main_infoW01_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [51]	{button_icon (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [52]	{image (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [53]	{image_rest2 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [54]	{image_rest3 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [55]	{Base (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [56]	{Mask (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [57]	{ui_main_infoW02_hit_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [58]	{ui_main_infoW01_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [59]	{button_icon (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [60]	{image (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [61]	{Base (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [62]	{ui_main_infoW02_hit_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [63]	{ui_main_infoW01_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [64]	{button_icon (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [65]	{image (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [66]	{Base (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [67]	{Mask (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [68]	{ui_main_infoW02_hit_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [69]	{ui_main_infoW01_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [70]	{button_icon (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [71]	{image (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [72]	{Base (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [73]	{Mask (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [74]	{LoopHit_01 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [75]	{ColorHit_01 (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [76]	{ui_main_infoW02_hit_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [77]	{ui_main_infoW01_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [78]	{button_icon (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [79]	{image (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [80]	{Base (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [81]	{Mask (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [82]	{ui_main_infoW02_hit_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [83]	{ui_main_infoW01_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [84]	{button_icon (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [85]	{image (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [86]	{Base (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [87]	{Mask (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [88]	{ui_main_infoW02_hit_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [89]	{ui_main_infoW01_add (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [90]	{button_icon (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [91]	{image (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [92]	{Base (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [93]	{Mask (Game.NonDrawingGraphic)}	UnityEngine.UI.Graphic {Game.NonDrawingGraphic}
- [94]	{BaseImage (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}
- [95]	{EffectBase (UnityEngine.UI.Image)}	UnityEngine.UI.Graphic {UnityEngine.UI.Image}


### Class info

- "TextController" used to draw the text glyphs on the screen (but not for the portrait window or text background)

## Resources Used

- https://www.unknowncheats.me/forum/unity/285864-beginners-guide-hacking-unity-games.html
- DnSpy
- https://docs.unity3d.com/Packages/com.unity.cinemachine@2.3/manual/CinemachineOverview.html
- https://docs.unity3d.com/Packages/com.unity.cinemachine@2.3/manual/CinemachineFreeLook.html
- https://docs.unity3d.com/Manual/gui-Basics.html
- https://docs.unity3d.com/2018.1/Documentation/ScriptReference/UI.Graphic.html

### Unity Pages

https://docs.unity3d.com/ScriptReference/Camera-allCameras.html