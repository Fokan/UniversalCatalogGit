jQuery(document).ready(function ($) {

    var jssor_1_options = {
        $AutoPlay: true,
        $ArrowNavigatorOptions: {
            $Class: $JssorArrowNavigator$
        },
        $ThumbnailNavigatorOptions: {
            $Class: $JssorThumbnailNavigator$,
            $Cols: 9,
            $SpacingX: 3,
            $SpacingY: 3,
            $Align: 260
        }
    };

    var jssor_1_slider = new $JssorSlider$("jssor_1", jssor_1_options);

    //responsive code begin
    //you can remove responsive code if you don't want the slider scales while window resizing
    function ScaleSlider() {
        var refSize = jssor_1_slider.$Elmt.parentNode.clientWidth;
        if (refSize) {
            refSize = Math.min(refSize, 600);
            jssor_1_slider.$ScaleWidth(refSize);
        }
        else {
            window.setTimeout(ScaleSlider, 30);
        }
    }
    ScaleSlider();
    $(window).bind("load", ScaleSlider);
    $(window).bind("resize", ScaleSlider);
    $(window).bind("orientationchange", ScaleSlider);
    //responsive code end
});

$(document).ready(function () {
    $(document).on('click', "#btnSend", function () {
        var id = $("#ItemId").val();
        var message = $("#newComment").val();
        if (message == "")
            return;
        $.ajax({
            url: "NewComment",
            data: { ItemId: id, Comment: message },
            success: function (data) {
                var date = data.date;
                var id = data.id;
                var admin = data.admin;
                var delIcon = "";
                if (admin == true) {
                    delIcon = "<span data-id=" + id + " class='delIcon'>&#10006;</span>";
                }
                $("#comments").append("<div class='comment panel panel-default'>" +
                                    "<div class='panel-heading'>" +
                                        "<span class='panel-title'>@User.Identity.Name</span>" +
                                        "<span class='pull-right'>" + date + delIcon + "</span>" +
                                    "</div>" +
                                    "<div class='panel-body'>" +
                                    "<p>" + message + "</p>" +
                                    "</div>" +
                                "</div>");
                $("#newComment").val("");
            }
        });
    });
    $(document).on('click', '.delIcon', function () {
        var commentId = $(this).data("id");
        $.post("/Admin/DeleteComment", { CommentId: commentId });
        $(this).closest(".comment").hide();

    });
});