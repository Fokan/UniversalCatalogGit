$(document).ready(function () {
    getprops();
    $(document).on('change', "#TypeId", function () {
        getprops();
    });
    $(document).on('click', "#moreImage", function () {
        $("#images").append("<input type='file' name='Images' />");
    });

});

function getprops() {
    Id = $("#TypeId").val();
    var Properties = [];
    var PropsPlace = document.getElementById("properties");
    PropsPlace.innerHTML = "";
    $.ajax({
        url: "/Items/GetPropertiesByType",
        cache: false,
        async: false,
        data: { "TypeId": Id },
        success: function (data) {
            Properties = [];
            for (var i = 0; i < data.length; i++) {
                PropsPlace.innerHTML +=
                "<div class='form-group'><label class = 'control-label col-md-2'>" + data[i].Name +
                "</label><div class='col-md-10'>" +
                "<input type='hidden' name='IdsOfProps' value='" + data[i].Id + "' />" +
                "<textarea type='text' class = 'form-control' name='props' value='' ></textarea></div></div>";

            }
        }
    });
}