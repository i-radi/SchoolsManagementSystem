$(document).ready(function () {
    var $schoolSelect = $("#schoolSelect");

    $("#organizationSelect").on("change", function () {
        var selectedOrganizationId = $(this).val();

        $.ajax({
            url: "/Members/GetSchools",
            type: "GET",
            data: { organizationId: selectedOrganizationId },
            success: function (data) {
                $schoolSelect.empty();
                $.each(data, function (index, school) {
                    $schoolSelect.append($("<option></option>")
                        .attr("value", school.value)
                        .text(school.text));
                });
            }
        });
    });
});

$(document).ready(function () {
    $('#profilePictureInput').change(function () {
        var input = this;
        var preview = $('#imagePreview');

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                preview.attr('src', e.target.result);
                preview.show();
            }

            reader.readAsDataURL(input.files[0]);
        } else {
            preview.hide();
            preview.attr('src', '');
        }
    });

    $('#teacherImageInput').change(function () {
        var input = this;
        var preview = $('#teacherImagePreview');

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                preview.attr('src', e.target.result);
                preview.show();
            }

            reader.readAsDataURL(input.files[0]);
        } else {
            preview.hide();
            preview.attr('src', '');
        }
    });

    $('#studentImageInput').change(function () {
        var input = this;
        var preview = $('#studentImagePreview');

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                preview.attr('src', e.target.result);
                preview.show();
            }

            reader.readAsDataURL(input.files[0]);
        } else {
            preview.hide();
            preview.attr('src', '');
        }
    });
});
