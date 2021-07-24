using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
    public class IAPService : IIAPService
    {
        private readonly IAPProvider _iapProvider;
        private readonly IPersistentProgressService _progressService;

        public bool IsInitialized => _iapProvider.IsInitialized;
        public event Action Initialized;

        public IAPService(IAPProvider iapProvider, IPersistentProgressService progressService)
        {
            _iapProvider = iapProvider;
            _progressService = progressService;
        }

        public void Initialize()
        {
            _iapProvider.Initialize(this);
            _iapProvider.Initialized += () => Initialized?.Invoke();
        }

        public List<ProductDescription> Products() => 
            ProductsDescription().ToList();

        public void StartPurchase(string productId) => 
            _iapProvider.StartPurchase(productId);

        public PurchaseProcessingResult ProcessingResult(Product purchaseProduct)
        {
            ProductConfig productConfig = _iapProvider.Configs[purchaseProduct.definition.id];

            switch (productConfig.ItemType)
            {
                case ItemType.Skull:
                    _progressService.Progress.WorldData.LootData.Add(productConfig.Quantity);
                    _progressService.Progress.PurchaseData.AddPurchase(purchaseProduct.definition.id);
                    break;
            }

            return PurchaseProcessingResult.Complete;
        }

        private IEnumerable<ProductDescription> ProductsDescription()
        {
            PurchaseData purchaseData = _progressService.Progress.PurchaseData;

            foreach (string productId in _iapProvider.Products.Keys)
            {
                ProductConfig config = _iapProvider.Configs[productId];
                Product product = _iapProvider.Products[productId];

                BoughtIAP boughtIAP = purchaseData.BoughtIaps.Find(x => x.IAPid == productId);

                if (ProductBoughtOut(boughtIAP, config))
                {
                    continue;
                }

                yield return new ProductDescription
                {
                    Id = productId,
                    Config = config,
                    Product = product,
                    AvailablePurchasesLeft = boughtIAP != null
                        ? config.MaxPurchaseCount - boughtIAP.Count
                        : config.MaxPurchaseCount
                };
            }
        }

        private static bool ProductBoughtOut(BoughtIAP boughtIAP, ProductConfig config) => 
            boughtIAP != null && boughtIAP.Count >= config.MaxPurchaseCount;
    }
}