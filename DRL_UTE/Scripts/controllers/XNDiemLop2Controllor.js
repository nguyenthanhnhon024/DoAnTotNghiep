var XNDiemLop2Controllor = {
    init: function () {
        XNDiemLop2Controllor.registerEvent();
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

                    XNDiemLop2Controllor.updateBangDiem(id, value);
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
XNDiemLop2Controllor.init();