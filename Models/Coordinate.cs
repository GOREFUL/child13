namespace child13.Models
{
    public class Coordinate
    {
        private int _x1, _x2, _y1, _y2;

        public Coordinate()
        {
            _x1 = 0;
            _x2 = 0;
            _y1 = 0;
            _y2 = 0;
        }

        public Coordinate(int x1, int x2, int y1, int y2)
        {
            _x1 = x1;
            _x2 = x2;
            _y1 = y1;
            _y2 = y2;
        }

        public void _SetCoordinate(int x1, int x2, int y1, int y2)
        {
            _x1 = x1;
            _x2 = x2;
            _y1 = y1;
            _y2 = y2;
        }

        public int _GetX1()
        {
            return _x1;
        }
        public int _GetX2()
        {
            return _x2;
        }

        public int _GetY1()
        {
            return _y1;
        }

        public int _GetY2()
        {
            return _y2;
        }
    }
}
