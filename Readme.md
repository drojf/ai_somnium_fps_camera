
## User's instructions

- Go to the [Releases Page](https://github.com/drojf/ai_somnium_freecam/releases)
- Download the latest modded `Assembly-CSharp.dll`, and replace your game DLL (in `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed folder`). Remember to keep a backup of your original `.dll`
- Use the arrow keys to move the camera. Hold the shift key to move faster. Use the mouse to rotate the camera

So far I have added a x/y/z movement keys, with shift to increase speed.

## Developer's Instructions

- Download DnSpy and extractt it somewhere
- Navigate to the `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed folder`
- Open the `Assembly-CSharp.dll` with dnspy, **while it's in the same folder as all the other DLLs**. If it's not in the same folder as the other DLLS, dnspy won't be able to find them.
- Expand the `Assembly-CSharp.dll` arrow
- Expand the `Game` arrow
- Navigate to the `InputProc` class
- Replace the `Update()` function of the `InputProc` class with the below code (In Unity, `Update()` is called once every game tick)
- Save the module. (You should enable "save extra metadata" or else the variable names will be lost in the saved DLLs?).

### Source Code for Update() function of InputProc class

```csharp
// Game.InputProc
private void LateUpdate()
{
    float mouseSensitivity = 10f;
    foreach (Camera camera in Camera.allCameras)
    {
        //Add FPS camera rotation
        this.cumX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        this.cumY += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        this.cumY = Mathf.Clamp(this.cumY, -90f, 90f);
        camera.transform.eulerAngles = new Vector3(-this.cumY, this.cumX, 0f);

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
        camera.transform.position = camera.transform.position + moveDir;
    }
}
```

## TODO

- Unlock camera rotation, or add camera rotation button (rotation currently limited by game)
  - Override the default rotation with `CinemachineCustomAxis.GetAxisCustom()`?

## Notes

- I want to use the "inputaxis" function, but I'm not sure what they named the axis. I haven't tried the default values, maybe that's worth a try?
- The axis (for the mouse X/Y) appear to be the default "Mouse X" and "Mouse Y", as seen in the `CinemachineCustomAxis.GetAxisCustom()` function.


## Resources Used

- https://www.unknowncheats.me/forum/unity/285864-beginners-guide-hacking-unity-games.html
- DnSpy

### Unity Pages

https://docs.unity3d.com/ScriptReference/Camera-allCameras.html