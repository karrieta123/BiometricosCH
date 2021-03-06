using System.Drawing;

namespace EDSDKWrapper.UI
{
    public struct EosFocus
    {
        internal static EosFocus Create(EDSDKLib.EDSDK.EdsFocusInfo focus)
        {
            var focusPoints = new EosFocusPoint[focus.pointNumber];
            for (var i = 0; i < focusPoints.Length; ++i)
                focusPoints[i] = EosFocusPoint.Create(focus.focusPoint[i]);

            return new EosFocus
            {
                Bounds = new Rectangle
                {
                    X = focus.imageRect.x,
                    Y = focus.imageRect.y,
                    Height = focus.imageRect.height,
                    Width = focus.imageRect.width,
                },
                ExecuteMode = focus.executeMode,
                FocusPoints = focusPoints
            };
        }
        /// <summary>
        /// Gets or sets the bounds.
        /// </summary>
        /// <value>
        /// The bounds.
        /// </value>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Gets or sets the execute mode.
        /// </summary>
        /// <value>
        /// The execute mode.
        /// </value>
        public long ExecuteMode { get; private set; }

        /// <summary>
        /// Gets or sets the focus points.
        /// </summary>
        /// <value>
        /// The focus points.
        /// </value>
        public EosFocusPoint[] FocusPoints { get; private set; }
    }
}
