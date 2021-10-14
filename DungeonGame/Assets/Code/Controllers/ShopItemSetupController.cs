using UnityEngine;
using UnityEngine.UI;

public class ShopItemSetupController : MonoBehaviour
{
    public TMPro.TMP_Text Title, Info, Price;
    public Image Icon;

    public void Setup(ItemBase item)
    {
        Title.text = item.Title;
        Info.text = item.Info;
        Price.text = item.Price.ToString();
        Icon.sprite = item.Icon;
        Destroy(this);
    }
}
