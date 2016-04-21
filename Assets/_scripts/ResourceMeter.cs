using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourceMeter : MonoBehaviour {
    public Image slider, background;
    public float max, animationSpeed, danger;
    public Color mainColour, dangerColour;

    private float value;

    void Start() {
        mainColour = background.color;
    }

    public void UpdateMeter(float newValue) {
        value = newValue;
    }

    void Update() {
        slider.fillAmount = Mathf.Lerp(slider.fillAmount, value / max, animationSpeed * Time.deltaTime);

        if (value <= danger)
            background.color = Color.Lerp(mainColour, dangerColour, Mathf.PingPong(2f*Time.time, 1f));
        else
            background.color = mainColour;
    }


}
