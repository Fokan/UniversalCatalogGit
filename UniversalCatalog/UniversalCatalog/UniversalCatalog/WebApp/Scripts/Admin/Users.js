$(document).ready(function () {
    $('#UserDataTable').DataTable();
    $(document).on('click', '.deleteUser', function () {
        var thisButton = $(this);
        bootbox.confirm("Are you sure?", function (result) {
            if (result) {
                var id = $(thisButton).data('userid');
                $.post("/Admin/DeleteUser", { UserId: id });
                $(thisButton).closest("tr").hide();
            }
        });
    });
});