using UnityEngine;

public class LightSwitch : Interactable
{

    public Light m_light;

    public bool isOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        UpdateLight();
    }

    public override string Description(){
        if(isOn) return "Press {E} to interact.";
        return "Press {E} to interact.";
    }
    
    void UpdateLight(){
        m_light.enabled = isOn;
    }    

    public override void Interact(){
        isOn = !isOn;
        UpdateLight();
    }
}
