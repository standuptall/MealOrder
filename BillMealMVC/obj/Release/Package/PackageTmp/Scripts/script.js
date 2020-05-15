
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

    $('.card-home').click(function () {
        window.location.href = "/Products";
    });
});
function mouseEnter() {
    $('.dropdown-menu').dropdown('show');
};
function mouseLeave() {
    $('.dropdown-menu').dropdown('hide');
};


function discountChange(which) {
    var discountamount = $('#DiscountAmount');
    var discountpercent = $('#DiscountPercent');
    if (discountamount.attr('name') == which.attr('name')) {
        discountpercent.val("0");
    }
    if (discountpercent.attr('name') == which.attr('name')) {
        discountamount.val("0");
    }
}

function showloader(value) {
    document.getElementById("loading").style.display = value ? 'block' : 'none';
}