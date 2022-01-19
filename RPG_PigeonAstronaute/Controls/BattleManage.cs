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

        public BattleManage(Player player, Ennemie ennemie)
        {
            _playerBattle = player;
            _ennemieBattle = ennemie;
        }

        public bool IntersectSprite()
        {
            if ((_playerBattle._rectangleSize.Intersects(_ennemieBattle._rectangleSize) || _ennemieBattle._rectangleSize.Intersects(_playerBattle._rectangleSize) && IsPresssingKey(_kbState, _touches[(int)Touches.Attack])))
                return true;
            else
                return false;
        }

        


    }
}
