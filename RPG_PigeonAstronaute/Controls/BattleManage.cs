using RPG_PigeonAstronaute.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_PigeonAstronaute.Controls
{
    public class BattleManage:ModelePerso
    {
        protected Player _playerBattle;
        protected Ennemie _ennemieBattle;

        public BattleManage()
        {
        }

        public bool IntersectSprite(Player player, Ennemie ennemie)
        {
            if ((player._rectangleSize.Intersects(ennemie._rectangleSize) || ennemie._rectangleSize.Intersects(player._rectangleSize)) && IsPresssingKey(_kbState, _touches[(int)Touches.Attack]))
                return true;
            else
                return false;
        }

        public bool IntersectSprite(Ennemie ennemie, Player player)
        {
            if ((ennemie._rectangleSize.Intersects(player._rectangleSize) && (ennemie._currentAnimation == _animationsAttack[0] || ennemie._currentAnimation == _animationsAttack[1] || ennemie._currentAnimation == _animationsAttack[2] || ennemie._currentAnimation == _animationsAttack[3])))
                return true;
            else
                return false;
        }

        public void Fight(Player player, Ennemie ennemie)
        {
            if (IntersectSprite(player, ennemie))
                player._health -= ennemie._dgt;
            if (IntersectSprite(ennemie, player))
                ennemie._health -= player._dgt;
        }
    }
}
