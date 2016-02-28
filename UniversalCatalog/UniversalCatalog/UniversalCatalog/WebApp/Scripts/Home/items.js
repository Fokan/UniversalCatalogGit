$(document).ready(function () {
    $(document).on('click', '.item', function () {
        var id = $(this).data('itemid');
        window.location.assign("/Home/Item?ItemId=" + id);
    });
});