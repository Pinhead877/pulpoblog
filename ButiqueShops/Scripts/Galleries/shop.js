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
}

function likeStore(shopid){
    $.ajax({
        url: "/services/likestore?shopid="+shopid,
        success: function (result) {
            if (result.error) {

            } else if(result === true) {
                $('#like').removeClass('fa-heart-o').addClass('fa-heart red');
            } else if (result === false) {
                $('#like').removeClass('fa-heart red').addClass('fa-heart-o');
            }
        }
    });
}