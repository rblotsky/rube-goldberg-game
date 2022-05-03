using Unity.VisualScripting;

namespace RubeGoldbergGame
{
    
    public class LevelData
    {
        private int _levelID;
        private string _levelName;
        private double _bestTime;
        private int _blocksUsed;
        private Completion _isBeat;

        public string LevelName
        {
            get => _levelName;
            set => _levelName = value;
        }

        public int LevelID
        {
            get => _levelID;
            set => _levelID = value;
        }

        public double BestTime
        {
            get => _bestTime;
            set => _bestTime = value;
        }

        public int BlocksUsed
        {
            get => _blocksUsed;
            set => _blocksUsed = value;
        }

        public Completion IsBeat
        {
            get => _isBeat;
            set => _isBeat = value;
        }
    }
}