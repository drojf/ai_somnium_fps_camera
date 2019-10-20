// This is a source code fragment - you must merge it into existing code using DnSpy.
// Please read the Readme.md for more instruction

//Merge this line into existing includes
using Cinemachine;
using UnityEngine.UI;

//Merge into existing InputProc class
class InputProc
{
    struct CameraBackupState {
        public Vector3 position;
        public Vector3 eulerAngles;
    }

    bool immediateGUIHidden;
    float rotX;
    float rotY;
    bool fpsEnabled = false;
    //Used to save/restore the "enabled" state of the GUI cameras
    Dictionary<Camera, Vector3> backupGUICameraState;
    //Used to save/restore the color (including alpha value) of the GUI elements
    //Dictionary<Graphic, Color> backupGUIColor;
    //Used to save the position/rotation of each 'normal' camera
    Dictionary<Camera, CameraBackupState> backupCameraPositions;
    //Used to save whether collision is enabled/disabled of each cinemachine collider (which is attached to a cinemachine camera)
    Dictionary<CinemachineCollider, bool> backupCollisionStates;
    GameObject cube;
    CinemachineFreeLook customFreeLook;
    Vector3 camPosOverride;

    // Game.InputProc
    // Tip: Use the Unity Editor to prototype the GUI rather than trying to do it in-game
    private void OnGUI()
    {
        if(!immediateGUIHidden)
        {
            //GUI.Box(new Rect(10f, 10f, 450f, 90f), "Mod Menu");
            //GUI.Button(new Rect(20f, 100f, 400f, 20f), "Button 1");
            //GUI.Button(new Rect(20f, 120f, 400f, 20f), "Button 2");

            // Make a background box
            Rect backgroundArea = new Rect (10f, 10f, 400f, Screen.height - 20f);

            Rect contentArea = new Rect (backgroundArea.x + 10f, 
                                        backgroundArea.y + 20f, 
                                        backgroundArea.width - 20f, 
                                        backgroundArea.height - 20f);

            GUI.Box(backgroundArea, "Mod Menu");

            GUILayout.BeginArea(contentArea);

            GUILayout.Label(
@" Press F11 to toggle this Mod Menu! (for screenshots)

Basic Controls:
F8 - Enable Noclip/FPS mode
F9 - Disable Noclip/FPS mode
P and ; - Move forward and back (hold SHIFT to move faster)
L  and ' - Move left and right (hold SHIFT to move faster)
SHIFT - Hold to move faster
Mouse Rotation - Rotate the camera (while in FPS mode)

Extra Controls:
F10 - Toggle GUI
F2 - Disable Slow Motion (revert to normal speed)
F3 - 10x Slow Motion
F4 - 100x Slow Motion (almost freezes the game)
F7 - Enable Noclip/FPS Mode with Magenta Box

Press F11 to toggle this Mod Menu! (for screenshots)
");
            GUILayout.EndArea ();
        }
    }

    bool cameraMightBeActiveCamera(Camera camera) {
        return !camera.name.Contains("UICamera") && !camera.name.Contains("Button") && !camera.name.Contains("Background") && camera.name != "Camera";
    }

    // Game.InputProc
    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.F11))
        {
            immediateGUIHidden = !immediateGUIHidden;
        }

        // Press F12 to toggle GUI on/off
        if(Input.GetKeyDown(KeyCode.F10))
        {
            if(backupGUICameraState == null)
            {
                backupGUICameraState = new Dictionary<Camera, Vector3>();
                foreach (Camera camera in Camera.allCameras)
                {
                    //Disable the character bust camera (called "Camera")
                    if (camera.name == "Camera")
                    {
                        backupGUICameraState[camera] = camera.transform.position;
                        //Move the camera way out of the way so the GUI isn't rendered
                        //If you disable the camera, the game will just spawn a new camera
                        camera.transform.position = new Vector3(1000000, 1000000, 0);
                    }
                }

                //For all UI elements except those named "Image":
                // - Backup the UI color values
                // - Set 0 alpha
                foreach(Graphic graphic in UnityEngine.Object.FindObjectsOfType(typeof(Graphic)))
                {
                    // Set graphics not named "Image" to 0 alpha
                    if (graphic.name != "Image")
                    {
                        graphic.CrossFadeAlpha(0f, .1f, true);
                    }
                }
            }
            else
            {
                // Restore the "enabled" state of each GUI camera
                foreach(KeyValuePair<Camera, Vector3> kvp in backupGUICameraState)
                {
                    kvp.Key.transform.position = kvp.Value;
                }

                // Set all "UnityEngine.UI.Graphic" objects not named "Image" to 1 alpha
                // This doesn't properly restore the UI's alpha value (if it was not originally '1') but should be good enough
                foreach(Graphic graphic in UnityEngine.Object.FindObjectsOfType(typeof(Graphic)))
                {
                    if (graphic.name != "Image")
                    {
                        graphic.CrossFadeAlpha(1f, .1f, true);
                    }
                }

                backupGUICameraState = null;
            }
        }

        // Every keyframe, move the camera way out of the way so the GUI isn't rendered
        // We can't disable the camera, as the game will just spawn a new camera if it detects the normal camera is disabled.
        if(backupGUICameraState != null) 
        {
            foreach (Camera camera2 in Camera.allCameras)
            {
                if (camera2.name == "Camera")
                {
                    camera2.transform.position = new Vector3(1E+07f, 1E+07f, 0f);
                }
            }
        }

        // Use F2-4 for slowmo: F2 = normal, F3 = 10x slower, F4 = 100x slower
        // see https://docs.unity3d.com/ScriptReference/Time-timeScale.html)
        {
            float newTimeScale = 0f;

            if (Input.GetKeyDown(KeyCode.F2))
            {
                newTimeScale = 1f;
            }
            else if(Input.GetKeyDown(KeyCode.F3))
            {
                newTimeScale = 0.1f;
            }
            else if(Input.GetKeyDown(KeyCode.F4))
            {
                newTimeScale = 0.01f;
            }

            if(newTimeScale != 0f)
            {
                Time.timeScale = newTimeScale;
                // Adjust fixed delta time according to timescale
                // The fixed delta time will now be 0.02 frames per real-time second
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
        }

        if(this.fpsEnabled)
        {
            float mouseSensitivity = 100f;

            //Add FPS camera rotation (independent of timescale)
            this.rotX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.unscaledDeltaTime;
            this.rotY += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.unscaledDeltaTime;
            this.rotY = Mathf.Clamp(this.rotY, -90f, 90f);

            foreach (Camera camera in Camera.allCameras)
            {
                // Only move the right camera (used in ADV mode) and character camera (used in somniums)
                if (cameraMightBeActiveCamera(camera) || camera.name == "Character Camera")
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

                    //Add FPS camera movement (independent of timescale)
                    Vector3 moveDir = default(Vector3);
                    float speed = (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) ? 20f : 1.5f;
                    speed *= Time.unscaledDeltaTime;

                    //Move right and forward relative to the camera
                    if (Input.GetKey(KeyCode.P))
                    {
                        moveDir += speed * camera.transform.forward;
                    }
                    if (Input.GetKey(KeyCode.Semicolon))
                    {
                        moveDir -= speed * camera.transform.forward;
                    }
                    if (Input.GetKey(KeyCode.Quote))
                    {
                        moveDir += speed * camera.transform.right;
                    }
                    if (Input.GetKey(KeyCode.L))
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
                        //move camera itself, only if not using a customFreeLookCamera
                        if(customFreeLook == null)
                        {
                            // Each 'normal' camera has an overridePosition where translation is accumulated
                            camPosOverride += moveDir;
                            camera.transform.position = camPosOverride;
                        }
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.F9)) {
                this.fpsEnabled = false;

                if(customFreeLook != null)
                {
                    //Set the custom freelook camera to the lowest priority so it becomes disabled
                    customFreeLook.m_Priority = int.MinValue;
                    customFreeLook = null;
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

                //Set initial camera override position to the first "might be active camera" camera
                camPosOverride = new Vector3();
                foreach (Camera camera in Camera.allCameras)
                {
                    if(cameraMightBeActiveCamera(camera))
                    {
                        camPosOverride = camera.transform.position;
                        break;
                    }
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
}
