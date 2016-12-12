using UnityEngine;
public class Lamp : Togglable {
    public Light lightObject;

    public float lightIntensity;
    public override void Start() {
        base.Start();
        UpdateLightState();
        baseTraction = 10;
    }
    protected override void Toggle() {
        base.Toggle();
        UpdateLightState();
    }

    private void UpdateLightState() {
        if (active) {
            
            lightObject.intensity = lightIntensity == 0 ? Config.Instance.LIGHT_INTENSITY : lightIntensity;
        } else {
            lightObject.intensity = 0.0f;
        }
    }
}