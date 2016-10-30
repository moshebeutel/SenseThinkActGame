using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public class Position
    {
        public static double CLOSE_TO_ONE = 1.5;
        public double X { get; set; }
        public double Y { get; set; }

        public Position(double x = 0.0, double y = 0.0)
        {
            X = x;
            Y = y;
        }
        public Position(Position other)
            : this(other.X, other.Y)
        {

        }

        public double AngleTo(Position otherPosition)
        {
            return Angle(this, otherPosition);
        }

        public double DistanceFrom(Position otherPosition)
        {
            return Distance(this, otherPosition);
        }

        public Position MidwayTo(Position otehrPosition, double otherPositionWeight)
        {
            return Mid(this, otehrPosition, otherPositionWeight);
        }

        public Position getOfset(double range, double angle)
        {
            return Ofset(this, range, angle);
        }

        public bool IsBetween(Position p1, Position p2)
        {
            return IsBetween(this, p1, p2);
        }

        public static double Distance(Position p1, Position p2)
        {
            var dX = p1.X - p2.X;
            var dY = p1.Y - p2.Y;
            return Math.Sqrt(dX * dX + dY * dY);
        }

        public static double Angle(Position p1, Position p2)
        {
            var dX = p2.X - p1.X;
            var dY = p2.Y - p1.Y;
            return Math.Atan2(dY, dX);
        }

        public static bool IsBetween(Position p, Position p1, Position p2)
        {
            var distanceFromFirst = p.DistanceFrom(p1);
            var distanceFromSecond = p.DistanceFrom(p2);
            var sumOfWays =  distanceFromFirst + distanceFromSecond;
            var directWay =  p1.DistanceFrom(p2);
            var ratio = sumOfWays / directWay;


            return ratio < CLOSE_TO_ONE;
        }

        public static Position Mid(Position p1, Position p2, double p2Weight)
        {
            Debug.Assert(p2Weight > 0 && p2Weight < 1);
            return new Position((1 - p2Weight) * p1.X + p2Weight * p2.X, (1 - p2Weight) * p1.Y + p2Weight * p2.Y);
        }

        public static Position Ofset(Position origin, double range, double angle)
        {
            var x = origin.X + range * Math.Cos(angle);
            var y = origin.Y + range * Math.Sin(angle);
            return new Position(x, y);
        }
    }
}
