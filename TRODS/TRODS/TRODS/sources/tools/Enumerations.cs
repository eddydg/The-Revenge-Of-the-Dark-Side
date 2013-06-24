using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRODS
{
    public class INFO
    {
        public static bool ENG = true;
        public static string version = "3.0";
    }

    public enum CharacterActions
    {
        StandRight, StandLeft, WalkLeft, WalkRight, WalkUp, WalkDown, JumpRight, JumpLeft, Fall, Paralized, ReceiveAttack, ReceiveAttackLeft, ReceiveAttackRight,
        Attack1Right, Attack1Left,
        AttackStunRight, AttackStunLeft,
        Attack2Left, Attack2Right
    }
    /// <summary>
    /// Scenes du jeu
    /// </summary>
    public enum Scene
    {
        MainMenu = 0, InGame = 1, Extra = 2, Credit = 3, Titre = 4, Options = 5,
        IntroVid, GameOver, IntroHistoire,IntroLateX,LateXEradicated,BeforeKingFight,AfterKingFight,LastFight
    };
    /// <summary>
    /// Les 4 directions
    /// </summary>
    public enum Direction
    {
        None = 0, Right = 1, Left = 2, Up = 3, Down = 4,
    };
    /// <summary>
    /// Enumeration de tous les effects sonores du jeu
    /// </summary>
    public enum Sons
    {
        MenuSelection,
    };
    /// <summary>
    /// Enumeration de toutes les musiques du jeu
    /// </summary>
    public enum Musiques
    {
        None, MenuMusic, CreditMusic, Intro, IntroLateX, TransitionLateX
    };
}
