using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.IAP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopItem : MonoBehaviour
    {
        public Button BuyItemButton;
        public TextMeshProUGUI PriceText;
        public TextMeshProUGUI QuantityText;
        public TextMeshProUGUI AvailableItemsLeftText;
        public Image Icon;

        private IIAPService _iapService;
        private IAssets _asset;
        private ProductDescription _productDescription;

        public void Construct(
            IIAPService iapService, 
            IAssets asset, 
            ProductDescription productDescription)
        {
            _iapService = iapService;
            _asset = asset;
            
            _productDescription = productDescription;
        }

        public async void Initialize()
        {
            BuyItemButton.onClick.AddListener(OnBuyItemClick);

            PriceText.text = _productDescription.Config.Price;
            QuantityText.text = _productDescription.Config.Quantity.ToString();
            AvailableItemsLeftText.text = _productDescription.AvailablePurchasesLeft.ToString();
            Icon.sprite = await _asset.Load<Sprite>(_productDescription.Config.Icon);
        }

        private void OnBuyItemClick() => 
            _iapService.StartPurchase(_productDescription.Id);
    }
}