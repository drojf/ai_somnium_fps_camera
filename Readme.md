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
