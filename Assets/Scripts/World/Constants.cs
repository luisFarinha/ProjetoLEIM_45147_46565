using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    //PLAYER-----------------------------------------------------------
    public const string PLAYER_OBJECT = "Character";
    
    //PLayer Animations
    public const string PLAYER_IDLE = "player_idle";
    public const string PLAYER_DASH = "player_dash";
    public const string PLAYER_RUN = "player_run";
    public const string PLAYER_JUMP = "player_jump";
    public const string PLAYER_DOUBLEJUMP = "player_doubleJump";
    public const string PLAYER_WALLSLIDE = "player_wallSlide";
    public const string PLAYER_STARTFALL = "player_startFall";
    public const string PLAYER_FALL = "player_fall";
    public const string PLAYER_GLIDE = "player_glide";
    public const string PLAYER_LAND = "player_land";
    public const string PLAYER_ATTACK = "player_attack";
    public const string PLAYER_ATTACKUP = "player_attackUp";
    public const string PLAYER_ATTACKDOWN = "player_attackDown";

    public enum ActionType
    {
        ATTAKING,
        LANDING,
        KNOCKED,
        STUNNED
    }

    //Particles
    public const string DUST = "Dust";
    public const string LANDING_DIRT = "LandingDirt";
    public const string DASHING_DUST = "DashingDust";
    public const string DASHING_SHINE = "DashingShine";
    public const string WALL_SLIDING_DUST = "WallSlidingDust";
    public const string WALL_SLIDING_DIRT = "WallSlidingDirt";
    public const string DOUBLE_JUMP_SHINE = "DoubleJumpShine";


    //UI---------------------------------------------------------------

    //Unlockable Menu Settings
    public const string GLIDE_TITLE = "Glide";
    public const string DASH_TITLE = "Dash";
    public const string WALL_JUMP_TITLE = "Wall Jump";
    public const string DOUBLE_JUMP_TITLE = "Double Jump";

    public const string GLIDE_KEYBIND = "Hold 'L' or 'Left Trigger'";
    public const string DASH_KEYBIND = "Press 'Shift' or 'Right Trigger'";
    public const string WALL_JUMP_KEYBIND = "Press 'Space' or 'South Button' while wall sliding";
    public const string DOUBLE_JUMP_KEYBIND = "Press 'Space' or 'South Button' after jumping";

    public const string GLIDE_SPRITE = "Glide1";
    public const string DASH_SPRITE = "Dash5";
    public const string WALL_JUMP_SPRITE = "WallSlide4";
    public const string DOUBLE_JUMP_SPRITE = "Land2";

    public const string GLIDE_DESCRIPTION = "It eases the fall.";
    public const string DASH_DESCRIPTION = "Move much faster for a short period of time.";
    public const string WALL_JUMP_DESCRIPTION = "Allows you to bounce off of walls.";
    public const string DOUBLE_JUMP_DESCRIPTION = "Provides a second push upwards.";

    public const string GLIDE_LORE = "Reliable enough, for braves and fools willing to take the risk";
    public const string DASH_LORE = "Become the wind";
    public const string WALL_JUMP_LORE = "Get your grip on!";
    public const string DOUBLE_JUMP_LORE = "Defy all logic, break the game ;)";

    public const string UNLOCKABLES_EXIT = "Press 'E' or 'North Button' to continue";

    //Unlockable Animations
    public const string UNLOCK_IDLE = "unlockScreen_idle";
    public const string UNLOCK_FADE_IN = "unlockScreen_fadeIn";
    public const string UNLOCK_FADE_OUT = "unlockScreen_fadeOut";

    //Scene Transition Animations
    public const string SCENE_TRANSITION_FADE_IN = "sceneTransition_fadeIn";
    public const string SCENE_TRANSITION_FADE_OUT = "sceneTransition_fadeOut";

    public enum UnlockableType
    {
        GLIDE,
        DASH,
        WALL_JUMP,
        DOUBLE_JUMP
    }


    //Money
    public const string MONEY_TEXT = "MoneyText";


    //COINS------------------------------------------------------------------------
    
    //Force Range
    public const float COIN_X_RANGE = 5f;
    public const float COIN_Y_MIN_RANGE = 6f;
    public const float COIN_Y_MAX_RANGE = 8f;
    public const float COIN_TIME_UNTIL_FOLLOW = 0.5f;
    //Follow Range
    public const float COIN_ACCELERATION = 0.4f;
    public const float COIN_SPEED_VARIATION = 3f;
    
    //Coin Text
    public const string SMALL_COIN_TEXT = "SmallCoin";
    public const string MEDIUM_COIN_TEXT = "MediumCoin";
    public const string LARGE_COIN_TEXT = "LargeCoin";
    //Coin Value
    public const int SMALL_COIN_VALUE = 1;
    public const int MEDIUM_COIN_VALUE = 3;
    public const int LARGE_COIN_VALUE = 9;

}
