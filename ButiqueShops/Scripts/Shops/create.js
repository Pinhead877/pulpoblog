function initMap() {
    lat = lat == null ? 32.07777472041461 : lat;
    lng = lng == null ? 34.774324893951416 : lng;
    var map = new GMaps({
        div: '#map',
        zoom: 10,
        lat: lat,
        lng: lng
    });
    map.addMarker({
        lat: lat,
        lng: lng
    });

    GMaps.on('click', map.map, function (event) {
        lat = event.latLng.lat();
        lng = event.latLng.lng();
        $('#Latitude').val(lat);
        $('#Longitude').val(lng);
        map.removeMarkers();
        map.addMarker({
            lat: lat,
            lng: lng,
            title: "Shop Location"
        });
    });

}