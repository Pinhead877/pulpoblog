function likeItem(itemid) {
    $.ajax({
        url: "/services/likeItem?itemid=" + itemid,
        success: function (result) {
            if (result.error) {

            } else if (result === true) {
                $('#like').removeClass('fa-heart-o').addClass('fa-heart red');
                $('#likecount').html(parseInt($('#likecount').html()) + 1);
            } else if (result === false) {
                $('#like').removeClass('fa-heart red').addClass('fa-heart-o');
                $('#likecount').html(parseInt($('#likecount').html())-1);
            }
        }
    });
}