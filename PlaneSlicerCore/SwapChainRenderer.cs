using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
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

					if (true)
					{

					}
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
