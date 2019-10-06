# AI: The Somnium Files FPS Camera Mod

This mod adds a FPS style camera to all? parts of the game, to inspect parts of the game you can't normally see.

I don't recommend playing the game the first time around with this mod enabled, as various things may break unexpectedly.

## How to Install/Use

### Installation

- Go to the [Releases Page](https://github.com/drojf/ai_somnium_freecam/releases)
- Download the latest modded `Assembly-CSharp.dll`, and replace your game DLL (in `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed folder`). Remember to keep a backup of your original `.dll`

### Usage and Notes

- Press **F8 to enable FPS mode**, and **F9 to revert to normal mode**. I've tried my best to "revert to normal mode" but this can break sometimes.
- Use the arrow keys to translate the camera. Hold the shift key to move faster. Use the mouse to rotate the camera.
- NOTE: make sure to exit FPS mode before accessing the menus, or you'll have various problems. If you do this accidentally, disable FPS mode, then return to the main menu to fix it.
- Any changes to the camera you make apply to ALL cameras - this is why the character portraits move when you move.
- Certain scenes look like they're in-game, but they are actually videos. Since it's a video, you cannot move the camera.

## Developer's Instructions / Reproduction Instructions

These instructions are for developers ONLY!

BIG NOTE: when you do "edit class" with dnspy, it may screw up the code depending on your settings. In this case, you should re-start with the original exe if you made changes, or just close/open the program again. Maybe some DnSpy experts can tell me what's happeneing or if I'm doing something wrong.

- Download DnSpy and extractt it somewhere
- Navigate to the `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed folder`
- Open the `Assembly-CSharp.dll` with dnspy, **while it's in the same folder as all the other DLLs**. If it's not in the same folder as the other DLLS, dnspy won't be able to find them.
- Expand the `Assembly-CSharp.dll` arrow
- Expand the `Game` arrow
- Navigate to the `InputProc` class
- Right click the class and click "add class members"
  - copy in the below code just below the `InputProc` class definition
- Save the module. (You should enable "save extra metadata" or else the variable names will be lost in the saved DLLs?).

### Source Code for Update() function of InputProc class

```csharp
struct CameraBackupState {
    public Vector3 position;
    public Vector3 eulerAngles;
}

float rotX;
float rotY;
bool fpsEnabled = false;
Dictionary<Camera, CameraBackupState> backupCameraPositions;

// Game.InputProc
private void LateUpdate()
{
    if(this.fpsEnabled)
    {
        float mouseSensitivity = 10f;
        foreach (Camera camera in Camera.allCameras)
        {
            //Add FPS camera rotation
            this.rotX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            this.rotY += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            this.rotY = Mathf.Clamp(this.rotY, -90f, 90f);
            camera.transform.eulerAngles = new Vector3(-this.rotY, this.rotX, 0f);

            //Add FPS camera movement
            Vector3 moveDir = default(Vector3);
            float speed = (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) ? 20f : 2f;
            speed *= Time.deltaTime;

            //Move right and forward relative to the camera
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveDir += speed * camera.transform.forward;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                moveDir -= speed * camera.transform.forward;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveDir += speed * camera.transform.right;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveDir -= speed * camera.transform.right;
            }

            //Always move up/down the Z axis
            if (Input.GetKey(KeyCode.LeftBracket))
            {
                moveDir += speed * Vector3.up;
            }
            if (Input.GetKey(KeyCode.RightBracket))
            {
                moveDir -= speed * Vector3.up;
            }

            camera.transform.position += moveDir;
        }

        if(Input.GetKeyDown(KeyCode.F9)) {
            this.fpsEnabled = false;
            //attempt to restore each camera's transform. If you enter a menu or change scene without exiting, this might break.
            foreach (Camera camera in Camera.allCameras)
            {
                if(backupCameraPositions.TryGetValue(camera, out CameraBackupState backupCameraState))
                {
                    camera.transform.position = backupCameraState.position;
                    camera.transform.eulerAngles = backupCameraState.eulerAngles;
                }
            }
            this.backupCameraPositions.Clear();
        }
    }
    else {
        if(Input.GetKeyDown(KeyCode.F8))
        {
            this.fpsEnabled = true;
            this.rotX = 0;
            this.rotY = 0;

            if(backupCameraPositions == null)
            {
                backupCameraPositions = new Dictionary<Camera, CameraBackupState>();
            }

            //backup the current camera(s) position
            foreach (Camera camera in Camera.allCameras)
            {
                backupCameraPositions[camera] = new CameraBackupState() {
                    position = camera.transform.position,
                    eulerAngles = camera.transform.eulerAngles,
                };
            }
        }
    }
}
```

### Setting up debugging

NOTE: I have found debugging not to be very useful, but you might find it useful for checking variables/values etc.

I just followed the instructions from https://github.com/0xd4d/dnSpy/wiki/Debugging-Unity-Games, but the below might be useful to you anyway.

The current version of the game runs Unity 2017.4.17, 64-bit
- Download Unity 2017.4.17, 64-bit from https://github.com/0xd4d/dnSpy/releases
- Replace your existing one in the `AI The Somnium Files\AI_TheSomniumFiles_Data\Mono\EmbedRuntime` folder (keep a backup)
- Click the green > button in dnspy
- In the dropdown, click "Unity game" option
- Find the game .exe `AI_TheSomniumFiles.exe`
- Start debugging

## TODO

- Only modify the current camera, not all cameras in the scene.

## Notes

- I want to use the "inputaxis" function, but I'm not sure what they named the axis. I haven't tried the default values, maybe that's worth a try?
- The axis (for the mouse X/Y) appear to be the default "Mouse X" and "Mouse Y", as seen in the `CinemachineCustomAxis.GetAxisCustom()` function.
- LuaCameraController?

## Resources Used

- https://www.unknowncheats.me/forum/unity/285864-beginners-guide-hacking-unity-games.html
- DnSpy

### Unity Pages

https://docs.unity3d.com/ScriptReference/Camera-allCameras.html
