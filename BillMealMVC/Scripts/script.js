
/* Edit product -->
 */
function img_onclick() {
    $('#file-input').click();
}
/* Edit product <--
 */

function cart_reload(cart_item_id, input) {
    var qty = input.value;
    window.location.href = "/checkout/review?id=" + cart_item_id+"&qty="+qty;
}

$(document).ready(function () {
    $('#cart_button').hover(mouseEnter, mouseLeave);
    $('#cart_button').click(function(){
        window.location.href = "/checkout"
    });

    $('.clickable-row').click(function () {
        window.location = $(this).data("href");
    });
});
function mouseEnter() {
    $('.dropdown-menu').dropdown('show');
};
function mouseLeave() {
    $('.dropdown-menu').dropdown('hide');
};