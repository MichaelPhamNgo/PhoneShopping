var tag = {
    init: function () {
        tag.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var status = $(this);
            var id = status.data('id');
            $.ajax({
                url: "/Admin/Tag/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response)
                    if (response.status == true) {
                        status.text("Processing");
                    } else {
                        status.text("Blocked");
                    }
                }
            });
        });
    }
}
tag.init();