var ChamDRLController = {
    init: function () {
        ChamDRLController.LoadData();
        ChamDRLController.registerEvent();
    },
    registerEvent: function () {
        $('.txtDiem').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                var max = $(this).data('max');
                var id = $(this).data('id');
                var value = $(this).val();

                if (value >= 0 && value <= max) {
                    var index = $('.txtDiem').index(this) + 1;
                    $('.txtDiem').eq(index).focus();
                    $('.txtDiem').eq(index).select();

                    ChamDRLController.updateBangDiem(id, value);
                } else {
                    alert("Điểm không được nhỏ hơn \"0\" và lớn hơn \"max\"");
                }

            }
        });
    },
    updateBangDiem: function (id, value) {
        var data = {
            id: id,
            diemTDG: value,
        };
        $.ajax({
            url: '/ChamDRL/Update',
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
    },
    LoadData: function () {
        $.ajax({
            url: '/ChamDRL/LoadData',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    var stt = 0;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            stt: stt = stt+1,
                            id: item.id,
                            noiDung: item.noiDung,
                            diemMax: item.diemMax,
                            diemTDG: item.diemTDG,
                            //canMinhChung: item.canMinhChung == 1 ? "<span class=\"badge badge-sm bg-gradient-success\">Cần</span>" : "<span class=\"badge badge-sm bg-gradient-secondary\">Không</span>",
                            minhChung: item.canMinhChung == 1 ? "<img src=\"/assets/imgMC/" + item.minhChung + "\" id=\"pictureUpload\" height=\"90\" width=\"90\" alt=\"Minh chứng\"/></br><a class=\"btn btn-danger btn-sm\" id=" + item.id + " href=\"/SinhVien/DRL/Edit/" + item.id + "\">Upload</a>" :""
                           // minhChung: item.minhChung
                                
                        });
                    });
                    $('#tblData').html(html);
                    ChamDRLController.registerEvent();
                }
            }
        })
    }
}
ChamDRLController.init();