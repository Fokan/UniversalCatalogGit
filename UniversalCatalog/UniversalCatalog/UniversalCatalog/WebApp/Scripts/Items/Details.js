$(document).ready(function () {
    $(document).on('click', '#fileupload', function () {
        $("#file").click();
    });
    $(document).on('click', '#file', function (e) {
        e.stopPropagation();
    });
    $(document).on('change', '#file', function () {
        $('#submitImageForm').click();
    });

    $(document).on('click', '.deleteImage', function () {
        var thisButton = $(this);
        var id = $(this).data("id");
        bootbox.confirm("Are you sure?", function (result) {
            if (result) {
                $.post("/Items/DeleteImage", { ImageId: id, ItemId: $("#ItemId").val() });
                $(thisButton).parent().hide();
            }
        });
    });

    $(document).on('click', "#DescriptionEdit", function () {
        $(".descriptions").removeAttr("disabled");
        $("#DescriptionsSave").show();
    });

    $(document).on('click', "#DescriptionsSave", function () {
        $("#DescriptionsSave").hide();
        var descriptions = document.getElementsByClassName("descriptions");
        var Data = [];
        for (var i = 0; i < descriptions.length; i++) {
            var propertyid = $(descriptions[i]).data("propertyid");
            var value = $(descriptions[i]).val();
            Data.push(new Array(propertyid, value));
            descriptions[i].disabled = true;
        }
        var Id = $("#ItemId").val().toString();
        $.post("/items/EditDescriptions", { "ItemId": Id, "Descriptions": Data });
    });

    $(document).on('click', "#moreImage", function () {
        $("#images").append("<input type='file' name='Images' class='img-thumbnail' />");
    });
});