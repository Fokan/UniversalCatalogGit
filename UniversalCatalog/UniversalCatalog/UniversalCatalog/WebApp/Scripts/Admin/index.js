$(document).ready(function () {
    $(document).on('click', '.imgDelete', function () {
        var thisButton = $(this);
        var src = $(this).data("img");
        bootbox.confirm("Are you sure?", function (result) {
            if (result) {
                $.post("/Admin/DeleteSlideImage", { ImageSRC: src });
                $(thisButton).parent(".img-thumbnail").hide();
            }
        });

    });

    $(document).on('click', '#fileupload', function () {
        $("#file").click();
    });
    $(document).on('click', '#file', function (e) {
        e.stopPropagation();
    });
    $(document).on('change', '#file', function () {
        $('#submitImageForm').click();
    });


});