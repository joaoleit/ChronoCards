using UnityEngine;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance { get; private set; }
    [SerializeField]
    private GameObject _prefab;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void InstantiatePopUp(string text)
    {
        var popUp = Instantiate(_prefab, transform);
        popUp.GetComponentInChildren<TMP_Text>().text = text;
    }
}