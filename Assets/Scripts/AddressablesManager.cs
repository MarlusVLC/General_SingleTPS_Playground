using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

[Serializable]
public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudioClip(string guid) : base(guid) { }
}
public class AddressablesManager : MonoBehaviour
{
    [SerializeField] private AssetReference _playerArmatureAssetReference;
    [SerializeField] private AssetReferenceTexture2D _gameLogoAssetReference;
    [SerializeField] private AssetReferenceAudioClip _entryAudioAssetReference;
    
    [Space]
    [SerializeField] private CinemachineCamera _mainCamera;
    
    [Space]
    [SerializeField] RawImage _gameLogoRawImage;

    private GameObject playerArmature;

    void Start()
    {
        Debug.Log("Initializing addressables");
        Addressables.InitializeAsync().Completed += AssetSetup;
    }

    private void AssetSetup(AsyncOperationHandle<IResourceLocator> obj)
    {
        Debug.Log("Addressables initialized");
        _playerArmatureAssetReference.InstantiateAsync().Completed += (go) =>
        {
            playerArmature = go.Result;
            var cameraRoot = playerArmature.transform.Find("PlayerCameraRoot");
            _mainCamera.Follow = cameraRoot;
            _mainCamera.LookAt = cameraRoot;
            
            Debug.Log("PLAYER ARMATURE instantiated");
        };

        _entryAudioAssetReference.LoadAssetAsync<AudioClip>().Completed += (clip) =>
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip.Result;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.Play();
            Debug.Log("AUDIO CLIP loaded");
        };

        _gameLogoAssetReference.LoadAssetAsync<Texture2D>().Completed += (texture) =>
        {
            Debug.Log("GAME LOGO loaded 2");
        };
    }

    private void Update()
    {
        if (_gameLogoAssetReference.Asset != null && _gameLogoRawImage.texture == null)
        {
            _gameLogoRawImage.texture = _gameLogoAssetReference.Asset as Texture2D;
            Color currentColor = _gameLogoRawImage.color;
            currentColor.a = 1;
            _gameLogoRawImage.color = currentColor;
            Debug.Log("GAME LOGO loaded");
        }
    }

    void OnDestroy()
    {
        _playerArmatureAssetReference.ReleaseInstance(playerArmature);
        Debug.Log($"Has PLAYER ARMATURE been released = {!_playerArmatureAssetReference.IsValid()}");
        _gameLogoAssetReference.ReleaseAsset();
        Debug.Log($"Has GAME LOGO been released = {!_gameLogoAssetReference.IsValid()}");

    }
}
