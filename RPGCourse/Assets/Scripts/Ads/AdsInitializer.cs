using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private bool _testMode = true;
    [SerializeField] private string _iOSGameId = "4548661";
    [SerializeField] private string _androidGmeId = "4548661";

    private string _gameId;

    private void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGmeId;

        Advertisement.Initialize(_gameId, _testMode);
    }

    public void OnInitializationComplete()
    {
        throw new System.NotImplementedException();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        throw new System.NotImplementedException();
    }
}
