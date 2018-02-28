using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Composition;
using Windows.Graphics;
using Windows.Graphics.DirectX;
using Microsoft.Graphics.Canvas;
using System.Threading;
using Windows.UI.Popups;
using Windows.Graphics.Display;
using Windows.Foundation;
using System.Numerics;
using Microsoft.Graphics.Canvas.UI.Composition;

namespace PlaneSlicerCore
{
	class App : IFrameworkView
	{
		CoreWindow Window;
		Compositor Compositor;
		ContainerVisual RootVisual;
		CompositionTarget CompositionTarget;
		CanvasDevice Device;
		CompositionGraphicsDevice CompositionGraphicsDevice;

		CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
		Random Random = new Random();

		public void Initialize(CoreApplicationView applicationView)
		{
			applicationView.Activated += ApplicationView_Activated;
		}

		public async void SetWindow(CoreWindow window)
		{
			this.Window = window;

			if (!Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2))
			{
				var dialog = new MessageDialog("This version of Windows does not support the Composition APIs.");
				await dialog.ShowAsync();
				CoreApplication.Exit();
				return;
			}

			window.PointerPressed += Window_PointerPressed;

			CoreApplication.Suspending += CoreApplication_Suspending;
			DisplayInformation.DisplayContentsInvalidated += DisplayInformation_DisplayContentsInvalidated;

			this.Compositor = new Compositor();

			CreateDevice();

			// Todo
			// Todo
			// Todo

			this.RootVisual = Compositor.CreateContainerVisual();
			//this.RootVisual.Children.InsertAtTop(//Todo);
			// Todo

			this.CompositionTarget = Compositor.CreateTargetForCurrentView();
			this.CompositionTarget.Root = RootVisual;

			var ignoredTask = UpdateVisualsLoop();
		}

		private async Task UpdateVisualsLoop()
		{
			var token = this.CancellationTokenSource.Token;

			while (!token.IsCancellationRequested)
			{
				// Todo

				await Task.Delay(TimeSpan.FromSeconds(2));
			}
		}

		private void UpdateVisual(Visual visual, Size size)
		{
			UpdateVisualPosition(visual, size);
			UpdateVisualOpacity(visual);
		}

		private void UpdateVisualPosition(Visual visual, Size size)
		{
			var oldOffset = visual.Offset;
			var newOffset = new Vector3(
				(float)(Random.NextDouble() * (this.Window.Bounds.Width - size.Width)),
				(float)(Random.NextDouble() * (this.Window.Bounds.Height - size.Height)),
				0
			);
			visual.Offset = newOffset;
			visual.Size = size.ToVector2();

			AnimateOffset(visual, oldOffset, newOffset);
		}

		private void UpdateVisualOpacity(Visual visual)
		{
			var oldOpacity = visual.Opacity;
			var newOpacity = (float)Random.NextDouble();

			var animation = this.Compositor.CreateScalarKeyFrameAnimation();
			animation.InsertKeyFrame(0, oldOpacity);
			animation.InsertKeyFrame(1, newOpacity);

			visual.Opacity = newOpacity;
			visual.StartAnimation("Opacity", animation);
		}

		private void AnimateOffset(Visual visual, Vector3 oldOffset, Vector3 newOffset)
		{
			var animation = this.Compositor.CreateVector3KeyFrameAnimation();
			animation.InsertKeyFrame(0, oldOffset);
			animation.InsertKeyFrame(1, newOffset);
			animation.Duration = TimeSpan.FromSeconds(1);

			visual.StartAnimation("Offset", animation);
		}

		private void CreateDevice()
		{
			this.Device = CanvasDevice.GetSharedDevice();
			Device.DeviceLost += Device_DeviceLost;

			if (this.CompositionGraphicsDevice == null)
			{
				CompositionGraphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(Compositor, Device);
			}
			else
			{
				CanvasComposition.SetCanvasDevice(CompositionGraphicsDevice, Device);
			}

			// Todo
			//if ()
			//{

			//}
		}

		public void Load(string entryPoint)
		{
		}

		public void Run()
		{
			CoreWindow.GetForCurrentThread().Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);
		}

		public void Uninitialize()
		{
			// Todo
			this.CancellationTokenSource.Cancel();
		}

		private void Device_DeviceLost(CanvasDevice sender, object args)
		{
			Device.DeviceLost -= Device_DeviceLost;
			var unwiatedTask = Window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				CreateDevice()
			);
		}

		private void Window_PointerPressed(CoreWindow sender, PointerEventArgs args)
		{
			//throw new NotImplementedException();
		}

		private void CoreApplication_Suspending(object sender, SuspendingEventArgs e)
		{
			// Todo
		}

		private void ApplicationView_Activated(CoreApplicationView sender, Windows.ApplicationModel.Activation.IActivatedEventArgs args)
		{
			CoreWindow.GetForCurrentThread().Activate();
		}

		private void DisplayInformation_DisplayContentsInvalidated(DisplayInformation sender, object args)
		{
			CanvasDevice.GetSharedDevice();
		}

	}

	class ViewSource : IFrameworkViewSource
	{
		public IFrameworkView CreateView()
		{
			return new App();
		}

		public void Main(string[] args)
		{
			CoreApplication.Run(new ViewSource());
		}
	}
}
