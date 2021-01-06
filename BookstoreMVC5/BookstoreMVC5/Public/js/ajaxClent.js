const formatter = new Intl.NumberFormat('VND', {
    style: 'currency',
    currency: 'VND',
    minimumFractionDigits: 2
})
function checklogin() {
    alertify.warning("<i class='fas fa-check text-success '></i> Vui lòng đăng nhập.");
    return false;
}
function additembook(UserId,productID) {

    var quantity = $("#quantity").val();
    $.ajax({
        url: '/them-gh?userID=' + UserId + '&productID=' + productID + '&quantity=' + quantity + '',
        type: 'GET',
        beforeSend: function () {
            $('.loading-ajax-effect').fadeIn('slow');
            $('.loading-ajax-effect').fadeOut('slow');
        },
        success: function (data) {

            alertify.success("<i class='fas fa-check text-success '></i> Đã thêm vào giỏ");


        }
    })
}
function addcart(productID) {

    var quantity = $("#quantity").val();
    $.ajax({
        url: '/them-gio-hang?productID=' + productID + '&quantity=' + quantity + '',
        type: 'GET',
        beforeSend: function () {
            $('.loading-ajax-effect').fadeIn('slow');
            $('.loading-ajax-effect').fadeOut('slow');
        },
        success: function (data) {
            if (data.meThod == "updateQuantity") {
                if (data.bad == true) {
                    alertify.error("<span class='font-weight-bold'>Thông báo: </span> Vượt quá số lượng trong kho");
                    return false;
                }
                else {
                    if (data.priceSale > 0) {
                        var sum = data.ProductPrice * data.quantity;
                    }
                    else {
                        var sum = data.price * data.quantity;
                    }
                    var totalCart = formatter.format(data.priceTotol).replace(/\,00 ₫/, '');
                    $('#totalCart').text(totalCart + " VND");
                    $("#CountCart_" + data.productID + "").text(data.quantity);
                }
            }
            //
            else if (data.meThod == "cartExist") {
                if (data.f == true) {
                    alert("Bạn đã chọn số lượng lớn hơn số lượng hiện có trong kho:");
                    return false;
                } else {
                    var totalCart = formatter.format(data.priceTotal).replace(/\,00 ₫/, '');
                    $('#totalCart').text(totalCart + " VND");
                    $('#CountCart').text(data.countCart);

                    if (data.product.pricesale > 0) {
                        var sum = data.priceSaleee * data.quantity;
                    }
                    else {
                        var sum = data.product.price * data.quantity;
                    }
                    var productItem = '<div class="single-cart-block" id="cartheaderItem_' + data.product.ID + '">' +
                        '<div class="cart-product">' +
                        '<a href="#" class="image">' +
                        '<img src="/public/images/product/' + data.product.img + '" alt="">' +
                        '</a>' +
                        '<div class="content">' +
                        '<h3 class="title">' +
                        '<a href="#">' +
                        '' + data.product.name + '' +
                        '</a>' +
                        '</h3>' +
                        '<p class="price"><span class="qty price" id="CountCart_' + data.product.ID + '">' + data.quantity + ' </span> × <span id="sum_' + data.product.ID + '"> ' + data.priceSaleee + ' </span></p>' +
                        ' <a href="javascript:void(0);" onclick="deleteItem(' + data.product.ID + ')" class="cross-btn"><i class="fas fa-times"></i></a>' +
                        '</div>' +
                        '</div>' +
                        '</div>';
                    $("#cardheaderAppenIemId").prepend(productItem);
                }
            }
            //done
            else if (data.meThod == "cartEmpty") {
                if (data.f == true) {
                    alert("Bạn đã chọn số lượng lớn hơn số lượng hiện có trong kho:");
                    return false;
                }
                else {
                    var totalCart = formatter.format(data.priceTotal).replace(/\,00 ₫/, '');
                    $('#totalCart').text(totalCart + " VND");
                    $('#CountCart').text("1");

                    var productItem = '<div class="single-cart-block" id="cartheaderItem_' + data.product.ID + '">' +
                        '<div class="cart-product">' +
                        '<a href="#" class="image">' +
                        '<img src="/public/images/product/' + data.product.img + '" alt="">' +
                        '</a>' +
                        '<div class="content">' +
                        '<h3 class="title">' +
                        '<a href="#">' +
                        '' + data.product.name + '' +
                        '</a>' +
                        '</h3>' +
                        '<p class="price"><span class="qty price" id="CountCart_' + data.product.ID + '">' + data.quantity + ' </span> × <span id="sum_' + data.product.ID + '"> ' + data.priceSaleee + ' </span></p>' +
                        ' <a href="javascript:void(0);" onclick="deleteItem(' + data.product.ID + ')" class="cross-btn"><i class="fas fa-times"></i></a>' +
                        '</div>' +
                        '</div>' +
                        '</div>';
                    $("#cardheaderAppenIemId").prepend(productItem);
                }
            }
            alertify.success("<i class='fas fa-check text-success '></i> Đã thêm vào giỏ");
           

        }
    })
}




