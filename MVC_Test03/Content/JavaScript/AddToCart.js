
    $(document).ready(function () {
        $(".add-to-cart").click(function () {
            var productId = $(this).data("product-id");

            $.ajax({
                url: '@Url.Action("AddToCart", "ShoppingCart")',
                type: 'POST',
                data: { id: productId },
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message); // Hiển thị thông báo thành công
                    } else {
                        toastr.error(response.message); // Hiển thị thông báo lỗi nếu không thành công
                    }
                },
                error: function () {
                    toastr.error("Có lỗi xảy ra, vui lòng thử lại!");
                }
            });
        });
    });
