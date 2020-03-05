using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Sample.Droid.DroidRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Sample.Renderers.MyMaps), typeof(DroidMap))]

namespace Sample.Droid.DroidRenderer
{
    public class DroidMap : MapRenderer
    {
        private bool IsMoving = false;
        public DroidMap(Context context) : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            this.NativeMap.MyLocationChange -= LocationChange;
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            base.OnElementChanged(e);
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);
            this.NativeMap.MyLocationChange += LocationChange;
            this.NativeMap.CameraIdle += CameraIdle;
        }

        private void CameraIdle(object sender, System.EventArgs e)
        {
            IsMoving = false;
        }

        private void LocationChange(object sender, GoogleMap.MyLocationChangeEventArgs e)
        {
            if (IsMoving)
            {
                return;
            }
            else
            {
                IsMoving = true;
            }
            CameraPosition cameraPosition = new CameraPosition(target: new LatLng(e.Location.Latitude, e.Location.Longitude), zoom: 20f, tilt: 45f, bearing: e.Location.Bearing);
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            this.NativeMap.AnimateCamera(cameraUpdate);
        }
    }
}