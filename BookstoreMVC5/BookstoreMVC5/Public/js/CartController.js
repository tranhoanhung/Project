function deleteitem(bookid) {
    $.ajax({
        url: '/Cart/deleteitem?bookid=' + bookid,
        type: 'GET',
        beforeSend: function () {
            $(".ajaxload-cart").fadeIn("fast");
            $(".ajaxload-cart").fadeOut("slow");
        },
        success: function (data) {
            console.log(data);
            $("#cartheaderItem_" + bookid + "").fadeOut("slow");
            $("#cartItem_" + bookid + "").fadeOut("slow");

            var totalCart = formatter.format(data.priceTotal).replace(/\,00 ₫/, '');
            $('#totalCart').text(totalCart + " VND");
            $('#CountCart').text(data.countCart);
            alertify.success("<i class='fas fa-check text-success '></i> Đã xóa 1 sản phẩm");
        }
    })
}


var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnUpdate').off('click').on('click', function () {
            var listProduct = $('.txtQuantity');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    quantity: $(item).val(),
                    book: {
                        ID: $(item).data('id')
                    }
                });
            });

            $.ajax({
                url: '/Cart/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                beforeSend: function () {
                    //$(".ajaxload-cart").fadeIn("fast");
                    //$(".ajaxload-cart").fadeOut("slow");
                },
                success: function (data) {
                    console.log(data);
                    if (data.status == true) {
                        
                        window.location.href = "/gio-hang";
                    }
                    alertify.success("<i class='fas fa-check text-success '></i> Đã thay đổi thành công");
                }
            })
        });

        $('#Addtocart').off('click').on('click', function () {
            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });

        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { id: $(this).data('id') },
                url: '/Cart/Delete',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            })
        });
    }
}
cart.init();