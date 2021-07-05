using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsListener
    {
        private string _gameId;
        private string _rewardedVideoId;

        private const string AndroidGameId = "4204085";
        private const string IOSGameId = "4204084";

        private const string RewardedVideoPlacementAndroidId = "Rewarded_Android";
        private const string RewardedVideoPlacementIOSId = "Rewarded_iOS";

        public event Action RewardedVideoReady;
        private Action _onVideoFinished;

        public int Reward => 10;

        public void Initialize()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = AndroidGameId;
                    _rewardedVideoId = RewardedVideoPlacementAndroidId;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOSGameId;
                    _rewardedVideoId = RewardedVideoPlacementIOSId;
                    break;
                case RuntimePlatform.WindowsEditor:
                    _gameId = AndroidGameId;
                    _rewardedVideoId = RewardedVideoPlacementAndroidId;
                    break;
                default:
                    Debug.Log("Unsupported platform for ads");
                    break;
            }
            
            Advertisement.AddListener(this);
            Advertisement.Initialize(_gameId);
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            Advertisement.Show(_rewardedVideoId);
            _onVideoFinished = onVideoFinished;
        }

        public bool IsRewardedVideoReady() => 
            Advertisement.IsReady(_rewardedVideoId);

        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log($"OnUnityAdsReady {placementId}");

            if (placementId == _rewardedVideoId) 
                RewardedVideoReady?.Invoke();
        }

        public void OnUnityAdsDidError(string message) => 
            Debug.Log($"OnUnityAdsDidError {message}");

        public void OnUnityAdsDidStart(string placementId) => 
            Debug.Log($"OnUnityAdsDidStart {placementId}");

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            switch (showResult)
            {
                case ShowResult.Failed:
                    Debug.Log($"OnUnityAdsDidFinish {placementId}");
                    break;
                case ShowResult.Skipped:
                    Debug.Log($"OnUnityAdsDidFinish {placementId}");
                    break;
                case ShowResult.Finished:
                    _onVideoFinished?.Invoke();
                    break;
                default:
                    Debug.Log($"OnUnityAdsDidFinish {placementId}");
                    break;
            }

            _onVideoFinished = null;
        }
    }
}