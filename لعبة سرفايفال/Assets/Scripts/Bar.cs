using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
  
    private Slider barSlider;
    public float percentage = 100;
    

    private void Start()
    {
        barSlider = GetComponent<Slider>();
    }
    private void UpdateVisualBar(float percentage)
    {
        barSlider.value = percentage / 100;
    }
    public void IncreaseBar(float value)
    {
        percentage = Mathf.Clamp(percentage + value, 0, 100);
        UpdateVisualBar(percentage);
    }

    public void DeceraseBar(float value)
    {
        percentage = Mathf.Clamp(percentage - value, 0, 100);
        UpdateVisualBar(percentage);
    }


}
