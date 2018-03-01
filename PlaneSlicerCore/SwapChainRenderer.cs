using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;

namespace PlaneSlicerCore
{
	class SwapChainRenderer : IDisposable
	{
		Compositor Compositor;
		CanvasSwapChain SwapChain;
		SpriteVisual SwapChainVisual;
		CancellationTokenSource DrawLoopCancellationTokenSource;

		private int DrawCount;
		private int DeviceCount;

		volatile bool _Paused;

		public bool Paused
		{
			get { return _Paused; }
			set { this._Paused = value; }
		}

		public Visual Visual { get { return this.SwapChainVisual; } }

		public Size Size
		{
			get
			{
				if (SwapChain == null)
					return new Size(0, 0);
				return SwapChain.Size;
			}
		}

		public SwapChainRenderer(Compositor compositor)
		{
			this.Compositor = compositor;
			SwapChainVisual = compositor.CreateSpriteVisual();
		}

		public void Dispose()
		{
			DrawLoopCancellationTokenSource?.Cancel();
			SwapChain?.Dispose();
		}

		public void SetDevice(CanvasDevice device, Size windowSize)
		{
			++DeviceCount;

			DrawLoopCancellationTokenSource?.Cancel();

			SwapChain = new CanvasSwapChain(device, 256, 256, 96);
			SwapChainVisual.Brush = Compositor.CreateSurfaceBrush(CanvasComposition.CreateCompositionSurfaceForSwapChain(Compositor, SwapChain));

			DrawLoopCancellationTokenSource = new CancellationTokenSource();
			Task.Factory.StartNew(
				DrawLoop,
				DrawLoopCancellationTokenSource.Token,
				TaskCreationOptions.LongRunning,
				TaskScheduler.Default
			);
		}

		private void DrawLoop()
		{
			var canceled = DrawLoopCancellationTokenSource.Token;

			try
			{
				bool wasPaused = false;
				while (!canceled.IsCancellationRequested)
				{
					bool isPaused = Paused;

					if (!isPaused || isPaused != wasPaused)
					{
						DrawSwapChain(SwapChain, isPaused);
					}
					else
					{
						SwapChain.WaitForVerticalBlank();
					}
				}
			}
			catch (Exception e) when (SwapChain.Device.IsDeviceLost(e.HResult))
			{
				SwapChain.Device.RaiseDeviceLost();
			}
		}

		private void DrawSwapChain(CanvasSwapChain swapChain, bool isPaused)
		{
			++DrawCount;

			using (var ds = swapChain.CreateDrawingSession(Colors.Transparent))
			{
				var size = swapChain.Size.ToVector2();
				var radius = (Math.Min(size.X, size.Y) / 2f) - 4f;

				var center = size / 2;

				ds.FillCircle(center, radius, Colors.LightGoldenrodYellow);
				ds.DrawCircle(center, radius, Colors.LightGray);

				double mu = (-DrawCount / 50f);

				for (int i = 0; i < 16; i++)
				{
					double a = mu + (i / 16f) * Math.PI * 2;
					var x = (float)Math.Sin(a);
					var y = (float)Math.Cos(a);
					ds.DrawLine(center, center + new Vector2(x, y) * radius, Colors.Black, 5);
				}

				var rectLenght = Math.Sqrt(radius * radius * 2);

				ds.FillCircle(center, (float)rectLenght / 2, Colors.LightGoldenrodYellow);

				var rect = new Rect(center.X - rectLenght / 2, center.Y - rectLenght / 2, rectLenght, rectLenght);
			}
		}
	}
}
