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
using Windows.ApplicationModel.Activation;

namespace PlaneSlicerCore
{
	//class App : IFrameworkView
	//{
	//	CoreWindow Window;
	//	Compositor Compositor;
	//	ContainerVisual RootVisual;
	//	CompositionTarget CompositionTarget;
	//	CanvasDevice Device;
	//	CompositionGraphicsDevice CompositionGraphicsDevice;

	//	SwapChainRenderer SwapChainRenderer;

	//	CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
	//	Random Random = new Random();

	//	public void Initialize(CoreApplicationView applicationView)
	//	{
	//		applicationView.Activated += ApplicationView_Activated;
	//	}

	//	public async void SetWindow(CoreWindow window)
	//	{
	//		this.Window = window;

	//		if (!Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2))
	//		{
	//			var dialog = new MessageDialog("This version of Windows does not support the Composition APIs.");
	//			await dialog.ShowAsync();
	//			CoreApplication.Exit();
	//			return;
	//		}

	//		window.PointerPressed += Window_PointerPressed;

	//		CoreApplication.Suspending += CoreApplication_Suspending;
	//		DisplayInformation.DisplayContentsInvalidated += DisplayInformation_DisplayContentsInvalidated;

	//		this.Compositor = new Compositor();

	//		CreateDevice();

	//		this.SwapChainRenderer = new SwapChainRenderer(Compositor);
	//		this.SwapChainRenderer.SetDevice(this.Device, new Size(window.Bounds.Width, window.Bounds.Height));

	//		this.SwapChainRenderer.Visual.Offset = new Vector3((float)window.Bounds.Width, (float)window.Bounds.Height, 0);
			
	//		this.RootVisual = Compositor.CreateContainerVisual();
	//		this.RootVisual.Children.InsertAtTop(SwapChainRenderer.Visual);

	//		this.CompositionTarget = Compositor.CreateTargetForCurrentView();
	//		this.CompositionTarget.Root = RootVisual;

	//		var ignoredTask = UpdateVisualsLoop();
	//	}

	//	private async Task UpdateVisualsLoop()
	//	{
	//		var token = this.CancellationTokenSource.Token;

	//		while (!token.IsCancellationRequested)
	//		{
	//			UpdateVisual(SwapChainRenderer.Visual, SwapChainRenderer.Size);

	//			await Task.Delay(TimeSpan.FromSeconds(2));
	//		}
	//	}

	//	private void UpdateVisual(Visual visual, Size size)
	//	{
	//		UpdateVisualPosition(visual, size);
	//		UpdateVisualOpacity(visual);
	//	}

	//	private void UpdateVisualPosition(Visual visual, Size size)
	//	{
	//		var oldOffset = visual.Offset;
	//		var newOffset = new Vector3(
	//			(float)(Random.NextDouble() * (this.Window.Bounds.Width - size.Width)),
	//			(float)(Random.NextDouble() * (this.Window.Bounds.Height - size.Height)),
	//			0
	//		);
	//		visual.Offset = newOffset;
	//		visual.Size = size.ToVector2();

	//		AnimateOffset(visual, oldOffset, newOffset);
	//	}

	//	private void UpdateVisualOpacity(Visual visual)
	//	{
	//		var oldOpacity = visual.Opacity;
	//		var newOpacity = (float)Random.NextDouble();

	//		var animation = this.Compositor.CreateScalarKeyFrameAnimation();
	//		animation.InsertKeyFrame(0, oldOpacity);
	//		animation.InsertKeyFrame(1, newOpacity);

	//		visual.Opacity = newOpacity;
	//		visual.StartAnimation("Opacity", animation);
	//	}

	//	private void AnimateOffset(Visual visual, Vector3 oldOffset, Vector3 newOffset)
	//	{
	//		var animation = this.Compositor.CreateVector3KeyFrameAnimation();
	//		animation.InsertKeyFrame(0, oldOffset);
	//		animation.InsertKeyFrame(1, newOffset);
	//		animation.Duration = TimeSpan.FromSeconds(1);

	//		visual.StartAnimation("Offset", animation);
	//	}

	//	private void CreateDevice()
	//	{
	//		this.Device = CanvasDevice.GetSharedDevice();
	//		Device.DeviceLost += Device_DeviceLost;

	//		if (this.CompositionGraphicsDevice == null)
	//		{
	//			CompositionGraphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(Compositor, Device);
	//		}
	//		else
	//		{
	//			CanvasComposition.SetCanvasDevice(CompositionGraphicsDevice, Device);
	//		}

	//		if (SwapChainRenderer != null)
	//		{
	//			SwapChainRenderer.SetDevice(Device, new Size(Window.Bounds.Width, Window.Bounds.Height));
	//		}
	//	}

	//	public void Load(string entryPoint)
	//	{
	//	}

	//	public void Run()
	//	{
	//		CoreWindow.GetForCurrentThread().Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);
	//	}

	//	public void Uninitialize()
	//	{
	//		this.SwapChainRenderer?.Dispose();
	//		this.CancellationTokenSource.Cancel();
	//	}

	//	private void Device_DeviceLost(CanvasDevice sender, object args)
	//	{
	//		Device.DeviceLost -= Device_DeviceLost;
	//		var unwiatedTask = Window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
	//			CreateDevice()
	//		);
	//	}

	//	private void Window_PointerPressed(CoreWindow sender, PointerEventArgs args)
	//	{
	//		//throw new NotImplementedException();
	//	}

	//	private void CoreApplication_Suspending(object sender, SuspendingEventArgs e)
	//	{
	//		// Todo
	//	}

	//	private void ApplicationView_Activated(CoreApplicationView sender, Windows.ApplicationModel.Activation.IActivatedEventArgs args)
	//	{
	//		CoreWindow.GetForCurrentThread().Activate();
	//	}

	//	private void DisplayInformation_DisplayContentsInvalidated(DisplayInformation sender, object args)
	//	{
	//		CanvasDevice.GetSharedDevice();
	//	}

	//}

	class App : IFrameworkView
	{
		CoreWindow Window;
		Compositor Compositor;
		ContainerVisual RootVisual;
		CompositionTarget CompositionTarget;
		CanvasDevice Device;
		CompositionGraphicsDevice CompositionGraphicsDevice;

		SwapChainRenderer SwapChainRenderer;

		CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
		Random Random = new Random();

		public void Initialize(CoreApplicationView applicationView)
		{
			applicationView.Activated += ApplicationView_Activated;
		}

		public void Uninitialize()
		{
			SwapChainRenderer?.Dispose();
			CancellationTokenSource.Cancel();
		}

		void ApplicationView_Activated(CoreApplicationView sender, IActivatedEventArgs args)
		{
			CoreWindow.GetForCurrentThread().Activate();
		}

		public void Load(string entryPoint)
		{
		}

		public void Run()
		{
			CoreWindow.GetForCurrentThread().Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);
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

			Compositor = new Compositor();

			CreateDevice();

			SwapChainRenderer = new SwapChainRenderer(Compositor);
			SwapChainRenderer.SetDevice(Device, new Size(window.Bounds.Width, window.Bounds.Height));

			SwapChainRenderer.Visual.Offset = new Vector3((float)window.Bounds.Width, (float)window.Bounds.Height, 0);

			RootVisual = Compositor.CreateContainerVisual();
			RootVisual.Children.InsertAtTop(SwapChainRenderer.Visual);

			CompositionTarget = Compositor.CreateTargetForCurrentView();
			CompositionTarget.Root = RootVisual;

			var ignoredTask = UpdateVisualsLoop();
		}

		async Task UpdateVisualsLoop()
		{
			var token = CancellationTokenSource.Token;

			while (!token.IsCancellationRequested)
			{
				UpdateVisual(SwapChainRenderer.Visual, SwapChainRenderer.Size);

				await Task.Delay(TimeSpan.FromSeconds(2));
			}
		}

		void UpdateVisual(Visual visual, Size size)
		{
			UpdateVisualPosition(visual, size);
			UpdateVisualOpacity(visual);
		}

		void UpdateVisualPosition(Visual visual, Size size)
		{
			var oldOffset = visual.Offset;
			var newOffset = new Vector3(
				(float)(Random.NextDouble() * (Window.Bounds.Width - size.Width)),
				(float)(Random.NextDouble() * (Window.Bounds.Height - size.Height)),
				0);

			visual.Offset = newOffset;
			visual.Size = size.ToVector2();

			AnimateOffset(visual, oldOffset, newOffset);
		}

		void UpdateVisualOpacity(Visual visual)
		{
			var oldOpacity = visual.Opacity;
			var newOpacity = (float)Random.NextDouble();

			var animation = Compositor.CreateScalarKeyFrameAnimation();
			animation.InsertKeyFrame(0, oldOpacity);
			animation.InsertKeyFrame(1, newOpacity);

			visual.Opacity = newOpacity;
			visual.StartAnimation("Opacity", animation);
		}

		void AnimateOffset(Visual visual, Vector3 oldOffset, Vector3 newOffset)
		{
			var animation = Compositor.CreateVector3KeyFrameAnimation();
			animation.InsertKeyFrame(0, oldOffset);
			animation.InsertKeyFrame(1, newOffset);
			animation.Duration = TimeSpan.FromSeconds(1);

			visual.StartAnimation("Offset", animation);
		}

		void Window_PointerPressed(CoreWindow sender, PointerEventArgs args)
		{
			SwapChainRenderer.Paused = !SwapChainRenderer.Paused;
		}

		void CoreApplication_Suspending(object sender, SuspendingEventArgs args)
		{
			try
			{
				Device.Trim();
			}
			catch (Exception e) when (Device.IsDeviceLost(e.HResult))
			{
				Device.RaiseDeviceLost();
			}
		}

		void CreateDevice()
		{
			Device = CanvasDevice.GetSharedDevice();
			Device.DeviceLost += Device_DeviceLost;

			if (CompositionGraphicsDevice == null)
			{
				CompositionGraphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(Compositor, Device);
			}
			else
			{
				CanvasComposition.SetCanvasDevice(CompositionGraphicsDevice, Device);
			}

			if (SwapChainRenderer != null)
				SwapChainRenderer.SetDevice(Device, new Size(Window.Bounds.Width, Window.Bounds.Height));
		}

		void Device_DeviceLost(CanvasDevice sender, object args)
		{
			Device.DeviceLost -= Device_DeviceLost;
			var unwaitedTask = Window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => CreateDevice());
		}

		void DisplayInformation_DisplayContentsInvalidated(DisplayInformation sender, object args)
		{
			// The display contents could be invalidated due to a lost device, or for some other reason.
			// We check this by calling GetSharedDevice, which will make sure the device is still valid before returning it.
			// If the shared device has been lost, GetSharedDevice will automatically raise its DeviceLost event.
			CanvasDevice.GetSharedDevice();
		}
	}

	class ViewSource : IFrameworkViewSource
	{
		public IFrameworkView CreateView()
		{
			return new App();
		}

		public static void Main(string[] args)
		{
			CoreApplication.Run(new ViewSource());
		}
	}
}
