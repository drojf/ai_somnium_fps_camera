// This is a source code fragment - you must merge it into existing code using DnSpy.
// Please read the Readme.md for more instruction

//Merge this line into existing includes
using Cinemachine;

//Merge into existing InputProc class
class InputProc
{
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
    Vector3 camPosOverride;

    bool cameraMightBeActiveCamera(Camera camera) {
        return !camera.name.Contains("UICamera") && !camera.name.Contains("Button") && !camera.name.Contains("Background") && camera.name != "Camera";
    }

    // Game.InputProc
    private void LateUpdate()
    {
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
