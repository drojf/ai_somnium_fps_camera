## Developer's Instructions

- Download DnSpy and extractt it somewhere
- Navigate to the `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed folder`
- Open the `Assembly-CSharp.dll` with dnspy, **while it's in the same folder as all the other DLLs**. If it's not in the same folder as the other DLLS, dnspy won't be able to find them.
- Expand the `Assembly-CSharp.dll` arrow
- Expand the `Game` arrow
- Navigate to the `InputProc` class
- Replace the `Update()` function of the `InputProc` class with the below (In Unity, `Update()` is called once every game tick)
- Save the module. (You should enable "save extra metadata" or else the variable names will be lost in the saved DLLs?).

## User's instructions

- Download the modded `Assembly-CSharp.dll`, and replace your game DLL ( in `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed folder`). Remember to keep a backup of your original `.dll`
- Use the y, h, g, j, t, u keys to move along the axis. Use the shift key to move faster

So far I have added a x/y/z movement keys, with shift to increase speed.

```csharp
private void Update()
{
    // Move all active cameras, since I don't know which is the currently active camera
    foreach (Camera camera in Camera.allCameras)
    {
        Vector3 moveDir = default(Vector3);
        float speed = Input.GetKey(KeyCode.LeftShift) ? 0.5f : 0.02f;
        //Move right and forward relative to the camera
        if (Input.GetKey(KeyCode.Y))
        {
            moveDir += speed * camera.transform.forward;
        }
        if (Input.GetKey(KeyCode.H))
        {
            moveDir -= speed * camera.transform.forward;
        }
        if (Input.GetKey(KeyCode.J))
        {
            moveDir += speed * camera.transform.right;
        }
        if (Input.GetKey(KeyCode.G))
        {
            moveDir -= speed * camera.transform.right;
        }
        //Always move up/down the Z axis
        if (Input.GetKey(KeyCode.T))
        {
            moveDir += speed * Vector3.up;
        }
        if (Input.GetKey(KeyCode.U))
        {
            moveDir -= speed * Vector3.up;
        }
        camera.transform.position = camera.transform.position + moveDir;
    }

    // Existing game code - don't touch this!
    this.proc();
    if (this.onUpdate != null)
    {
        this.onUpdate.call();
    }
    // End Existing game code
}
```

## TODO

- Unlock camera rotation, or add camera rotation button (rotation currently limited by game)

## Note

- I want to use the "inputaxis" function, but I'm not sure what they named the axis. I haven't tried the default values, maybe that's worth a try?

## Resources Used

- https://www.unknowncheats.me/forum/unity/285864-beginners-guide-hacking-unity-games.html
- DnSpy
