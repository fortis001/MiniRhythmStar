
namespace MyGame.Common.DataFormat
{
    public enum SFXID
    {
        None = 0,

        // ---  UI & Navigation ---
        UI_Btn_Click = 100,
        UI_Btn_Hover = 101,
        UI_Btn_Confirm = 102,
        UI_Menu_Back = 110,
        UI_Menu_Open = 111,
        UI_Error = 190,

        // ---  Game System ---
        Game_Start = 200,
        Game_CountDown = 201,
        Game_Clear_Win = 202,
        Game_Clear_Fail = 203,
        Game_Pause = 204,

        // ---  Note Judgement ---
        Note_Perfect = 300,
        Note_Great = 301,
        Note_Good = 302,
        Note_Miss = 303,
        Note_Combo_Break = 304,

        // ---  Special Effects ---
        FX_Fever_Ready = 400,
        FX_Fever_Active = 401,
        FX_Level_Up = 402,
    }

    public enum BGMID
    {
        None,

        // --- BGM ---
        BGM_Lobby = 100,
        BGM_LevelSelect = 101,
        BGM_SongSelect_Silent= 102,
        BGM_Game_Silent = 103,
        BGM_Editor_Silent = 104,
        BGM_Result = 105
    }
}

