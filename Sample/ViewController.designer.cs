// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Sample
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UITextField DotHeightTextField { get; set; }

		[Outlet]
		UIKit.UITextField DotRadiusTextField { get; set; }

		[Outlet]
		UIKit.UITextField DotWidthTextField { get; set; }

		[Outlet]
		UIKit.UIButton UpdateButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (UpdateButton != null) {
				UpdateButton.Dispose ();
				UpdateButton = null;
			}

			if (DotRadiusTextField != null) {
				DotRadiusTextField.Dispose ();
				DotRadiusTextField = null;
			}

			if (DotHeightTextField != null) {
				DotHeightTextField.Dispose ();
				DotHeightTextField = null;
			}

			if (DotWidthTextField != null) {
				DotWidthTextField.Dispose ();
				DotWidthTextField = null;
			}
		}
	}
}
