# AI: The Somnium Files FPS Camera Mod

This mod adds a FPS style camera to all? parts of the game, to inspect parts of the game you can't normally see.

I don't recommend playing the game the first time around with this mod enabled, as various things may break unexpectedly.

## How to Install/Use

### Installation

- Go to the [Releases Page](https://github.com/drojf/ai_somnium_freecam/releases)
- Download the latest modded `Assembly-CSharp.dll`, and replace your game DLL (in `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed folder`). Remember to keep a backup of your original `.dll`

### Usage and Notes

- Press **F8 to enable FPS mode**, and **F9 to revert to normal mode**
- Use the arrow keys to translate the camera. Hold the shift key to move faster. Use the mouse to rotate the camera.
- Any changes to the camera you make apply to ALL cameras - this is why the character portraits move when you move.
- Certain scenes look like they're in-game, but they are actually videos. Since it's a video, you cannot move the camera.
- Common Problems:
  - Don't hold right click in somniums to rotate - use only the mouse.
  - **Make sure to exit FPS mode before accessing the menus (F9)**, otherwise you can't operate the menus/flowchart will be empty.

#### Known Bugs and wierd behaviors

- Cameras won't always revert to their proper position when you press F9 (especially during cinematic scenes)
- Cinematic scenes while in somnium seem to break my method of moving the camera - but you can still rotate the camera.
- The pink box in somniums represents your current physical position in the world. When pressing F8, the pink box will spawn directly underneath the player. A side affect of this is that the box can be used to lift and push the player around.
- If the pink box gets in the way, hold right click while moving the mouse and you can rotate the camera around the pink box.
- Since my method moves all cameras, including the character portrait camera, the character portrait will move/disappear when you use FPS mode.
- If the camera starts rotating randomly, enter and leave the main menu and it should stop.

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
Dictionary<CinemachineFreeLook, Transform> backupFollows;
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
                //Delete the custom freelook camera
                Destroy(customFreeLook);
                customFreeLook = null;
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

            // Restore cinemachine follow targets
            foreach(CinemachineFreeLook freelook in FindObjectsOfType<CinemachineFreeLook>())
            {
                if(backupFollows.TryGetValue(freelook, out Transform backupFollow))
                {
                    freelook.Follow = backupFollow;
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
            backupFollows.Clear();
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

            if(backupFollows == null)
            {
                backupFollows = new Dictionary<CinemachineFreeLook, Transform>();
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

            // TODO: Spawn a unity object (lets call it tracker), save it to the class
            if(cube == null)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            }

            //Clone the existing  CinemachineFreeLook camera (if it exists)
            foreach(CinemachineFreeLook freelook in FindObjectsOfType<CinemachineFreeLook>())
            {
                //Set the cube's location to the last freelook follow's position, to give a decent initial position
                cube.transform.position = freelook.Follow.position;

                customFreeLook = (CinemachineFreeLook) Instantiate(freelook);
                customFreeLook.name = "hackedFPSFreeLook";
                //customFreeLook.enabled = true;

                //set Follow to cube
                customFreeLook.Follow = cube.transform;

                //give it the highest priority
                customFreeLook.m_Priority = int.MaxValue;
                break;


                //backupFollows[freelook] = freelook.Follow;
                //freelook.Follow = cube.transform;
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

- Only modify the current camera, not all cameras in the scene. (Note: Using dnspy's debug mode, Camera.allCameras actually lists named cameras)
- Fix FPS Camera so it doesn't get reset each time camera changes, and works at all in somnium cinematics (Need to create proper Cinemachine camera)

## Notes

- I want to use the "inputaxis" function, but I'm not sure what they named the axis. I haven't tried the default values, maybe that's worth a try?
- The axis (for the mouse X/Y) appear to be the default "Mouse X" and "Mouse Y", as seen in the `CinemachineCustomAxis.GetAxisCustom()` function.
- LuaCameraController?

### Camera names

- "Right Camera" - main camera for ADV mode?
- "Character Camera" - Somnium camera (interacts with Cinemachine)
- "Camera" - used for character portraits
- Other camera names: BackgroundCamera, UICamera, UICamera2, UICamera3D, ButtonCamer, FrontCamera, MiddleCamera, AIBALL_RENDER_Camera, RightWindow

## Resources Used

- https://www.unknowncheats.me/forum/unity/285864-beginners-guide-hacking-unity-games.html
- DnSpy
- https://docs.unity3d.com/Packages/com.unity.cinemachine@2.3/manual/CinemachineOverview.html
- https://docs.unity3d.com/Packages/com.unity.cinemachine@2.3/manual/CinemachineFreeLook.html

### Unity Pages

https://docs.unity3d.com/ScriptReference/Camera-allCameras.html
