using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows.Shop;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UIRoot";
        
        private readonly IAssets _asset;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IAdsService _adService;

        private Transform _uiRoot;

        public UIFactory(IAssets asset, IStaticDataService staticData, IPersistentProgressService progressService, IAdsService adService)
        {
            _asset = asset;
            _staticData = staticData;
            _progressService = progressService;
            _adService = adService;
        }
        
        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            ShopWindow window = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
            window.Construct(_adService, _progressService);
        }

        public async Task CreateUIRoot()
        {
            GameObject root = await _asset.Instantiate(UIRootPath);
            _uiRoot = root.transform;
        }
    }
}