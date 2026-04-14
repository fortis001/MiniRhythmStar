using MyGame.Common.DataFormat;
using MyGame.Core.Managers;
using MyGame.UI.Components;
using UnityEngine;

public class BaseUIManager : MonoBehaviour
{
    [SerializeField] private SimpleBtn[] _commonBtns; // 공통 사운드 배정 버튼들

    public virtual void Init()
    {
        foreach (var btn in _commonBtns)
        {
            btn.OnHover += () => SoundManager.Instance.PlaySFX(SFXID.UI_Btn_Hover);
            btn.OnClick += () => SoundManager.Instance.PlaySFX(SFXID.UI_Btn_Click);
        }
    }
}
