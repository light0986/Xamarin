using Android.App;
using Android.Views;
using System;
using Android.Hardware;
using Android.Runtime;
using Android.Graphics;
using Android.OS;
using Camera = Android.Hardware.Camera;
using System.IO;
using Android.Widget;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Android.Media;

namespace AndroidApp1.Othres
{
    public class CameraPV : SurfaceView, ISurfaceHolderCallback
    {
        Activity _Contex;
        [Obsolete]
        Camera _Camera;

        [Obsolete]
        public CameraPV(Activity contex, Camera camera): base(contex)
        {
            this.Holder.AddCallback(this);
            this.Holder.SetType(SurfaceType.PushBuffers);
            _Contex = contex;
            _Camera = camera;
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
            
        }

        [Obsolete]
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            _Camera.SetPreviewDisplay(this.Holder);
            Camera.CameraInfo caminfo = new Camera.CameraInfo();
            Camera.GetCameraInfo(0 ,caminfo );
            var rotation = _Contex.WindowManager.DefaultDisplay.Rotation;
            int degrees = 0;
            switch (rotation)
            {
                case SurfaceOrientation.Rotation0:
                    degrees = 0; break;

                case SurfaceOrientation.Rotation90:
                    degrees = 90; break;

                case SurfaceOrientation.Rotation180:
                    degrees = 180; break;

                case SurfaceOrientation.Rotation270:
                    degrees = 270; break;
            }

            if(caminfo.Facing == CameraFacing.Front)
            {
                _Camera.SetDisplayOrientation(
                    (caminfo.Orientation - degrees + 360) % 360);
            }
            _Camera.StartPreview();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            
        }
    }

    [Obsolete]
    public class CamerFragment : Fragment
    {
        public Camera Camera { get; set; }

        [Obsolete]
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Platform.Init(this.Activity, savedInstanceState);
            Camera = Camera.Open();
            CameraPV view = new CameraPV(this.Activity, Camera);

            return view;
        }

        [Obsolete]
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    [Obsolete]
    public class TakePictureCallBack : Java.Lang.Object, Camera.IPictureCallback
    {
        Activity _Contex;

        public TakePictureCallBack(Activity context)
        {
            _Contex = context;
        }

        public async void OnPictureTaken(byte[] data, Camera camera)
        {
            var sdCardPath = Application.Context.GetExternalFilesDir(null).AbsolutePath;
            var PicPath = System.IO.Path.Combine(sdCardPath , "pic.jpg");

            using (var file = File.Create(PicPath))
            {
                file.Write(data, 0, data.Length);
            }

            Toast.MakeText(_Contex, "拍照完成", ToastLength.Short).Show();
            await Task.Delay(500);
            camera.StartPreview();
        }
    }

    [Obsolete]
    public class SurfaceHolderCallBack : Java.Lang.Object, ISurfaceHolderCallback
    {
        public int RotataDegrees {  get; set; }
        Camera _Camera;
        Activity _Context;

        public SurfaceHolderCallBack(Activity context, Camera camera, MediaRecorder mediaRecodr)
        {
            _Context = context;
            _Camera = camera;
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
            
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            _Camera.SetPreviewDisplay(holder);
            Camera.CameraInfo cameraInfo = new Camera.CameraInfo();
            Camera.GetCameraInfo(0, cameraInfo);
            var rotation = _Context.WindowManager.DefaultDisplay.Rotation;
            int degrees = 0;
            switch (rotation)
            {
                case SurfaceOrientation.Rotation0:
                    degrees = 0; break;
                
                case SurfaceOrientation.Rotation90:
                    degrees = 90; break;

                case SurfaceOrientation.Rotation180:
                    degrees = 180; break;

                case SurfaceOrientation.Rotation270:
                    degrees = 270; break;
            }
            RotataDegrees = (cameraInfo.Orientation = degrees + 360) % 360;
            _Camera.SetDisplayOrientation(RotataDegrees);
            _Camera.StartPreview();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            
        }
    }

    public class SurfaceHolderCallBack_Two : Java.Lang.Object, ISurfaceHolderCallback
    {
        public int RotataDegrees { get; set; }

        [Obsolete]
        Camera _Camera;
        Activity _Context;

        [Obsolete]
        public SurfaceHolderCallBack_Two(Activity context, Camera camera, MediaRecorder mediaRecodr)
        {
            _Context = context;
            _Camera = camera;
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {

        }

        [Obsolete]
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            _Camera.SetPreviewDisplay(holder);
            Camera.CameraInfo cameraInfo = new Camera.CameraInfo();
            Camera.GetCameraInfo(0, cameraInfo);
            _Camera.StartPreview();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {

        }
    }
}