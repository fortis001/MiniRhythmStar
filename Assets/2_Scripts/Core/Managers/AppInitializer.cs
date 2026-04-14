using System.Collections;
using MyGame.Common.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MyGame.Core.Managers
{
    public class AppInitializer : MonoBehaviour
    {
        [SerializeField] GameManager _gameManager;
        [SerializeField] SoundManager _soundManager;
        [SerializeField] TransitionManager _transitionManager;
        [SerializeField] UserDataManager _userDataManager;
        [SerializeField] SongDatabaseManager _songDatabase;
        [SerializeField] InputManager _inputManager;


        private IEnumerator Start()
        {
            _gameManager.Init();
            _soundManager.Init();
            _transitionManager.Init();
            _userDataManager.Init();
            _songDatabase.Init();
            _inputManager.Init();




            yield return new WaitForSeconds(0.2f);


            SceneManager.LoadScene(SceneNames.Title);
        }
    }
}

