
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