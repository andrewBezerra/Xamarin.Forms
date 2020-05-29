﻿using System.ComponentModel;
using System.Drawing;
using AppKit;

namespace Xamarin.Forms.Platform.MacOS
{
	public class FrameRenderer : VisualElementRenderer<Frame>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
				SetupLayer();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
				e.PropertyName == VisualElement.BackgroundProperty.PropertyName ||
				e.PropertyName == Xamarin.Forms.Frame.BorderColorProperty.PropertyName ||
				e.PropertyName == Xamarin.Forms.Frame.HasShadowProperty.PropertyName ||
				e.PropertyName == Xamarin.Forms.Frame.CornerRadiusProperty.PropertyName)
				SetupLayer();
		}

		protected override void SetBackgroundColor(Color color)
		{

		}

		protected override void SetBackground(Brush brush)
		{
			Layer.RemoveGradientLayer();

			if (brush != null && !brush.IsEmpty)
			{
				if (brush is SolidColorBrush solidColorBrush)
				{
					var backgroundColor = solidColorBrush.Color;

					if (backgroundColor == Color.Default)
						Layer.BackgroundColor = NSColor.White.CGColor;
					else
						Layer.BackgroundColor = backgroundColor.ToCGColor();
				}
				else
				{
					var gradientLayer = this.GetGradientLayer(brush);

					if (gradientLayer != null)
					{
						Layer.BackgroundColor = NSColor.Clear.CGColor;
						Layer.InsertGradientLayer(gradientLayer, 0);

						gradientLayer.CornerRadius = Layer.CornerRadius;
						gradientLayer.BorderColor = Layer.BorderColor;
					}
				}
			}
		}

		void SetupLayer()
		{
			float cornerRadius = Element.CornerRadius;

			if (cornerRadius == -1f)
				cornerRadius = 5f; // default corner radius

			Layer.CornerRadius = cornerRadius;

			if (Element.BackgroundColor == Color.Default)
				Layer.BackgroundColor = NSColor.White.CGColor;
			else
				Layer.BackgroundColor = Element.BackgroundColor.ToCGColor();

			if (Element.HasShadow)
			{
				Layer.ShadowRadius = 5;
				Layer.ShadowColor = NSColor.Black.CGColor;
				Layer.ShadowOpacity = 0.8f;
				Layer.ShadowOffset = new SizeF();
			}
			else
				Layer.ShadowOpacity = 0;

			if (Element.BorderColor == Color.Default)
				Layer.BorderColor = NSColor.Clear.CGColor;
			else
			{
				Layer.BorderColor = Element.BorderColor.ToCGColor();
				Layer.BorderWidth = 1;
			}

			Layer.RasterizationScale = NSScreen.MainScreen.BackingScaleFactor;
			Layer.ShouldRasterize = true;
		}
	}
}