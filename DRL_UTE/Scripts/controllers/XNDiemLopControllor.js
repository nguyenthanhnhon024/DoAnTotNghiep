var XNDiemLopControllor = {
    init: function () {
        XNDiemLopControllor.registerEvent();
    },
    registerEvent: function () {
        $('.txtDiem').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                var max = $(this).attr("max");
                var id = $(this).attr("id");
                var value = $(this).val();
                if (value >= 0 && value <= max) {
                    var index = $('.txtDiem').index(this) + 1;
                    $('.txtDiem').eq(index).focus();
                    $('.txtDiem').eq(index).select();

                    XNDiemLopControllor.updateBangDiem(id, value);
                } else {
                    alert("Điểm không được nhỏ hơn \"0\" và lớn hơn \"max\"");
                }

            }
        });
    },
    updateBangDiem: function (id, value) {
        var data = {
            id: id,
            diemLDG: value,
        };
        $.ajax({
            url: '/XNDiemLop/Update',
            type: 'POST',
            dataType: 'json',
            data: { model: JSON.stringify(data) },
            success: function (response) {
                if (response.status) {
                    alert("Cập nhật thành công");
                }
                else {
                    alert(response.message);
                }
            }
        })
    }
}
XNDiemLopControllor.init();