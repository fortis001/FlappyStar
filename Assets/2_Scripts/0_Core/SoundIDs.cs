using LSH.Core;

namespace FlappyStar.Core
{
    [SoundIdEnum(SoundType.SFX)]
    public enum SFXID
    {
        None = 0,

        UI_Btn_Click = 100,
        UI_Btn_Hover = 101,
        UI_Btn_Confirm = 102,

        Player_Jump = 200,
        Player_Hit = 201,

        Gameplay_GameOver = 300,
        GamePlay_Crowd1 = 310,
        GamePlay_Crowd2 = 311,
        GamePlay_Crowd3 = 312,
        GamePlay_Switch = 320,

    }

    [SoundIdEnum(SoundType.BGM)]
    public enum BGMID
    {
        None = 0,

        Title = 100,
        InGame = 200,
    }
}