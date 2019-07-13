using RPGSystem.Characters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Combat
{
    public class Arena : IdentifiableItem
    {
        private int sizeX;
        [XmlAttribute]
        public int SizeX { get => sizeX; set => SetField(ref sizeX, value); }

        private int sizeY;
        [XmlAttribute]
        public int SizeY { get => sizeY; set => SetField(ref sizeY, value); }

        private string defaultEntrance;
        [XmlAttribute]
        public string DefaultEntrance { get => defaultEntrance; set => SetField(ref defaultEntrance, value); }

        private string defaultExit;
        [XmlAttribute]
        public string DefaultExit { get => defaultExit; set => SetField(ref defaultExit, value); }

        [XmlArray]
        [XmlArrayItem("Coords")]
        public BindingList<Coordinates> InaccessibleLocations { get; } = new BindingList<Coordinates>();

        [XmlArray]
        [XmlArrayItem("Character")]
        public BindingList<CombatArenaCharacterLocation> CharacterLocations { get; } = new BindingList<CombatArenaCharacterLocation>();

        [XmlArray]
        [XmlArrayItem("Door")]
        public BindingList<CombatArenaDoor> Doors { get; } = new BindingList<CombatArenaDoor>();

        public bool IsLocationAvailable(int x, int y, string ignoreCharacter = null)
        {
            if (x < 0 || x >= SizeX)
            {
                return false;
            }
            if (y < 0 || y >= SizeY)
            {
                return false;
            }
            foreach (var inacc in InaccessibleLocations)
            {
                if (inacc.X == x && inacc.Y == y)
                {
                    return false;
                }
            }
            foreach (var charLoc in CharacterLocations)
            {
                if (charLoc.CharacterIdentifier != ignoreCharacter && charLoc.IsCharacterOccupyingLocation(x, y))
                {
                    return false;
                }
            }
            return true;
        }

        public CombatArenaDoor FindEntrance(string doorIdentifier = null)
        {
            return FindDoor(String.IsNullOrEmpty(doorIdentifier) ? DefaultEntrance : doorIdentifier);
        }

        public CombatArenaDoor FindExit(string doorIdentifier = null)
        {
            return FindDoor(String.IsNullOrEmpty(doorIdentifier) ? DefaultExit : doorIdentifier);
        }

        public CombatArenaDoor FindDoor(string doorIdentifier)
        {
            if (!String.IsNullOrEmpty(doorIdentifier))
            {
                foreach (var door in Doors)
                {
                    if (door.Identifier == doorIdentifier)
                    {
                        return door;
                    }
                }
            }
            return null;
        }

        public Coordinates FindNearestFreeLocation(Coordinates coords, CharacterInstance character = null)
        {
            int radius = 0;
            bool moreToSearch = true;
            while (moreToSearch)
            {
                moreToSearch = false;
                int xStart = coords.X - radius;
                int xEnd = coords.X + radius;
                int yStart = coords.Y - radius;
                int yEnd = coords.Y + radius;
                for (int x = xStart; x <= xEnd; x++)
                {
                    for (int y = yStart; y <= yEnd; y++)
                    {
                        if (x == xStart || x == xEnd || y == yStart || y == yEnd)
                        {
                            if (IsWithinArena(x, y))
                            {
                                moreToSearch = true;
                            }
                            if (IsLocationAvailable(x, y, character?.Identifier))
                            {
                                if (character == null || CanCharacterBePlacedAt(character, x, y))
                                {
                                    return new Coordinates { X = x, Y = y };
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        public bool IsWithinArena(int x,int y)
        {
            return x >= 0 && x < SizeX && y >= 0 && y < SizeY;
        }

        public bool TryCharacterEnter(CharacterInstance character, string doorIdentifier = null)
        {
            if (character != null)
            {
                var existingLocation = GetCharacterLocation(character.Identifier);
                if (existingLocation != null)
                {
                    // already in the arena
                    return true;
                }
                var defaultEntrance = FindEntrance();
            }
            return false;
        }

        public bool CanCharacterBePlacedAt(CharacterInstance character, int x, int y)
        {
            string characterId = character?.Identifier;
            CharacterSize charSize = character?.Size;
            if (charSize != null && charSize.FootprintModifier > 1)
            {
                int overhang = Convert.ToInt32((charSize.FootprintModifier - 0.5) / 2);
                for (int newX = x - overhang; newX <= (x + overhang); newX++)
                {
                    for (int newY = y - overhang; newY <= (y + overhang); newY++)
                    {
                        if (!IsLocationAvailable(newX, newY, characterId))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return IsLocationAvailable(x, y);
        }

        public bool CanCharacterMoveTo(CharacterInstance character, int x, int y)
        {
            if (CanCharacterBePlacedAt(character, x, y))
            {
                var charLoc = GetCharacterLocation(character.Identifier);
                if (charLoc == null)
                {
                    return true; // not currently in the arena, so this must be the starting position
                }
                else
                {
                    // TODO determine if character can move to this position from where they are
                }
            }
            return false;
        }

        public CombatArenaCharacterLocation GetCharacterLocation(string characterIdentifier)
        {
            foreach (var charLoc in CharacterLocations)
            {
                if (charLoc.CharacterIdentifier == characterIdentifier)
                {
                    return charLoc;
                }
            }
            return null;
        }

        public bool MoveCharacterTo(CharacterInstance character, int x, int y)
        {
            if (CanCharacterBePlacedAt(character, x, y))
            {
                var charLoc = GetCharacterLocation(character.Identifier);
                if (charLoc == null)
                {
                    charLoc = new CombatArenaCharacterLocation() { CharacterIdentifier = character.Identifier };
                    CharacterLocations.Add(charLoc);
                }
                charLoc.X = x;
                charLoc.Y = y;
                return true;
            }
            return false;
        }
    }

    public class Coordinates
    {
        [XmlAttribute]
        public int X { get; set; }

        [XmlAttribute]
        public int Y { get; set; }

        public Coordinates() { }

        public Coordinates(Coordinates toCopy)
        {
            if (toCopy != null)
            {
                X = toCopy.X;
                Y = toCopy.Y;
            }
        }

        public override bool Equals(object obj)
        {
            Coordinates otherCoords = obj as Coordinates;
            if (otherCoords != null)
            {
                return otherCoords.X == X && otherCoords.Y == Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return Format(X, Y);
        }

        public static string Format(int x, int y)
        {
            return String.Format("({0},{1})", x, y);
        }
    }

    public class CombatArenaCharacterLocation : Coordinates
    {
        [XmlAttribute("Identifier")]
        public string CharacterIdentifier { get; set; }

        [XmlAttribute]
        public SimpleMapDirection Facing { get; set; }

        public CombatArenaCharacterLocation() { }

        public CombatArenaCharacterLocation(CombatArenaCharacterLocation toCopy) : base(toCopy)
        {
            if (toCopy != null)
            {
                CharacterIdentifier = toCopy.CharacterIdentifier;
            }
        }

        [XmlIgnore]
        public CharacterInstance Character
        {
            get { return Game.ActiveGame?.Find(CharacterIdentifier); }
        }

        public bool IsCharacterOccupyingLocation(int x, int y)
        {
            if (x == X && y == Y)
            {
                return true;
            }
            CharacterSize charSize = Character?.Size;
            if (charSize != null && charSize.FootprintModifier > 1)
            {
                double overhang = charSize.FootprintModifier / 2;
                if (x >= (X - overhang) && x <= (X + overhang) && 
                    y >= (Y - overhang) && y <= (Y + overhang))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class CombatArenaPath : List<Coordinates>
    {
        public bool Crosses(CombatArenaPath otherPath)
        {
            if (otherPath != null)
            {
                List<int> hashes = new List<int>();
                List<int> otherHashes = new List<int>();
                foreach (var coords in this)
                {
                    hashes.Add(coords.GetHashCode());
                }
                foreach (var otherCoords in otherPath)
                {
                    if (hashes.Contains(otherCoords.GetHashCode()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static CombatArenaPath ReadPath(Coordinates coords, Dictionary<string, CombatArenaPath> paths)
        {
            string key = coords?.ToString();
            if (!String.IsNullOrEmpty(key) && paths != null && paths.ContainsKey(key))
            {
                return paths[key];
            }
            return null;
        }

        public static CombatArenaPath FindPath(CharacterInstance character, Arena arena, Coordinates start, Coordinates end)
        {
            // search simultaneously from start and end until paths cross, breadth-first search

            CoordinatesDictionary<CombatArenaPath> startPaths = new CoordinatesDictionary<CombatArenaPath>();
            List<Coordinates> nextStartLocations = new List<Coordinates>();
            nextStartLocations.Add(start);

            CoordinatesDictionary<CombatArenaPath> endPaths = new CoordinatesDictionary<CombatArenaPath>();
            List<Coordinates> nextEndLocations = new List<Coordinates>();
            nextEndLocations.Add(end);

            while (nextStartLocations.Count > 0 || nextEndLocations.Count > 0)
            {
                List<Coordinates> newStartLocations = new List<Coordinates>();
                foreach (var point in nextStartLocations)
                {
                    newStartLocations.AddRange(PopulateAdjacentPaths(character, arena, point, startPaths));
                }
                nextStartLocations = newStartLocations;

                List<Coordinates> newEndLocations = new List<Coordinates>();
                foreach (var point in nextEndLocations)
                {
                    newEndLocations.AddRange(PopulateAdjacentPaths(character, arena, point, endPaths));
                }
                nextEndLocations = newEndLocations;

                foreach (var key in startPaths.Keys)
                {
                    if (endPaths.ContainsKey(key))
                    {
                        var startPath = startPaths[key];
                        var endPath = endPaths[key];
                        if (endPath.Count > 1)
                        {
                            for (int i = endPath.Count - 2; i >= 0; i--)
                            {
                                startPath.Add(endPath[i]);
                            }
                        }
                        return startPath;
                    }
                }
            }
            return null;
        }

        private static CombatArenaPath CloneAndExtendPath(CombatArenaPath path, int x, int y)
        {
            return CloneAndExtendPath(path, new Coordinates { X = x, Y = y });
        }

        private static CombatArenaPath CloneAndExtendPath(CombatArenaPath path, Coordinates point)
        {
            CombatArenaPath clone = new CombatArenaPath();
            if (path != null)
            {
                foreach (var coord in path)
                {
                    clone.Add(new Coordinates(coord));
                }
            }
            if (!clone.Contains(point))
            {
                clone.Add(point);
            }
            return clone;
        }

        private static List<Coordinates> PopulateAdjacentPaths(CharacterInstance character, Arena arena, Coordinates point, CoordinatesDictionary<CombatArenaPath> visited)
        {
            List<Coordinates> nextLocations = new List<Coordinates>();
            if (point != null)
            {
                int nextX = point.X;
                int nextY = point.Y;
                var currentPath = visited[point];
                if (currentPath == null)
                {
                    currentPath = new CombatArenaPath();
                    currentPath.Add(new Coordinates(point));
                    visited[point] = currentPath;
                }
                for (int x = nextX - 1; x <= nextX + 1; x++)
                {
                    for (int y = nextY - 1; y <= nextY + 1; y++)
                    {
                        if (x != nextX || y != nextY)
                        {
                            var coords = new Coordinates { X = x, Y = y };
                            if (!nextLocations.Contains(coords) && !visited.ContainsKey(x, y))
                            {
                                if ((character == null && arena.IsLocationAvailable(x, y)) ||
                                    arena.CanCharacterBePlacedAt(character, x, y))
                                {
                                    nextLocations.Add(coords);
                                    visited[x, y] = CloneAndExtendPath(currentPath, coords);
                                }
                            }
                        }
                    }
                }
            }
            return nextLocations;
        }

        
    }

    public class CoordinatesDictionary<T> where T : class
    {
        private Dictionary<string, T> lookup = new Dictionary<string, T>();

        public T this[int x, int y]
        {
            get
            {
                string key = GetKey(x, y);
                if (lookup.ContainsKey(key))
                {
                    return lookup[key];
                }
                return null;
            }
            set
            {
                string key = GetKey(x, y);
                lookup[key] = value;
            }
        }

        public T this[string key]
        {
            get { return lookup[key]; }
            set { lookup[key] = value; }
        }

        public T this[Coordinates coords]
        {
            get
            {
                if (coords != null)
                {
                    return this[coords.X, coords.Y];
                }
                return null;
            }
            set
            {
                if (coords != null)
                {
                    this[coords.X, coords.Y] = value;
                }
            }
        }

        public bool ContainsKey(int x, int y)
        {
            return lookup.ContainsKey(GetKey(x, y));
        }

        public bool ContainsKey(Coordinates coords)
        {
            return ContainsKey(coords.X, coords.Y);
        }

        public bool ContainsKey(string key)
        {
            return lookup.ContainsKey(key);
        }

        public IEnumerable<string> Keys
        {
            get { return lookup.Keys; }
        }

        private static string GetKey(int x, int y)
        {
            return x + "#" + y;
        }
    }

    public class CombatArenaDoor : IdentifiableItem
    {
        [XmlElement]
        public Coordinates Location { get; set; }

        [XmlAttribute]
        public CombatArenaDoorType Type { get; set; }

        [XmlAttribute]
        public bool IsLocked { get; set; }

        [XmlAttribute]
        public SimpleMapDirection Facing { get; set; }
    }

    public enum CombatArenaDoorType
    {
        Entrance,
        Exit,
        Any
    }
}
