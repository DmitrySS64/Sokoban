using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Sokoban.MVVM.ViewModel;
using Sokoban.MVVM.Model;
using System.Windows;

namespace Sokoban.MVVM.ViewModel
{
    public class GameState
    {
        MapViewModel map;
        Vector2 Player;

        Vector2 pointForBox;
        bool standsOnAPoint = false;

        SaveEventsViewModel save;

        int needScore;

        public bool WinGame
        {
            get;
            private set;
        }


        public GameState(MapViewModel Map)
        {
            map = Map;
            Player = map.GetPlacePlayer();
            needScore = map.GetNeedScore();
            save = new SaveEventsViewModel(Map);
        }

        public bool CanMove(Vector2 VectorMove)
        {
            Vector2 endPos = Player + VectorMove;
            int Cell = map.GetCell((int)endPos.X, (int)endPos.Y);

            if (Cell == 1 || Cell == 5)
            {
                return false;
            }
            if (Cell == 4 || Cell == 6)
            {
                return CanPush(endPos, VectorMove);
            }
            return true;
        }

        private bool CanPush(Vector2 Pos, Vector2 Move)
        {
            Vector2 endPos = Pos + Move;
            int cell = map.GetCell((int)endPos.X, (int)endPos.Y);
            if (cell == 2 || cell == 3)
                return true;
            return false;
        }

        public MapViewModel Model()
        {
            return map;
        }

        public void Move(Vector2 vectorMove)
        {
            if (CanMove(vectorMove))
            {
                Vector2 endPos = Player + vectorMove;
                int Cell = map.GetCell((int)endPos.X, (int)endPos.Y);

                Vector2 endPosBox = endPos + vectorMove;
                int Cell2 = map.GetCell((int)endPosBox.X, (int)endPosBox.Y);

                Vector2[] vectors = { Player, endPos, endPosBox };

                int type1;
                if (standsOnAPoint)
                {
                    type1 = 3;
                }
                else type1 = 7;

                int[] Types = { type1, map.GetCell((int)endPos.X, (int)endPos.Y), map.GetCell((int)endPosBox.X, (int)endPosBox.Y) };

                save.SaveEvent(vectors, Types);

                if (standsOnAPoint)
                {
                    standsOnAPoint = false;
                    map.SetCell((int)Player.X, (int)Player.Y, 3);
                }
                else
                {
                    map.SetCell((int)Player.X, (int)Player.Y, 2);
                }


                if (Cell == 4)
                {
                    if (Cell2 == 3)
                    {
                        map.SetCell((int)endPosBox.X, (int)endPosBox.Y, 6);
                        if (map.GetGreenBoxs() == needScore)
                        {
                            WinGame = true;
                        }
                    }
                    else
                    {
                        map.SetCell((int)endPosBox.X, (int)endPosBox.Y, 4);
                    }
                }
                if (Cell == 6)
                {
                    standsOnAPoint = true;
                    pointForBox = endPos;
                    if (Cell2 == 3)
                    {
                        map.SetCell((int)endPosBox.X, (int)endPosBox.Y, 6);
                    }
                    else
                    {
                        map.SetCell((int)endPosBox.X, (int)endPosBox.Y, 4);
                    }
                }
                if (Cell == 3)
                {
                    standsOnAPoint = true;
                    pointForBox = endPos;
                }

                map.SetCell((int)endPos.X, (int)endPos.Y, 7);
                Player = endPos;
            }
        }

        public void ReturnMove()
        {
            Event lastEvent = save.LastEvent();
            if (lastEvent == null)
            {
                return;
            }
            Player = lastEvent.lastPos;
            int type = map.GetCell((int)Player.X, (int)Player.Y);
            Vector2 Pos1 = lastEvent.playerPos;
            Vector2 Pos2 = lastEvent.frontPos;

            if (type == 3)
            {
                standsOnAPoint = true;
                pointForBox = Player;
            }
            else standsOnAPoint = false;

            map.SetCell((int)Player.X, (int)Player.Y, 7);
            map.SetCell((int)Pos1.X, (int)Pos1.Y, lastEvent.underType);
            map.SetCell((int)Pos2.X, (int)Pos2.Y, lastEvent.frontType);
        }

        public void ReturnGame()
        {
            map = save.startMap;
            Player = map.GetPlacePlayer();
            needScore = map.GetNeedScore();
            save = new SaveEventsViewModel(map);
        }

        public void SaveGame()
        {
            save.SaveEvents();
        }
    }
}
