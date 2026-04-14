using MyGame.Common.Constants;
using MyGame.Common.DataFormat;
using MyGame.Core.Managers;
using MyGame.UI.Components;
using UnityEngine;


namespace MyGame.UI.Title
{
    public class TitleUIManager : BaseUIManager
    {

        public override void Init()
        {
            base.Init();
        }

        public void OnClickStartBtn()
        {
            TransitionManager.Instance.LoadNextScene(SceneNames.LevelSelect);
        }

        public void OnClickEditorBtn()
        {
            TransitionManager.Instance.LoadNextScene(SceneNames.Editor);
        }

        public void OnClickExitBtn()
        {
            GameManager.Instance.ExitGame();
        }
    }
}

