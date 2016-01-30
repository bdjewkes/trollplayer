using System;
using System.Linq;

namespace SpryFox.Common {

    public static class Neighbours2D {

        public enum EightDir {
            UpLeft,
            Up,
            UpRight,
            Right,
            DownRight,
            Down,
            DownLeft,
            Left,
        }

        public readonly static Point[] Eight = DirsToOffsets<EightDir>(OffsetTowards);

        public enum CornersDir {
            UpLeft,
            UpRight,
            DownRight,
            DownLeft,
        }
        
        public readonly static Point[] Corners = DirsToOffsets<CornersDir>(OffsetTowards);

        public static CornersDir CornerFromAxes(bool isRight, bool isUp) {
            return isRight && isUp ? CornersDir.UpRight
                : isRight && isUp == false ? CornersDir.DownRight
                : isRight == false && isUp ? CornersDir.UpLeft
                : CornersDir.DownLeft;
        }

        //////////////////////////////////////////////////

        //////////////////////////////////////////////////

        static Point[] DirsToOffsets<T>(Func<T,Point> converter) {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(dir => converter(dir))
                .ToArray();
        }

        // only used at boot. use Eight[(int)dir] to get cached value
        static Point OffsetTowards(EightDir dir) {
            switch(dir) {
            case EightDir.UpLeft: return new Point(-1, 1);
            case EightDir.Up: return new Point(0, 1);
            case EightDir.UpRight: return new Point(1, 1);
            case EightDir.Right: return new Point(1, 0);
            case EightDir.DownRight: return new Point(1, -1);
            case EightDir.Down: return new Point(0, -1);
            case EightDir.DownLeft: return new Point(-1, -1);
            case EightDir.Left: return new Point(-1, 0);
            default:
                Assert.Fail("unknown 8Dir ", dir);
                return Point.zero;
            }
        }

        static Point OffsetTowards(CornersDir dir) {
            switch(dir) {
            case CornersDir.UpLeft: return new Point(-1, 1);
            case CornersDir.UpRight: return new Point(1, 1);
            case CornersDir.DownRight: return new Point(1, -1);
            case CornersDir.DownLeft: return new Point(-1, -1);
            default:
                Assert.Fail("unknown corner dir ", dir);
                return Point.zero;
            }
        }
    }
}