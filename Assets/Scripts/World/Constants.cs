using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    //PLAYER-----------------------------------------------------------
    
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
        Attacking,
        Landing,
        Knocked,
        Stunned
    }



    //UI---------------------------------------------------------------

    //Unlockable Menu Settings
    public const string glideTitle = "Glide";
    public const string dashTitle = "Dash";
    public const string wallJumpTitle = "Wall Jump";
    public const string doubleJumpTitle = "Double Jump";

    public const string glideDescription = "It eases the fall. Reliable enough, for the brave and the fools willing to take the risk";
    public const string dashDescription = "Move much faster for a short period of time. Become the wind";
    public const string wallJumpDescription = "Allows you to bounce off of walls. Get your grip on! ";
    public const string doubleJumpDescription = "Provides a second push upwards. defy all logic, break the game ;)";

    public const string unlockablesExit = "Press 'E' or 'North Button' to continue";

    public enum UnlockableType
    {
        Glide,
        Dash,
        WallJump,
        DoubleJump
    }
}
