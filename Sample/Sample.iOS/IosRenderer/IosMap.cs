using CoreLocation;
using MapKit;
using Sample.iOS.IosRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Sample.Renderers.MyMaps), typeof(IosMap))]
namespace Sample.iOS.IosRenderer
{
    public class IosMap : MapRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            MKMapView nativeMap = Control as MKMapView;

            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                nativeMap.DidUpdateUserLocation -= UserLocation;
            }
            if (e.NewElement != null)
            {

                nativeMap.UserTrackingMode = MKUserTrackingMode.FollowWithHeading;
                nativeMap.ShowsUserLocation = true;
                nativeMap.ShowsScale = true;
                nativeMap.PitchEnabled = true;
                nativeMap.ShowsBuildings = false;
                nativeMap.ShowsTraffic = true;
                nativeMap.ShowsCompass = true;
                nativeMap.DidUpdateUserLocation += UserLocation;
            }
        }

        private void UserLocation(object sender, MKUserLocationEventArgs e)
        {
            MKMapView nativeMap = Control as MKMapView;
            // Create the camera
            MKMapCamera camera = new MKMapCamera
            {
                Pitch = 80,
                CenterCoordinate = new CLLocationCoordinate2D(e.UserLocation.Location.Coordinate.Latitude, e.UserLocation.Location.Coordinate.Longitude),
                Altitude = 100,
                Heading = e.UserLocation.Heading.HeadingAccuracy,
            };
            nativeMap.SetCamera(camera, true);
        }
    }
}