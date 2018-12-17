using System;
using CoreGraphics;
using UIKit;
using OHPageControlLib;

namespace Sample
{
    public partial class ViewController : UIViewController
    {
        OHPageControl pc;
        int page;

        protected ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            UpdateButton.TouchUpInside += HandleButtonTouchupInside;
            DotRadiusTextField.Ended += HandleTextFieldsEndEditing;
            DotHeightTextField.Ended += HandleTextFieldsEndEditing;
            DotWidthTextField.Ended += HandleTextFieldsEndEditing;

            var frame = new CGRect(0, 500, UIScreen.MainScreen.Bounds.Width, 100);
            pc = new OHPageControl(frame, 8)
            {
                //set dot radius 10, dot height 20, dot width 20 to make it circle
                DotRadius = 3,
                Padding = 6,
                DotHeight = 6,
                DotWidth = 30,
                InactiveTintColor = UIColor.White.ColorWithAlpha(0.4f),
                CurrentPageTintColor = UIColor.Black,
                BorderWidth = 1,
                BorderColor = UIColor.Black.ColorWithAlpha(0.4f),
                FadeType = FadeType.scale
            };
            View.AddSubview(pc);

            page = pc.CurrentPage;

            DotRadiusTextField.Text = pc.DotRadius.ToString();
            DotHeightTextField.Text = pc.DotHeight.ToString();
            DotWidthTextField.Text = pc.DotWidth.ToString();
        }

        private void HandleTextFieldsEndEditing(object sender, EventArgs e)
        {
            pc.DotRadius = int.TryParse(DotRadiusTextField.Text, out int resultDot) ? resultDot : 3;
            pc.DotHeight = int.TryParse(DotHeightTextField.Text, out int resultHeight) ? resultHeight : 6;
            pc.DotWidth = int.TryParse(DotWidthTextField.Text, out int resultWidth) ? resultWidth : 6;
        }

        private void HandleButtonTouchupInside(object sender, EventArgs e)
        {
            page++;

            if (page > 7) //currentpage - 1
            {
                page = 0;
            }

            pc.CurrentPage = page;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
