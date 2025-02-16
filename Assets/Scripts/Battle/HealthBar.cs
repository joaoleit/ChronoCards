using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject floatingTextPrefab;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        int diff = (int)slider.value - health;
        slider.value = health;

        if (diff > 0)
        {
            InstantiateFloatingText("-" + diff, Color.red);
            return;
        }
        if (diff < 0)
        {
            InstantiateFloatingText("+" + (diff * -1), Color.green);
            return;
        }
        InstantiateFloatingText("0", Color.gray);
    }

    private void InstantiateFloatingText(string text, Color color)
    {
        if (floatingTextPrefab)
        {
            RectTransform rect = GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            rect.GetWorldCorners(corners);
            GameObject textObj = Instantiate(floatingTextPrefab, corners[2], Quaternion.identity, rect.parent);
            textObj.GetComponent<FloatingText>().SetText(text, color);
        }
    }

}
