using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using EasyTransition;

public class TriggerInventoryScene : MonoBehaviour
{
    public string inventoryScene = "InventoryTesting";
    public PlayerController playerController;
    public TransitionSettings transition;
    public float startDelay;

    public void StartInventoryScene()
    {
        GameManager.Instance.OpenInventory();
        TransitionManager.Instance.Transition(inventoryScene, transition, startDelay);
    }
}