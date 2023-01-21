using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Sokoban.MVVM.Model;

namespace Sokoban.MVVM.ViewModel
{

    public class SaveEventsViewModel
    {
        public MapViewModel startMap
        {
            get;
            private set;
        }

        List<Event> events;

        private void SaveMap(MapViewModel map)
        {
            int width = map.Width;
            int height = map.Height;
            startMap = new MapViewModel(width, height);
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    startMap.SetCell(i, j, map.GetCell(i, j));
                }
            }
        }

        public SaveEventsViewModel(MapViewModel StartMap)
        {
            SaveMap(StartMap);

            events = new List<Event>();
        }

        public void SaveEvent(Vector2[] Pos, int[] Types)
        {
            events.Add(new Event(Pos, Types));
        }

        public Event LastEvent()
        {
            if (events.Count == 0)
            {
                return null;
            }
            Event _event = events.Last();
            events.Remove(_event);
            return _event;
        }

        public void SaveEvents()
        {
            string path = "D:/vs/Sokoban/bin/Debug/Levels/SaveLevel.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                int width = startMap.Width;
                int height = startMap.Height;

                writer.WriteLineAsync(width + "/n" + height);
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        int N = startMap.GetCell(i, j);
                        writer.WriteAsync(N.ToString());
                    }
                    writer.WriteLineAsync("");
                }

                foreach (var i in events)
                {
                    writer.WriteLineAsync(i.lastPos.X + " " + i.lastPos.Y);
                    writer.WriteLineAsync(i.playerPos.X + " " + i.playerPos.Y);
                    writer.WriteLineAsync(i.frontPos.X + " " + i.frontPos.Y);
                    writer.WriteLineAsync(i.lastType.ToString());
                    writer.WriteLineAsync(i.underType.ToString());
                    writer.WriteLineAsync(i.frontType.ToString());
                }
            }
        }

        
    }
    public class Event
    {
        public Vector2 lastPos
        {
            get;
            private set;
        }
        public Vector2 playerPos { get; private set; }
        public Vector2 frontPos { get; private set; }
        public int lastType { get; private set; }
        public int underType
        {
            get; private set;
        }
        public int frontType
        {
            get; private set;
        }

        public Event(Vector2[] Pos, int[] Types)
        {
            lastPos = Pos[0];
            playerPos = Pos[1];
            frontPos = Pos[2];
            lastType = Types[0];
            underType = Types[1];
            frontType = Types[2];
        }
    }
}
