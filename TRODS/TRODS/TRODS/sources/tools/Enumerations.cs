using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRODS
{
    public enum CharacterActions
    {
        StandRight, StandLeft, WalkLeft, WalkRight, WalkUp, WalkDown, JumpRight, JumpLeft, Fall, Paralized, ReceiveAttack,ReceiveAttackLeft,ReceiveAttackRight,
        Attack1Right, Attack1Left,
        AttackStunRight, AttackStunLeft

    }
    /// <summary>
    /// Scenes du jeu
    /// </summary>
    public enum Scene
    {
        MainMenu = 0, InGame = 1, Extra = 2, Credit = 3, Titre = 4, Options = 5,IntroVid,GameOver,IntroHistoire
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
        MenuMusic, CreditMusic,
    };
}
