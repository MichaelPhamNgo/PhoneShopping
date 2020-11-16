var tag = {
    init: function () {
        tag.registerEvents();
    },
    registerEvents: function () {
        $('.change-status').off('click').on('click', function (e) {
            e.preventDefault();
            var status = $(this);
            var id = status.data('id');
            $.ajax({
                url: "/Admin/Tag/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    toastr.success("Change status success.")
                    if (response.status == true) {
                        status.text("Processing");
                        status.removeClass("btn-danger");
                        status.addClass("btn-info");
                    } else {
                        status.text("Block");
                        status.addClass("btn-danger");
                        status.removeClass("btn-info");
                    }
                }
            });
        });
    }
}
tag.init();