using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
    
    [SerializeField] private CinemachineCamera _mainCamera;
    
    private RawImage _gameLogoRawImage;
    
    
    
}
