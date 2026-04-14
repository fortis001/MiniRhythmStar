using MyGame.Common.Constants;
using MyGame.Common.Enums;
using MyGame.Core.Managers;


namespace MyGame.UI.LevelSelect
{
    public class LevelUIManager : BaseUIManager
    {
        public override void Init()
        {
            base.Init();
        }

        public void OnClickLevelBtn(int levelValue)
        {
            Level selectedLevel = (Level)levelValue;
            GameManager.Instance.SetLevel(selectedLevel);

            TransitionManager.Instance.LoadNextScene(SceneNames.SongSelect);
        }

        public void OnClickReturnBtn()
        {
            TransitionManager.Instance.LoadNextScene(SceneNames.Title);
        }

    }
}

