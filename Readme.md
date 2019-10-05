## Developer's Instructions

- Download DnSpy and extractt it somewhere
- Navigate to the `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed folder`
- Open the `Assembly-CSharp.dll` with dnspy, **while it's in the same folder as all the other DLLs**. If it's not in the same folder as the other DLLS, dnspy won't be able to find them.
- Navigate to the `InputProc` class
- Replace the `Update()` function of the `InputProc` class with the below (In Unity, `Update()` is called once every game tick)

## User's instructions

- Download the modded `Assembly-CSharp.dll`, and replace your game DLL ( in `AI The Somnium Files\AI_TheSomniumFiles_Data\Managed folder`). Remember to keep a backup of your original `.dll`
- Use the y, h, g, j, t, u keys to move along the axis. Use the shift key to move faster

So far I have added a x/y/z movement keys, with shift to increase speed.

```csharp
private void Update()
{
    float x = 0f;
    float y = 0f;
    float z = 0f;
    float multiplier = Input.GetKey(KeyCode.LeftShift) ? 0.5f : 0.05f;
    if (Input.GetKey(KeyCode.Y))
    {
        x = 1f;
    }
    if (Input.GetKey(KeyCode.H))
    {
        x = -1f;
    }
    if (Input.GetKey(KeyCode.G))
    {
        y = 1f;
    }
    if (Input.GetKey(KeyCode.J))
    {
        y = -1f;
    }
    if (Input.GetKey(KeyCode.T))
    {
        z = 1f;
    }
    if (Input.GetKey(KeyCode.U))
    {
        z = -1f;
    }
    x *= multiplier;
    y *= multiplier;
    z *= multiplier;
    foreach (Camera camera in Camera.allCameras)
    {
        camera.transform.position = new Vector3(camera.transform.position.x + x, camera.transform.position.y + y, camera.transform.position.z + z);
    }
    this.proc();
    if (this.onUpdate != null)
    {
        this.onUpdate.call();
    }
}
```

## TODO

- Make movement aligned with camera direction (currently moves along cartesian axes)
- Unlock camera rotation, or add camera rotation button (rotation currently limited by game)

## Note

- I want to use the "inputaxis" function, but I'm not sure what they named the axis. I haven't tried the default values, maybe that's worth a try?
