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

    struct CameraClipState {
        public float near;
        public float far;
        public float fieldOfView;
    }

    bool custom_clip;
    bool immediateGUIHidden;
    float rotX;
    float rotY;
    bool fpsEnabled;
    float userFov;

    //Used to save/restore the clip settings of the GUI cameras
    Dictionary<Camera, CameraClipState> backupClipState;
    //Used to save/restore the "enabled" state of the GUI cameras
    Dictionary<Camera, Vector3> backupGUICameraState;
    //Used to save the position/rotation of each 'normal' camera
    Dictionary<Camera, CameraBackupState> backupCameraPositions;
    //Used to save whether collision is enabled/disabled of each cinemachine collider (which is attached to a cinemachine camera)
    Dictionary<CinemachineCollider, bool> backupCollisionStates;
    GameObject cube;
    CinemachineFreeLook customFreeLook;
    Vector3 camPosOverride;

    private void SetTimescale(float newTimeScale) {
        Time.timeScale = newTimeScale;
        // Adjust fixed delta time according to timescale
        // The fixed delta time will now be 0.02 frames per real-time second
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

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

            GUI.Box(backgroundArea, "Keyboard Shortcuts");

            GUILayout.BeginArea(contentArea);

            GUILayout.Label(
@"Press F10 & F11 to toggle the GUIs! (for taking screenshots)

Movement Controls:
F8 - Enable Noclip/FPS mode
F9 - Disable Noclip/FPS mode
P and ; - Move forward and back (hold SHIFT to move faster)
L  and ' - Move left and right (hold SHIFT to move faster)
SHIFT - Hold to move faster
Mouse Rotation - Rotate the camera (while in FPS mode)

Screenshot Controls:
F11 - Toggle this window
F10 - Toggle Game GUI
F7 - Fix Camera Zoom and Clip distance (get closer without clipping)
Scroll Wheel - Adjust Camera Zoom (when enabled with F7)

Slow Motion/Pause Controls:
F2 - Play at normal speed/Resume Game
F3 - 10x Slow Motion (note: doesn't always work)
F4 - Freeze/Pause the game entirely (resume with F2)

Rarely Used Controls:
F6 - Enable Noclip/FPS Mode with Magenta Box

Press F10 & F11 to toggle the GUIs! (for taking screenshots)

https://github.com/drojf/ai_somnium_fps_camera
");
            GUILayout.Label($"Custom Zoom/Fov Enabled: {backupClipState != null} (Press F7)");
            if(backupClipState != null)
            {
                GUILayout.Label($"User Fov: {userFov}");
            }

            GUILayout.EndArea ();
        }
    }

    bool cameraMightBeActiveCamera(Camera camera) {
        return !camera.name.Contains("UICamera") && !camera.name.Contains("Button") && !camera.name.Contains("Background") && camera.name != "Camera";
    }

    void forceGraphicAlpha(Graphic graphic, float newAlpha) {
        Color color = graphic.canvasRenderer.GetColor();
        color.a = newAlpha;
        graphic.canvasRenderer.SetColor(color);
    }

    // Game.InputProc
    private void LateUpdate()
    {
        if(userFov == 0f)
        {
            userFov = 45f;
        }

        if(Input.mouseScrollDelta.y > 0f)
        {
            userFov *= 1.1f;
        }
        else if(Input.mouseScrollDelta.y < 0f)
        {
            userFov *= .9f;
        }

        // Prevent fov getting too small or too large
        userFov = Mathf.Clamp(userFov, 1f, 180f);

        if(Input.GetKeyDown(KeyCode.F7))
        {
            userFov = 45f;
            if(backupClipState == null)
            {
                backupClipState = new Dictionary<Camera, CameraClipState>();
            }
            else
            {
                // Restore clip settings
                foreach(KeyValuePair<Camera, CameraClipState> kvp in backupClipState)
                {
                    kvp.Key.near = kvp.Value.near;
                    kvp.Key.far = kvp.Value.far;
                    kvp.Key.fieldOfView = kvp.Value.fieldOfView;
                }
                backupClipState = null;
            }
        }

        if(backupClipState != null)
        {
            foreach (Camera camera in Camera.allCameras)
            {
                // If a new camera is found, backup clip settings, then force clip plane to .1 and FOV to 60 degrees
                if(!backupClipState.ContainsKey(camera))
                {
                    backupClipState[camera] = new CameraClipState() {
                        near = camera.near,
                        far = camera.far,
                        fieldOfView = camera.fieldOfView,
                    };
                }
                
                camera.near = .1f;
                camera.fieldOfView = userFov;
            }
        }

        // Press F11 to toggle the keyboard shortcuts window
        if(Input.GetKeyDown(KeyCode.F11))
        {
            immediateGUIHidden = !immediateGUIHidden;
        }

        // Press F10 to toggle GUI on/off
        if(Input.GetKeyDown(KeyCode.F10))
        {
            if(backupGUICameraState == null)
            {
                // This can be replaced with a bool later
                backupGUICameraState = new Dictionary<Camera, Vector3>();
            }
            else
            {
                // Set all "UnityEngine.UI.Graphic" objects not named "Image" to 1 alpha
                // This doesn't properly restore the UI's alpha value (if it was not originally '1') but should be good enough
                foreach(Graphic graphic in UnityEngine.Object.FindObjectsOfType(typeof(Graphic)))
                {
                    if (graphic.name != "Image")
                    {
                        forceGraphicAlpha(graphic, 1f);
                    }
                }

                backupGUICameraState = null;
            }
        }

        // Every keyframe, rotate the bustshot camera way out of the way so the character isn't rendered
        // We can't disable the camera, as the game will just spawn a new camera if it detects the normal camera is disabled.
        foreach (Camera camera2 in Camera.allCameras)
        {
            if (camera2.name == "Camera")
            {
                camera2.transform.rotation = ((this.backupGUICameraState == null) ? 
                new Quaternion(0f, -1f, 0f, 0f) : // This is the normal position, camera pointing forwards
                new Quaternion(0f, 0f, -1f, 0f)); // This disables the camera by pointing the camera backwards
            }
        }

        // This must be done every frame (or periodically) in case you hover over an item in ADV mode which causes a new UI widget to spawn
        //For all UI elements except those named "Image":
        // - Backup the UI color values
        // - Set 0 alpha
        if(backupGUICameraState != null)
        {
            foreach(Graphic graphic in UnityEngine.Object.FindObjectsOfType(typeof(Graphic)))
            {
                // Set graphics not named "Image" to 0 alpha
                if (graphic.name != "Image")
                {
                    forceGraphicAlpha(graphic, 0f);
                }
            }
        }

        // Use F2-4 for slowmo: F2 = normal, F3 = 10x slower, F4 = toggle stop time
        // see https://docs.unity3d.com/ScriptReference/Time-timeScale.html)
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                SetTimescale(1f);
                foreach(RootNode rootNode in UnityEngine.Object.FindObjectsOfType<RootNode>())
                {
                    rootNode.UnPause();
                }
            }
            else if(Input.GetKeyDown(KeyCode.F3))
            {
                SetTimescale(.1f);
                foreach(RootNode rootNode in UnityEngine.Object.FindObjectsOfType<RootNode>())
                {
                    rootNode.UnPause();
                }
            }
            else if(Input.GetKeyDown(KeyCode.F4))
            {
                SetTimescale(0);
                foreach(RootNode rootNode in UnityEngine.Object.FindObjectsOfType<RootNode>())
                {
                    rootNode.ModPause();
                }
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
                foreach(KeyValuePair<Camera, CameraBackupState> kvp in backupCameraPositions)
                {
                    kvp.Key.transform.position = kvp.Value.position;
                    kvp.Key.transform.eulerAngles = kvp.Value.eulerAngles;
                }

                // Restore collider states
                foreach(KeyValuePair<CinemachineCollider, bool> kvp in backupCollisionStates)
                {
                    kvp.Key.m_AvoidObstacles = kvp.Value;
                }

                // Clear backups
                this.backupCameraPositions.Clear();
                backupCollisionStates.Clear();
            }
        }
        else {
            if(Input.GetKeyDown(KeyCode.F6) | Input.GetKeyDown(KeyCode.F8))
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
