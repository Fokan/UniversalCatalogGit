$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();

    $(document).on('click', '.deleteProperty', function () {
        var selectButton = $(this);
        bootbox.confirm("Are you sure?", function (result) {
            if (result) {
                $.get("/Types/RemoveProperty", { TypeId: $("#TypeId").val(), PropertyId: $(selectButton).data('propertyid') });
                $(selectButton).closest("tr").hide();
            }
        });
    });

    $(document).on('click', '#newProperty', function () {
        bootbox.dialog({
            title: 'Add new property',
            buttons: {
                close: {
                    label: 'Close',
                    className: "btn-danger",
                    callback: function () { }
                },
                doAdd: {
                    label: 'Create',
                    className: "btn-success",
                    callback: function () {
                        AddProperty();

                    }
                }
            },
            message: '<div class="form-group"><label class = "control-label">Name</label><div >' +
                '<input type="text" name="PropertyName" class="form-control" id="PropertyName" required/></div></div>'
        });
    });
});

function AddProperty() {
    var name = $("#PropertyName").val();
    if (name == '') {
        toastr.error("Input name required!", "Add new property");
    }
    $.ajax({
        url: "/Types/AddProperty",
        data: { "TypeId": $("#TypeId").val(), "PropertyName": name },
        contentType: "application/json",
        success: function (data) {
            var error = data.Error;
            var id = data.Id;
            if (error) {
                toastr.error("Porperty with name '" + name + "' already exists!", "Add new property");
            }
            else {
                var PropertyTr = '<tr><td>' + name + '</td><td><button type="button" class="btn btn-primary deleteProperty pull-right" data-propertyid=' + id + '>x</button></td></tr>';
                $(".table").children("tbody").append(PropertyTr);
                toastr.success("Property '" + name + "' created successfully");
            }
        }
    });
}