using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject shopCanvas;
    public Transform upgradesContainer;
    public GameObject upgradeButtonPrefab;
    public GameObject upgradeGrid;
    private UpgradeManager upgradeManager;

    void Start()
    {
        upgradeGrid.SetActive(false);
        upgradeManager = upgradeGrid.GetComponent<UpgradeManager>();
        shopCanvas.SetActive(false);
    }

    public void OpenShop()
    {
        BlockingUI.SetBlocking(true);

        foreach (Transform child in upgradesContainer)
        {
            Destroy(child.gameObject);
        }

        CreateUpgradeButton("Forge cards", () => HandleUpgrade(UpgradeType.Forge));
        CreateUpgradeButton("Boost cards +1", () => HandleUpgrade(UpgradeType.Boost));
        CreateUpgradeButton("Cards cost -1", () => HandleUpgrade(UpgradeType.ManaCost));
        CreateUpgradeButton("Heal all damage taken", () => HandleUpgrade(UpgradeType.Heal));
        CreateUpgradeButton("Maximum health +10%", () => HandleUpgrade(UpgradeType.IncreaseHealth));
        CreateUpgradeButton("Maximum mana +1", () => HandleUpgrade(UpgradeType.IncreaseMaxMana));
        CreateUpgradeButton("Starting mana +1", () => HandleUpgrade(UpgradeType.IncreaseStartMana));
        CreateUpgradeButton("End turn draws +1 card", () => HandleUpgrade(UpgradeType.IncreaseDraw));

        shopCanvas.SetActive(true);
    }

    void CreateUpgradeButton(string upgradeName, UnityEngine.Events.UnityAction action)
    {
        GameObject button = Instantiate(upgradeButtonPrefab, upgradesContainer);
        button.GetComponentInChildren<TMP_Text>().fontSize = 20;
        button.GetComponentInChildren<TMP_Text>().text = upgradeName;
        button.GetComponentInChildren<Button>().onClick.AddListener(action);
    }

    void HandleUpgrade(UpgradeType upgradeType)
    {
        upgradeGrid.SetActive(true);
        upgradeManager.setUpgrade(upgradeType);
        CloseShop();
    }

    public void CancelUpgrade()
    {

        upgradeManager.ReturnCardsToOriginalSlots(upgradeManager.GetInvolvedCards());
        upgradeManager.ClearUpgradeSlots();
        upgradeGrid.SetActive(false);
        shopCanvas.SetActive(false);
        BlockingUI.SetBlocking(false);
    }

    public void CloseShop()
    {
        shopCanvas.SetActive(false);
        BlockingUI.SetBlocking(false);
    }
}

public enum UpgradeType
{
    Forge,
    Boost,
    ManaCost,
    Heal,
    IncreaseHealth,
    IncreaseMaxMana,
    IncreaseStartMana,
    IncreaseDraw
}