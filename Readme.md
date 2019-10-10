# AI: The Somnium Files FPS Camera Mod

This mod adds a FPS style camera to all? parts of the game, to inspect parts of the game you can't normally see.

I don't recommend your first playthrough have this mod enabled, as various things may break unexpectedly.

## How to Install/Use

### Installation

- Go to the [Releases Page](https://github.com/drojf/ai_somnium_freecam/releases)
- Download the latest modded `Assembly-CSharp.dll`, and replace your game DLL (in `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed folder`). Remember to keep a backup of your original `.dll`

### Usage and Notes

- This mod only works with mouse and keyboard (on PC)
- Controls:
  - Press **F8 to enable FPS mode**, and **F9 to revert to normal mode**
  - Use the **arrow keys** to translate the camera.
  - Hold the shift key to move faster.
  - Move the mouse without holding any mouse buttons to rotate the camera.
- Common Problems:
  - Don't hold right click in somniums to rotate - use only the mouse.
  - Menus won't operate properly, or be invisible while in FPS mode. **Hit F9 to exit FPS mode to fix the menus**
  - A few scenes look like they're in-game, but they are actually videos. Since it's a video, you cannot move the camera.

#### Known Bugs and wierd behaviors

- Cameras may not always revert to their original position when you press F9
- The pink box in somniums represents your current physical position in the world. When pressing F8, the pink box will spawn directly underneath the player. A side affect of this is that the box can be used to lift and push the player around.
- If the pink box gets in the way, hold right click while moving the mouse and you can rotate the camera around the pink box.
- If the camera starts rotating randomly in somniums, enter and leave the main menu and it should stop.

#### Interesting findings

- I actually haven't found anything "unexpected", like unused rooms etc in the game - it seems fairly clean/expected.
- Although you cannot jump in somniums, the game has a falling animation for characters (presumably it comes as a default feature of whatever they used). You can also walk and fall down from any collidable object in the game.

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

ALWAYS remember to save the module (with the game closed?), otherwise your changes won't be seen.

### Source Code for Update() function of InputProc class

#### Add code at top of InputProc.cs

```csharp
using Cinemachine;
```

#### Add code to InputProc Class

```csharp
struct CameraBackupState {
    public Vector3 position;
    public Vector3 eulerAngles;
}

float rotX;
float rotY;
bool fpsEnabled = false;
Dictionary<Camera, CameraBackupState> backupCameraPositions;
Dictionary<CinemachineCollider, bool> backupCollisionStates;
GameObject cube;
CinemachineFreeLook customFreeLook;

// Game.InputProc
private void LateUpdate()
{
    if(this.fpsEnabled)
    {
        float mouseSensitivity = 100f;

        //Add FPS camera rotation
        this.rotX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        this.rotY += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        this.rotY = Mathf.Clamp(this.rotY, -90f, 90f);

        foreach (Camera camera in Camera.allCameras)
        {
            // Only move the right camera (used in ADV mode) and character camera (used in somniums)
            if(camera.name == "RightCamera" || camera.name == "Character Camera")
            {
                camera.transform.eulerAngles = new Vector3(-this.rotY, this.rotX, 0f);

                //Backup the camera if it hasn't already been backed up
                if(!backupCameraPositions.ContainsKey(camera))
                {
                    backupCameraPositions[camera] = new CameraBackupState() {
                        position = camera.transform.position,
                        eulerAngles = camera.transform.eulerAngles,
                    };
                }

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

                if (camera.name == "Character Camera")
                {
                    this.cube.transform.position += moveDir;
                }
                else
                {
                    // Each 'noraml' camera has an overridePosition where translation is accumulated
                    camera.transform.position += moveDir;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.F9)) {
            this.fpsEnabled = false;

            if(customFreeLook != null)
            {
                //Set the custom freelook camera to the lowest priority so it becomes disabled
                customFreeLook.m_Priority = int.MinValue;
            }

            if(cube != null)
            {
                Destroy(cube);
                cube = null;
            }

            //attempt to restore each camera's transform. If you enter a menu or change scene without exiting, this might break.
            foreach (Camera camera in Camera.allCameras)
            {
                if(backupCameraPositions.TryGetValue(camera, out CameraBackupState backupCameraState))
                {
                    camera.transform.position = backupCameraState.position;
                    camera.transform.eulerAngles = backupCameraState.eulerAngles;
                }
            }

            // Restore collider states
            foreach(CinemachineCollider collider in  UnityEngine.Object.FindObjectsOfType<CinemachineCollider>())
            {
                if(backupCollisionStates.TryGetValue(collider, out bool avoidObstacles))
                {
                    collider.m_AvoidObstacles = avoidObstacles;
                }
            }

            // Clear backups
            this.backupCameraPositions.Clear();
            backupCollisionStates.Clear();
        }
    }
    else {
        if(Input.GetKeyDown(KeyCode.F7) | Input.GetKeyDown(KeyCode.F8))
        {
            this.fpsEnabled = true;
            this.rotX = 0;
            this.rotY = 0;

            if(backupCameraPositions == null)
            {
                backupCameraPositions = new Dictionary<Camera, CameraBackupState>();
            }

            if(backupCollisionStates == null)
            {
                backupCollisionStates = new Dictionary<CinemachineCollider, bool>();
            }

            //////////////// Modify somium camera so it can be moved properly ////////////////
            //Disable camera collisions on all cinemachine coliders
            foreach(CinemachineCollider collider in UnityEngine.Object.FindObjectsOfType<CinemachineCollider>())
            {
                backupCollisionStates[collider] = collider.m_AvoidObstacles;
                collider.m_AvoidObstacles = false;
            }

            // Spawn a unity object for the CinemachineFreeLook to follow, save it to the class
            if(Input.GetKeyDown(KeyCode.F8))
            {
                cube = new GameObject();
            }
            else
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            }

            //Clone the first existing CinemachineFreeLook camera (if it exists)
            foreach(CinemachineFreeLook freelook in FindObjectsOfType<CinemachineFreeLook>())
            {
                if(freelook.name != "hackedFPSFreeLook") {
                    //Set the cube's location to the last freelook follow's position, to give a decent initial position
                    cube.transform.position = freelook.Follow.position;

                    if(customFreeLook == null)
                    {
                        customFreeLook = (CinemachineFreeLook) Instantiate(freelook);
                    }

                    customFreeLook.name = "hackedFPSFreeLook";

                    //set Follow to cube
                    customFreeLook.Follow = cube.transform;

                    //give it the highest priority
                    customFreeLook.m_Priority = int.MaxValue;
                    break;

                }
            }
            //////////////// Modify somium camera so it can be moved properly ////////////////
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

- add button to hide the UI ... is there already one in the game?
- hide the pink box or use another invisible game object

## Notes

- I want to use the "inputaxis" function, but I'm not sure what they named the axis. I haven't tried the default values, maybe that's worth a try?
- The axis (for the mouse X/Y) appear to be the default "Mouse X" and "Mouse Y", as seen in the `CinemachineCustomAxis.GetAxisCustom()` function.
- LuaCameraController?
- Reduce near field clip to allow being closer to objects without clipping

### Camera names

- "Right Camera" - main camera for ADV mode?
- "Character Camera" - Somnium camera (interacts with Cinemachine)
- "Camera" - used for character portraits
- Other camera names: BackgroundCamera, UICamera, UICamera2, UICamera3D, ButtonCamer, FrontCamera, MiddleCamera, AIBALL_RENDER_Camera, RightWindow

Camera List in Somnium:

- BackgroundGamera
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

## Resources Used

- https://www.unknowncheats.me/forum/unity/285864-beginners-guide-hacking-unity-games.html
- DnSpy
- https://docs.unity3d.com/Packages/com.unity.cinemachine@2.3/manual/CinemachineOverview.html
- https://docs.unity3d.com/Packages/com.unity.cinemachine@2.3/manual/CinemachineFreeLook.html

### Unity Pages

https://docs.unity3d.com/ScriptReference/Camera-allCameras.html
