#if VCONTAINER
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Example3
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;

        private GameObject _playerInstance;
        private IObjectResolver _objectResolver;

        [Inject]
        private void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        //Difference from the video
        //Here we are using the built in Unity attribute when in 
        //the video I was using the Button attribute from the Odin plugin
        //To use it - click three points button of this component and find it there
        [ContextMenu("InstantiatePlayer")]
        private void InstantiatePlayer()
        {
            if (_playerInstance != null)
            {
                Debug.LogError("Already created!");
                return;
            }

            _playerInstance = _objectResolver.Instantiate(_playerPrefab,
                Vector3.up, Quaternion.identity);
        }
    }
}
#endif