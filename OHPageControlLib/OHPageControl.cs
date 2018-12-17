using System;
using CoreAnimation;
using UIKit;
using CoreGraphics;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace OHPageControlLib
{
    public class OHPageControl : UIControl
    {
        #region Private Props

        readonly int limit = 5;
        List<int> fullScaleIndex = new List<int> { 0, 1, 2 };
        List<CALayer> dotLayers = new List<CALayer>();

        //float _diameter;
        int _centerIndex;
        int _currentPage;
        UIColor _inactiveTintColor = UIColor.LightGray;
        UIColor _currentPageTintColor = UIColor.Black;
        float _dotRadius = 5;
        float _dotWidth = 10;
        float _dotHeight = 10;
        float _padding = 8;
        float _minScaleValue = 0.4f;
        float _middleScaleValue = 0.7f;
        int _numberOfPages;
        bool _hideForSinglePage = true;
        float _inactiveTransparency = 0.4f;
        float _borderWidth;
        UIColor _borderColor = UIColor.Clear;

        FadeType _fadeType = FadeType.scale;
        float _minOpacityValue = 0.4f;
        float _middleOpacityValue = 0.7f;
        CGSize _size = CGSize.Empty;

        #endregion

        #region Public Props

        public int CenterIndex
        {
            get
            {
                _centerIndex = fullScaleIndex[1];
                return _centerIndex;
            }
        }

        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                if (_numberOfPages > _currentPage)
                {
                    Update();
                }
            }
        }

        public UIColor InactiveTintColor
        {
            get
            {
                return _inactiveTintColor;
            }
            set
            {
                _inactiveTintColor = value;
                SetNeedsLayout();
            }
        }

        public UIColor CurrentPageTintColor
        {
            get
            {
                return _currentPageTintColor;
            }
            set
            {
                _currentPageTintColor = value;
                SetNeedsLayout();
            }
        }

        public float DotRadius
        {
            get
            {
                return _dotRadius;
            }
            set
            {
                _dotRadius = value;
                if(_dotHeight < 2 * _dotRadius)
                {
                    _dotHeight = 2 * _dotRadius;
                }
                if(_dotWidth < 2 * _dotRadius)
                {
                    _dotWidth = 2 * _dotRadius;
                }
                UpdateDotLayersLayout();
            }
        }

        public float DotWidth
        {
            get
            {
                return _dotWidth;
            }
            set
            {
                _dotWidth = value;
                if(_dotRadius > _dotWidth / 2)
                {
                    _dotRadius = _dotWidth / 2;
                }
                UpdateDotLayersLayout();
            }
        }

        public float DotHeight
        {
            get
            {
                return _dotHeight;
            }
            set
            {
                _dotHeight = value;
                if (_dotRadius > _dotHeight / 2)
                {
                    _dotRadius = _dotHeight / 2;
                }
                UpdateDotLayersLayout();
            }
        }

        public float Padding
        {
            get
            {
                return _padding;
            }
            set
            {
                _padding = value;
                UpdateDotLayersLayout();
            }
        }

        public float MinScaleValue
        {
            get
            {
                return _minScaleValue;
            }
            set
            {
                _minScaleValue = value;
                SetNeedsLayout();
            }
        }

        public float MinOpacityValue
        {
            get
            {
                return _minOpacityValue;
            }
            set
            {
                _minOpacityValue = value;
                SetNeedsLayout();
            }
        }

        public float MiddleScaleValue
        {
            get
            {
                return _middleScaleValue;
            }
            set
            {
                _middleScaleValue = value;
                SetNeedsLayout();
            }
        }

        public float MiddleOpacityValue
        {
            get
            {
                return _middleOpacityValue;
            }
            set
            {
                _middleOpacityValue = value;
                SetNeedsLayout();
            }
        }

        public int NumberOfPages
        {
            get
            {
                return _numberOfPages;
            }
            set
            {
                _numberOfPages = value;
                SetupDotLayers();
                this.Hidden = _hideForSinglePage && _numberOfPages <= 1;
            }
        }

        public bool HideForSinglePage
        {
            get
            {
                return _hideForSinglePage;
            }
            set
            {
                _hideForSinglePage = value;
                SetNeedsLayout();
            }
        }

        public float InactiveTransparency
        {
            get
            {
                return _inactiveTransparency;
            }
            set
            {
                _inactiveTransparency = value;
                SetNeedsLayout();
            }
        }

        public float BorderWidth
        {
            get
            {
                return _borderWidth;
            }
            set
            {
                _borderWidth = value;
                SetNeedsLayout();
            }
        }

        public UIColor BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                SetNeedsLayout();
            }
        }

        public CGSize Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                SetNeedsLayout();
            }
        }

        public FadeType FadeType
        {
            get
            {
                return _fadeType;
            }
            set
            {
                _fadeType = value;
                SetNeedsLayout();
            }
        }

        #endregion

        #region Ctor

        public OHPageControl(CGRect frame, int numberOfPages)
        {
            Frame = frame;
            NumberOfPages = numberOfPages;
            SetupDotLayers();
        }

        #endregion

        #region Override Methods

        public override CGSize IntrinsicContentSize
        {
            get
            {
                return SizeThatFits(CGSize.Empty);
            }
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            var minValue = Math.Min(7, _numberOfPages);
            return new CGSize((float)minValue * DotWidth + (float)minValue - 1 * _padding, DotHeight);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            foreach (var layer in dotLayers)
            {
                if (_borderWidth > 0)
                {
                    layer.BorderWidth = _borderWidth;
                    layer.BorderColor = _borderColor.CGColor;
                }
            }

            Update();
        }

        #endregion

        #region Extension Methods

        void SetupDotLayers()
        {
            foreach (var layer in dotLayers)
            {
                layer.RemoveFromSuperLayer();
            }
            dotLayers.Clear();

            for (int num = 0; num < _numberOfPages; num++)
            {
                var dotLayer = new CALayer();
                this.Layer.AddSublayer(dotLayer);
                dotLayers.Add(dotLayer);
            }

            Size = CGSize.Empty;
            SetNeedsLayout();
            InvalidateIntrinsicContentSize();
        }

        void UpdateDotLayersLayout()
        {
            var floatCount = (float)_numberOfPages;
            var x = (Bounds.Size.Width - DotWidth * floatCount - _padding * (floatCount - 1)) * 0.5;
            var y = (Bounds.Size.Height - DotHeight) * 0.5;
            var frame = new CGRect(x, y, DotWidth, DotHeight);

            foreach (var layer in dotLayers)
            {
                layer.AffineTransform = CGAffineTransform.MakeIdentity();
                layer.CornerRadius = _dotRadius;
                layer.Frame = frame;
                frame.X += DotWidth + _padding;
            }
        }

        void SetupDotLayersPosition()
        {
            var centerLayer = dotLayers[CenterIndex];
            centerLayer.Position = new CGPoint(Frame.Width / 2, Frame.Height / 2);

            foreach (var layer in dotLayers)
            {
                var offset = dotLayers.IndexOf(layer);
                if (offset != CenterIndex)
                {
                    var index = Math.Abs(offset - CenterIndex);
                    var interval = offset > CenterIndex ? DotWidth + _padding : -(DotWidth + _padding);
                    layer.Position = new CGPoint(centerLayer.Position.X + interval * (float)index, layer.Position.Y);
                    Debug.WriteLine("dot Position X: " + layer.Position.X + " Y: " + layer.Position.Y);
                }
            }
        }

        void SetupDotLayersScale()
        {
            foreach (var layer in dotLayers)
            {
                try
                {
                    var offset = dotLayers.IndexOf(layer);

                    var first = fullScaleIndex.FirstOrDefault();
                    var last = fullScaleIndex.LastOrDefault();
                    var transform = CGAffineTransform.MakeIdentity();

                    if(!fullScaleIndex.Contains(offset))
                    {
                        float scaleValue = 0f;
                        if (Math.Abs(offset - first) == 1 || Math.Abs(offset - last) == 1)
                        {
                            scaleValue = Math.Min(_middleScaleValue, 1);
                        } 
                        else if (Math.Abs(offset - first) == 2 || Math.Abs(offset - last) == 2)
                        {
                            scaleValue = Math.Min(_minScaleValue, 1);
                        }
                        else
                        {
                            scaleValue = 0;
                        }
                        transform.Scale(scaleValue, scaleValue);
                    }

                    layer.AffineTransform = transform;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to setup dot layers scale. Error: " + ex);
                }
            }
        }

        void SetupDotLayersOpacity()
        {
            foreach (var layer in dotLayers)
            {
                try
                {
                    float opacity = 1f;
                    var offset = dotLayers.IndexOf(layer);

                    var first = fullScaleIndex.FirstOrDefault();
                    var last = fullScaleIndex.LastOrDefault();
                    var transform = CGAffineTransform.MakeIdentity();

                    if (!fullScaleIndex.Contains(offset))
                    {
                        float opacityValue = 0f;
                        if (Math.Abs(offset - first) == 1 || Math.Abs(offset - last) == 1)
                        {
                            opacityValue = Math.Min(_middleOpacityValue, 1);
                        }
                        else if (Math.Abs(offset - first) == 2 || Math.Abs(offset - last) == 2)
                        {
                            opacityValue = Math.Min(_minOpacityValue, 1);
                        }
                        else
                        {
                            opacityValue = 0;
                        }
                        opacity = opacityValue;
                    }

                    layer.Opacity = (float)opacity;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to setup dot layers opacity. Error: " + ex);
                }
            }
        }

        void Update()
        {
            foreach (var layer in dotLayers)
            {
                var offset = dotLayers.IndexOf(layer);
                layer.BackgroundColor = offset == _currentPage ? _currentPageTintColor.CGColor : _inactiveTintColor.ColorWithAlpha(_inactiveTransparency).CGColor;
            }

            if(Bounds.Size != _size)
            {
                UpdateDotLayersLayout();
                _size = Bounds.Size;
            }

            if (_numberOfPages > limit)
            {
                ChangeFullScaleIndexesIfNeeded();
                SetupDotLayersPosition();
                switch (_fadeType)
                {
                    case FadeType.scale:
                        SetupDotLayersScale();
                        break;
                    case FadeType.opacity:
                        SetupDotLayersOpacity();
                        break;
                }
            }
        }

        void ChangeFullScaleIndexesIfNeeded()
        {
            if(!fullScaleIndex.Contains(_currentPage))
            {
                var index = fullScaleIndex.LastOrDefault();
                var moreThanBefore = index < _currentPage;
                if(moreThanBefore)
                {
                    fullScaleIndex[0] = _currentPage - 2;
                    fullScaleIndex[1] = _currentPage - 1;
                    fullScaleIndex[2] = _currentPage;
                }
                else
                {
                    fullScaleIndex[0] = _currentPage;
                    fullScaleIndex[1] = _currentPage + 1;
                    fullScaleIndex[2] = _currentPage + 2;
                }
            }
        }

        #endregion
    }
}
