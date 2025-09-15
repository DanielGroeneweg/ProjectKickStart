using UnityEngine;
using TMPro;
public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text display;
    [SerializeField] private Inventory inventory;
    private void OnEnable()
    {
        UpdateMoney();
    }
    public void UpdateMoney()
    {
        display.text = $"Money: {inventory.coins}";
    }
}
