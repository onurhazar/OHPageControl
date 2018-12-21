# OHPageControl
A page control written in C#, similar to that used in Instagram

![Demo](https://github.com/onurhazar/OHPageControl/blob/master/OHPageControl1.gif) ![Demo](https://github.com/onurhazar/OHPageControl/blob/master/OHPageControl2.gif)



## Requirements
iOS 8.0+
Xcode 9.0+

## How to Use
```C#
var frame = new CGRect(0, 500, UIScreen.MainScreen.Bounds.Width, 100);
pc = new OHPageControl(frame, 8)
{
    DotRadius = 3,//set 8 to make it circle
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
```
