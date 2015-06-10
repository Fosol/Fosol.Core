using System;

namespace Fosol.Core.Drawing
{
    /// <summary>
    /// OffsetRule static class, provides a collection of preconfigured CenterPoint objects.
    /// Here are a number of default behaviours that specify the center of a object.
    /// Default center is the true center; position 0, 0 in the x, y axis's.
    /// A center point ratio can be defined with two number.  The x-axis and the y-axis.
    /// Each value can be -1 to 1, where 0 represents the true center.
    /// </summary>
    /// <exexample>
    ///    +
    ///    +
    ///    +
    /// ---0+++
    ///    -
    ///    -
    ///    -
    /// </exexample>
    public static class OffsetRule
    {
        #region Variables
        /// <summary>
        /// Centering options for the Scale function.
        /// </summary>
        public enum Option
        {
            Center,
            Portrait,
            Landscape,
            Left,
            Right,
            Top,
            TopLeft,
            TopRight,
            Bottom,
            BottomLeft,
            BottomRight
        }
        #endregion

        #region Properties
        /// <summary>
        /// get - Center the object.
        /// </summary>
        public static readonly CenterPoint Center = new CenterPoint(0, 0);

        /// <summary>
        /// get - Center closer to the top.
        /// </summary>
        public static readonly CenterPoint Portrait = new CenterPoint(0, 0.5f);

        /// <summary>
        /// get - Center the object.
        /// </summary>
        public static readonly CenterPoint Landscape = new CenterPoint(0, 0);

        /// <summary>
        /// get - Center on the absolute left side.
        /// </summary>
        public static readonly CenterPoint Left = new CenterPoint(-1, 0);

        /// <summary>
        /// get - Center on the absolute right side.
        /// </summary>
        public static readonly CenterPoint Right = new CenterPoint(1, 0);

        /// <summary>
        /// get - Center on the absolute top.
        /// </summary>
        public static readonly CenterPoint Top = new CenterPoint(0, 1);

        /// <summary>
        /// get - Center on the absolute top left.
        /// </summary>
        public static readonly CenterPoint TopLeft = new CenterPoint(-1, 1);

        /// <summary>
        /// get - Center on the absolute top left.
        /// </summary>
        public static readonly CenterPoint TopRight = new CenterPoint(1, 1);

        /// <summary>
        /// get - Center on the absolute bottom.
        /// </summary>
        public static readonly CenterPoint Bottom = new CenterPoint(0, -1);

        /// <summary>
        /// get - Center on the absolute bottom.
        /// </summary>
        public static readonly CenterPoint BottomLeft = new CenterPoint(-1, -1);

        /// <summary>
        /// get - Center on the absolute bottom.
        /// </summary>
        public static readonly CenterPoint BottomRight = new CenterPoint(1, -1);

        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Return the corresponding CenterPoint that matches the specified option.
        /// </summary>
        /// <param name="option">Option value.</param>
        /// <returns>CenterPoint object.</returns>
        public static CenterPoint FromOption(Option option)
        {
            switch (option)
            {
                case (Option.Center):
                    return Center;
                case (Option.Portrait):
                    return Portrait;
                case (Option.Landscape):
                    return Landscape;
                case (Option.Left):
                    return Left;
                case (Option.Right):
                    return Right;
                case (Option.Top):
                    return Top;
                case (Option.TopLeft):
                    return TopLeft;
                case (Option.TopRight):
                    return TopRight;
                case (Option.Bottom):
                    return Bottom;
                case (Option.BottomLeft):
                    return BottomLeft;
                case (Option.BottomRight):
                    return BottomRight;
                default:
                    return Center;
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion

    }
}
